using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class ChatHandler
    {
        public static void S_ChatHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Chat chatPkt = packet as S_Chat;
        }
    }
}