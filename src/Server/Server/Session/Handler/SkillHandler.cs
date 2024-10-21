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
    public class SkillHandler
    {
        public static void C_SkillHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Skill skillPkt = packet as C_Skill;

            S_Skill resPkt = new S_Skill();
            Player player = clientSession.Player;
            if (player.PlayerState.BaseState == BaseState.Idle || player.PlayerState.BaseState == BaseState.Moving)
            {
                clientSession.Player.PlayerState = skillPkt.PlayerState;
                clientSession.Player.PlayerState.BaseState = BaseState.Skill;

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
    }
}
