using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using UnityEngine;

namespace Network.Packet.Handler
{
    public class SkillHandler
    {
        public static void S_SkillHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Skill skillPkt = packet as S_Skill;

            if (skillPkt.Success)
            {
                if (skillPkt.PlayerId == Manager.GameManager.MyPlayerId)
                {
                    GameObject go = GameObject.Find($"MyPlayer_{skillPkt.PlayerId}");
                    MyPlayerController mpc = go.GetComponent<MyPlayerController>();
                    mpc.PlayerState = skillPkt.PlayerState;
                    mpc.UseSkill();
                }
                else
                {
                    GameObject go = GameObject.Find($"Player_{skillPkt.PlayerId}");
                    PlayerController pc = go.GetComponent<PlayerController>();
                    pc.PlayerState = skillPkt.PlayerState;
                    pc.UseSkill();
                }
            }
        }
    }
}