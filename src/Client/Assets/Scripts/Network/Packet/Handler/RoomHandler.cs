using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;

namespace Network.Packet.Handler
{
    public class RoomHandler
    {
        public static void S_EnterRoomHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_EnterRoom enterRoomPkt = packet as S_EnterRoom;
        }

        public static void S_LeaveRoomHandler(PacketSession session, IMessage packet) 
        {
            ServerSession serverSession = session as ServerSession;
            S_LeaveRoom leaveRoomPkt = packet as S_LeaveRoom;
        }
    }
}