using Google.Protobuf.Protocol;
using Google.Protobuf;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class TestHandler
    {
        public static void S_TestHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Test testPkt = packet as S_Test;
        }
    }
}