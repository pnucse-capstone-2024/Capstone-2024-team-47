using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.Buffer;
using System.Threading;

namespace NetworkCore
{
    public abstract class Session
    {
        Socket _socket;
        int _disconnect = 0;

        RecvBuffer _recvBuffer = new(65535);

        object _lock = new();
        Queue<ArraySegment<byte>> _sendQueue = new();
        List<ArraySegment<byte>> _pendingList = new();
        SocketAsyncEventArgs _sendArgs = new();
        SocketAsyncEventArgs _recvArgs = new();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract int OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(int numOfBytes);
        public abstract void OnDisconnected(EndPoint endPoint);

        void Clear()
        {
            lock (_lock)
            {
                _sendQueue.Clear();
                _pendingList.Clear();
            }
        }

        public void Start(Socket socket)
        {
            _socket = socket;

            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
        }

        #region -- Recv --

        void RegisterRecv()
        {
            if (_disconnect == 1)
                return;

            _recvBuffer.Clean();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);
                if (pending == false)
                {
                    OnRecvCompleted(null, _recvArgs);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorLog($"RegisterRecv Failed {e}");
            }
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.BytesTransferred == 0)
            {
                Logger.WarningLog("args.BytesTransferred == 0, Disconnect");
                Disconnect();
                return;
            }

            if (args.SocketError != SocketError.Success)
            {
                Logger.ErrorLog("args.SocketError != SocketError.Success, Disconnect");
                Disconnect();
                return;
            }

            try
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                {
                    Disconnect();
                    return;
                }

                int processLen = OnRecv(_recvBuffer.ReadSegment);
                if (processLen < 0 || _recvBuffer.DataSize < processLen)
                {
                    Disconnect();
                    return;
                }

                if (_recvBuffer.OnRead(processLen) == false)
                {
                    Disconnect();
                    return;
                }

                RegisterRecv();
            }
            catch (Exception e)
            {
                Logger.ErrorLog($"OnRecvCompleted Failed {e}");
            }
        }

        #endregion

        #region -- Send --

        public void Send(ArraySegment<byte> sendBuf)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuf);
                if (_pendingList.Count == 0)
                {
                    RegisterSend();
                }
            }
        }

        public void Send(List<ArraySegment<byte>> sendBufList)
        {
            if (sendBufList.Count == 0)
                return;

            lock (_lock)
            {
                foreach (ArraySegment<byte> sendBuf in sendBufList)
                    _sendQueue.Enqueue(sendBuf);

                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }

        void RegisterSend()
        {
            if (_disconnect == 1)
                return;

            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buf = _sendQueue.Dequeue();
                _pendingList.Add(buf);
            }
            _sendArgs.BufferList = _pendingList;

            try
            {
                bool pending = _socket.SendAsync(_sendArgs);
                if (pending == false)
                {
                    OnSendCompleted(null, _sendArgs);
                }
            }
            catch (Exception e)
            {
                Logger.ErrorLog($"RegisterSend Failed {e}");
            }
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;
                        _pendingList.Clear();

                        OnSend(_sendArgs.BytesTransferred);

                        if (_sendQueue.Count > 0)
                        {
                            RegisterSend();
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.ErrorLog($"OnRecvComplted Failed {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        #endregion

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnect, 1) == 1)
            {
                return;
            }

            OnDisconnected(_socket.RemoteEndPoint);

            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Clear();
        }

    }
}
