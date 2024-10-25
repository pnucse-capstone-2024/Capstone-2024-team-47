using Google.Protobuf;
using Google.Protobuf.Security;
using NetworkCore.Encryption.PublicKey;
using NetworkCore;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Session
{
    public class ServerSession : PacketSession
    {
        public ServerPacketHandler serverPacketHandler = new();

        public override void OnConnected(EndPoint endPoint)
        {
            Logger.InfoLog("[Server]: Successfully Connected.");

            StartSessionKeyExchange();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Logger.InfoLog("[Server]: Successfully Disconnected.");
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
                Logger.ErrorLog("Can't send normal packet on not secure section");
                return;
            }
            base.Send(packet);
        }
    }
}
