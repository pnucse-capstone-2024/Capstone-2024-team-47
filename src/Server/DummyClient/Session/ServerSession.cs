using Google.Protobuf;
using Google.Protobuf.Security;
using NetworkCore.Encryption.PublicKey;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;
using Google.Protobuf.Protocol;

namespace DummyClient.Session
{
    public class ServerSession : PacketSession
    {
        static int SessionIdGenerator = 0;
        int _sessionId;
        public int SessionId { get { return _sessionId; } }

        private Random _random = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private System.Timers.Timer _timer = new System.Timers.Timer();

        public ServerSession()
        {
            _sessionId = Interlocked.Increment(ref SessionIdGenerator);

            // Test1();
            Test2();
        }

        ServerPacketHandler serverPacketHandler = new();

        public override void OnConnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Server]: Successfully Connected. Client_{SessionId}");

            StartSessionKeyExchange();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Server]: Successfully Disconnected. Client_{_sessionId}");
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            serverPacketHandler.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {

        }

        public new void Send(IMessage packet)
        {
            if (!IsSecure)
            {
                Logger.ErrorLog($"Client_{SessionId} Can't send normal packet on not secure section");
                return;
            }
            base.Send(packet);
        }

        public string GenerateRandomString(int length)
        {
            return new string(Enumerable.Repeat(_chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public void SendTestPacket()
        {
            C_Test testPkt = new C_Test();
            testPkt.Lorem = GenerateRandomString(50);
            Logger.InfoLog($"Client_{SessionId}: {testPkt.Lorem}");
            Send(testPkt);
        }

        private int GetRandomInterval()
        {
            return _random.Next(250, 1000);
        }
        
        private void Test1()
        {
            _timer.Interval = GetRandomInterval();
            _timer.Elapsed += ((s, e) => 
            {
                _timer.Interval = GetRandomInterval();
                SendTestPacket(); 
            });
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void Test2()
        {
            _timer.Interval = 2000;
            _timer.Elapsed += ((s, e) =>
            {
                for (int i = 0; i < 100; i++)
                    SendTestPacket();
            });
            _timer.Enabled = true;
        }
    }
}
