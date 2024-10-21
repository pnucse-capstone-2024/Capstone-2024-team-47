using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.Packet.Handler
{
    public class MapHandler
    {
        public static void S_EnterMapHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_EnterMap enterMapPkt = packet as S_EnterMap;

            if (enterMapPkt.Success)
            {
                int mapId = enterMapPkt.MapId;
                string mapName = $"Map_0{mapId}";

                Player myPlayer = enterMapPkt.MyPlayer;
                List<Player> players = new List<Player>();
                foreach (Player p in enterMapPkt.Players)
                    players.Add(p);

                Manager.MapManager.MyPlayer = myPlayer;
                Manager.MapManager.PlayersInMap = players;
                Manager.MapManager.MapName = mapName;
                Manager.GameManager.MyPlayerId = myPlayer.PlayerId;

                SceneManager.LoadScene(mapName);
            }
        }

        public static void S_LeaveMapHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_LeaveMap leaveMapPkt = packet as S_LeaveMap;

            if (leaveMapPkt.Success)
            {
                int playerId = leaveMapPkt.PlayerId;

                GameObject go = GameObject.Find($"Player_{playerId}");
                GameObject.Destroy(go);
            }
        }

        public static void S_EnterMapBroadcastHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_EnterMapBroadcast broadcastPkt = packet as S_EnterMapBroadcast;

            Player p = broadcastPkt.Player;

            Vector3 spawnPosition = new Vector3(p.PlayerState.PosX, p.PlayerState.PosY, 0);
            GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Characters/Player");
            GameObject go = GameObject.Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            go.name = $"Player_{p.PlayerId}";

            PlayerController pc = go.GetComponent<PlayerController>();
            pc.PlayerState = p.PlayerState;
        }
    }
}