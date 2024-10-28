using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using NetworkCore.Packet;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class RoomHandler
    {
        public static void C_EnterRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_EnterRoom enterRoomPkt = packet as C_EnterRoom;

            int roomId = enterRoomPkt.RoomId;

            S_EnterRoom resPkt = new S_EnterRoom();

            // RoomId 체크
            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                // Invalid RoomId
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 입장 시도
            if (!room.Enter(clientSession))
            {
                // Room 입장 실패
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 입장 성공
            clientSession.RoomId = roomId;

            resPkt.Success = true;
            clientSession.Send(resPkt);

            // Map 처리
            S_EnterMap enterMapPkt = new S_EnterMap();
            
            // MapId 체크
            int mapId = clientSession.Player.PlayerInfo.MapId;
            Map? map = room.GetMap(mapId);
            if (map == null)
            {
                enterMapPkt.Success = false;
                clientSession.Send(enterMapPkt);
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
            enterMapPkt.Success = true;
            clientSession.MapId = mapId;
            enterMapPkt.MapId = mapId;
            enterMapPkt.MyPlayer = clientSession.Player;
            foreach (Player player in map.GetPlayers())
                enterMapPkt.Players.Add(player);
            clientSession.Send(enterMapPkt);

            // Map에 있던 기존 플레이어들에게 해당 플레이어가 들어왔음을 공지
            S_EnterMapBroadcast s_enterMapBroadcastPkt = new S_EnterMapBroadcast();
            s_enterMapBroadcastPkt.Player = clientSession.Player;
            foreach (ClientSession otherSession in map.GetSessions())
            {
                if (otherSession.Player.PlayerId == clientSession.Player.PlayerId)
                    continue;
                otherSession.Send(s_enterMapBroadcastPkt);
            }
        }

        public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_LeaveRoom leaveRoomPkt = packet as C_LeaveRoom;

            // Map을 먼저 나가고, Room을 나가야 함.

            int roomId = clientSession.RoomId;
            int mapId = clientSession.MapId;

            S_LeaveRoom resPkt = new S_LeaveRoom();

            Room? room = RoomManager.Instance.GetRoom(roomId);
            if (room == null)
            {
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            Map? map = room.GetMap(mapId);
            if (map == null)
            {
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            if (!map.Leave(clientSession))
            {
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Map 퇴장 성공
            S_LeaveMap leaveMapPkt = new S_LeaveMap();
            leaveMapPkt.Success = true;
            leaveMapPkt.PlayerId = clientSession.Player.PlayerId;
            map.Broadcast(leaveMapPkt);

            if (!room.Leave(clientSession))
            {
                resPkt.Success = false;
                clientSession.Send(resPkt);
                return;
            }

            // Room 퇴장 성공
            clientSession.RoomId = 0;
            resPkt.Success = true;
            clientSession.Send(resPkt);
        }
    }
}
