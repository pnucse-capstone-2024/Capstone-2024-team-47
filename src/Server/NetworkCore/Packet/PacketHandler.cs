using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Security;
using NetworkCore.Encryption.PublicKey;
using NetworkCore.Packet.Security;

namespace NetworkCore.Packet
{
    public abstract class PacketHandler
    {
        protected Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new();
        protected Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new();

        public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

        public PacketHandler()
        {
            Init();
        }

        public virtual void Init()
        {
            _onRecv.Add((ushort)MsgId.CHello, MakePacket<C_Hello>);
            _handler.Add((ushort)MsgId.CHello, SessionKeyExchangeHandler.C_HelloHandler);
            _onRecv.Add((ushort)MsgId.SHello, MakePacket<S_Hello>);
            _handler.Add((ushort)MsgId.SHello, SessionKeyExchangeHandler.S_HelloHandler);
            _onRecv.Add((ushort)MsgId.CHelloDone, MakePacket<C_Hello_Done>);
            _handler.Add((ushort)MsgId.CHelloDone, SessionKeyExchangeHandler.C_HelloDoneHandler);
            _onRecv.Add((ushort)MsgId.SHelloDone, MakePacket<S_Hello_Done>);
            _handler.Add((ushort)MsgId.SHelloDone, SessionKeyExchangeHandler.S_HelloDoneHandler);
        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            ushort offset = 2;

            // size를 어차피 안씀
            // ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            // offset += 2;

            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + offset);
            offset += 2;

            Action<PacketSession, ArraySegment<byte>, ushort> action = null;
            if (_onRecv.TryGetValue(id, out action))
                action.Invoke(session, buffer, id);
        }

        protected void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
        {
            T pkt = new T();
            pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

            if (CustomHandler != null)
                CustomHandler.Invoke(session, pkt, id);
            else
            {
                Action<PacketSession, IMessage> action = null;
                if (_handler.TryGetValue(id, out action))
                    action.Invoke(session, pkt);
            }
        }

        protected abstract void AddOnRecv();
        protected abstract void AddHandler();
    }
}
