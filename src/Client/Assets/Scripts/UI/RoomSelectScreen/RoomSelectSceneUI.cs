using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectSceneUI : MonoBehaviour
{
    [SerializeField] GameObject _roomPanel;
    [SerializeField] GameObject _playerInfoPanel;
    [SerializeField] GameObject _roomInfoPrefab;

    public void UpdateRoomInfo(List<RoomInfo> roomInfos)
    {
        Vector2 currentPosition = new Vector2(0, 175);

        foreach (RoomInfo roomInfo in roomInfos)
        {
            int roomId = roomInfo.RoomId;
            string roomTitle = roomInfo.RoomTitle;

            GameObject roomInfoObject = Instantiate(_roomInfoPrefab, _roomPanel.transform);
            roomInfoObject.transform.localPosition = currentPosition;
            
            RoomItem roomItem = roomInfoObject.GetComponent<RoomItem>();
            roomItem.SetRoomItem(roomId, roomTitle);

            currentPosition.y -= 50;
        }
    }

    public void UpdatePlayerInfo(PlayerInfo playerInfo)
    {
        string name = playerInfo.PlayerName;
        int mapId = playerInfo.MapId;   
        int level = playerInfo.Level;

        PlayerInfoItem playerInfoItem = _playerInfoPanel.GetComponent<PlayerInfoItem>();
        playerInfoItem.SetName(name);
        playerInfoItem.SetLevel(level);
        playerInfoItem.SetMapId(mapId);
    }
}
