using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using UnityEngine;

namespace Network.Packet.Handler
{
    public class JumpHandler
    {
        public static void S_JumpHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Jump jumpPkt = packet as S_Jump;

            if (jumpPkt.Success)
            {
                if (jumpPkt.PlayerId == Manager.GameManager.MyPlayerId)
                {
                    GameObject go = GameObject.Find($"MyPlayer_{jumpPkt.PlayerId}");
                    MyPlayerController mpc = go.GetComponent<MyPlayerController>();
                    mpc.PlayerState = jumpPkt.PlayerState;
                }
                else
                {
                    GameObject go = GameObject.Find($"Player_{jumpPkt.PlayerId}");
                    PlayerController pc = go.GetComponent<PlayerController>();
                    pc.PlayerState = jumpPkt.PlayerState;
                }
            }
        }

        public static void S_FallHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Fall fallPkt = packet as S_Fall;
        }

        public static void S_LandHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Land landPkt = packet as S_Land;
        }

        public static void S_JumpEndHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_JumpEnd jumpEndPkt = packet as S_JumpEnd;

            if (jumpEndPkt.PlayerId == Manager.GameManager.MyPlayerId)
            {
                GameObject go = GameObject.Find($"MyPlayer_{jumpEndPkt.PlayerId}");
                MyPlayerController mpc = go.GetComponent<MyPlayerController>();
                mpc.PlayerState = jumpEndPkt.PlayerState;
            }
            else
            {
                GameObject go = GameObject.Find($"Player_{jumpEndPkt.PlayerId}");
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.PlayerState = jumpEndPkt.PlayerState;
            }
        }
    }
}
