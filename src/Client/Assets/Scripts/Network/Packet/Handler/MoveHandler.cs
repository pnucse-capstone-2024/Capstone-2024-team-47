using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using UnityEngine;

namespace Network.Packet.Handler
{
    public class MoveHandler
    {
        public static void S_MoveStartHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_MoveStart moveStartPkt = packet as S_MoveStart;

            if (moveStartPkt.PlayerId != Manager.GameManager.MyPlayerId)
            {
                GameObject go = GameObject.Find($"Player_{moveStartPkt.PlayerId}");
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.PlayerState = moveStartPkt.PlayerState;
            }
        }

        public static void S_MovingHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Moving movingPkt = packet as S_Moving;

            if (movingPkt.PlayerId != Manager.GameManager.MyPlayerId)
            {
                GameObject go = GameObject.Find($"Player_{movingPkt.PlayerId}");
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.PlayerState = movingPkt.PlayerState;
            }
        }

        public static void S_MoveEndHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_MoveEnd moveEndPkt = packet as S_MoveEnd;

            if (moveEndPkt.PlayerId != Manager.GameManager.MyPlayerId)
            {
                GameObject go = GameObject.Find($"Player_{moveEndPkt.PlayerId}");
                PlayerController pc = go.GetComponent<PlayerController>();
                pc.PlayerState = moveEndPkt.PlayerState;
            }
        }
    }
}