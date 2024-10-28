using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class MapHandler
    {
        public static void C_EnterMapHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_EnterMap enterMapPkt = packet as C_EnterMap;

            int roomId = clientSession.RoomId;
            int mapId = enterMapPkt.MapId;

            S_EnterMap resPkt = new S_EnterMap();

            // RoomId 체크
            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                // Invalid RoomId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }
            
            // MapId 체크
            Map? map = room.GetMap(mapId);
            if (map == null)
            {
                // Invalid MapId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Map 입장 시도
            if (!map.Enter(clientSession))
            {
                // Map 입장 실패
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }
            
            // Map 입장 성공
            clientSession.MapId = mapId;
            resPkt.Success = true;
            resPkt.MyPlayer = clientSession.Player;
            foreach (Player player in map.GetPlayers())
                resPkt.Players.Add(player);
            clientSession.Send(resPkt);

            // Map Broadcast
            S_EnterMapBroadcast s_EnterMapBroadcast = new S_EnterMapBroadcast();
            s_EnterMapBroadcast.Player = clientSession.Player;
            map.Broadcast(s_EnterMapBroadcast);
        }

        public static void C_LeaveMapHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveMap leaveMapPkt = packet as C_LeaveMap;

            int roomId = clientSession.RoomId;
            int mapId = clientSession.MapId;

            S_LeaveMap resPkt = new S_LeaveMap();

            // RoomId 검증 
            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                // Invalid RoomId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // MapId 검증
            Map? map = room.GetMap(mapId);
            if (map == null)
            {
                // Invalid MapId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Map 퇴장 시도
            if (!map.Leave(clientSession))
            {
                // Map 퇴장 실패
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Map 퇴장 성공
            clientSession.MapId = -1;

            resPkt.Success = true;
            clientSession.Send(resPkt);
        }
    }
}
