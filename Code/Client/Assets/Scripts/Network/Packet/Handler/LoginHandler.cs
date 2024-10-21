using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network.Packet.Handler
{
    public class LoginHandler
    {
        public static void S_LoginHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Login loginPkt = packet as S_Login;

            if (loginPkt.Success)
            {
                Player myPlayer = loginPkt.Player;
                List<RoomInfo> roomInfos = new List<RoomInfo>();
                foreach (RoomInfo roomInfo in loginPkt.RoomInfo)
                    roomInfos.Add(roomInfo);

                void OnSceneLoaded(Scene scene, LoadSceneMode mode)
                {
                    if (scene.name == "RoomSelectScene")
                    {
                        GameObject go = GameObject.Find("RoomSelectUI");
                        RoomSelectSceneUI roomSelectSceneUI = go.GetComponent<RoomSelectSceneUI>();
                        roomSelectSceneUI.UpdateRoomInfo(roomInfos);
                        roomSelectSceneUI.UpdatePlayerInfo(myPlayer.PlayerInfo);

                        SceneManager.sceneLoaded -= OnSceneLoaded;
                    }
                }
                SceneManager.sceneLoaded += OnSceneLoaded;

                SceneManager.LoadScene("RoomSelectScene");
            }
            else
            {
                switch (loginPkt.ErrorCode)
                {
                    case 1:
                        PopupQueue.Instance.Push("없는 아이디입니다.");
                        break;
                    case 2:
                        PopupQueue.Instance.Push("잘못된 비밀번호입니다.");
                        break;
                }
            }
        }
    }
}