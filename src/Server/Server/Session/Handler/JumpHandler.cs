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
    public class JumpHandler
    {
        public static void C_JumpHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Jump jumpPkt = packet as C_Jump;

            S_Jump resPkt = new S_Jump();
            Player player = clientSession.Player;
            if (player.PlayerState.BaseState == BaseState.Idle || player.PlayerState.BaseState == BaseState.Moving)
            {
                clientSession.Player.PlayerState = jumpPkt.PlayerState;
                clientSession.Player.PlayerState.BaseState = BaseState.Jump;

                resPkt.Success = true;
                resPkt.PlayerId = player.PlayerId;
                resPkt.PlayerState = clientSession.Player.PlayerState;
            }
            else
            {
                resPkt.Success = false;
            }
            RoomManager.Instance.GetRoom(clientSession.RoomId)?
                .GetMap(clientSession.MapId)?
                .Broadcast(resPkt);
        }

        public static void C_FallHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Fall fallPkt = packet as C_Fall;


        }

        public static void C_LandHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Land landPkt = packet as C_Land;


        }

        public static void C_JumpEndHandler(PacketSession session, IMessage packet) 
        {
            ClientSession clientSession = session as ClientSession;
            C_JumpEnd jumpEndPkt = packet as C_JumpEnd;

            clientSession.Player.PlayerState = jumpEndPkt.PlayerState;
            clientSession.Player.PlayerState.BaseState = BaseState.Idle;

            S_JumpEnd resPkt = new S_JumpEnd();
            resPkt.PlayerId = clientSession.Player.PlayerId;
            resPkt.PlayerState = clientSession.Player.PlayerState;
            RoomManager.Instance.GetRoom(clientSession.RoomId)?
                .GetMap(clientSession.MapId)?
                .Broadcast(resPkt);
        }
    }
}
