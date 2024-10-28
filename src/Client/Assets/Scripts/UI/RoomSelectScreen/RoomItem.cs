using Google.Protobuf.Protocol;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _roomNumberText;
    [SerializeField] TextMeshProUGUI _roomTitleText;
    [SerializeField] Button _enterButton;

    public void SetRoomItem(int roomId, string roomTitle)
    {
        _roomNumberText.text = roomId.ToString();
        _roomTitleText.text = roomTitle;
        _enterButton.onClick.AddListener(() =>
        {
            C_EnterRoom enterRoomPkt = new C_EnterRoom();
            enterRoomPkt.RoomId = roomId;   
            Manager.NetworkManager.Send(enterRoomPkt);
        });
    }

}