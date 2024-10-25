using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Session;
using UnityEngine.SceneManagement;

namespace Network.Packet.Handler
{
    public class RegisterHandler
    {
        public static void S_RegisterHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Register registerPkt = packet as S_Register; 

            if (registerPkt.Success)
            {
                PopupQueue.Instance.Push("ȸ������ ����!", () =>
                {
                    SceneManager.LoadScene("LoginScene");
                });
            }
            else
            {
                switch (registerPkt.ErrorCode)
                {
                    case 1:
                        PopupQueue.Instance.Push("�̹� ���Ե� ���̵��Դϴ�.");
                        break;
                }
            }
        }
    }
}