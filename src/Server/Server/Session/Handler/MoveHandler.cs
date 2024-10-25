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
    public class MoveHandler
    {
        public static void C_MoveStartHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_MoveStart moveStartPkt = packet as C_MoveStart;

            S_MoveStart resPkt = new S_MoveStart();
            resPkt.Success = true;
            resPkt.PlayerId = clientSession.Player.PlayerId;

            clientSession.Player.PlayerState = moveStartPkt.PlayerState;
            clientSession.Player.PlayerState.BaseState
                = BaseState.Moving;

            resPkt.PlayerState = clientSession.Player.PlayerState;
            RoomManager.Instance.GetRoom(clientSession.RoomId)?
                .GetMap(clientSession.MapId)?
                .Broadcast(resPkt);

        }
        public static void C_MovingHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Moving movingPkt = packet as C_Moving;

        }

        public static void C_MoveEndHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_MoveEnd moveEndPkt = packet as C_MoveEnd;

            S_MoveEnd resPkt = new S_MoveEnd();
            resPkt.Success = true;
            resPkt.PlayerId = clientSession.Player.PlayerId;

            clientSession.Player.PlayerState = moveEndPkt.PlayerState;
            clientSession.Player.PlayerState.BaseState
                = BaseState.Idle;

            resPkt.PlayerState = clientSession.Player.PlayerState;
            RoomManager.Instance.GetRoom(clientSession.RoomId)?
                .GetMap(clientSession.MapId)?
                .Broadcast(resPkt);
        }
    }
}
