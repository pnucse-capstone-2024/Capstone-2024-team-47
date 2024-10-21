using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Account;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class LoginHandler
    {
        public static void C_LoginHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Login loginPkt = packet as C_Login;

            string id = loginPkt.Id;
            string pw = loginPkt.Pw;

            S_Login resPkt = new S_Login();
            Account? account = AccountDB.Instance.GetAccount(id);
            if (account == null)
            {
                // 1: 없는 아이디
                resPkt.Success = false;
                resPkt.ErrorCode = 1;
                clientSession.Send(resPkt);
                return;
            }

            if (!account.PasswordVerify(pw))
            {
                // 2: 잘못된 비밀번호
                resPkt.Success = false;
                resPkt.ErrorCode = 2;
                clientSession.Send(resPkt);
                return; 
            }

            // 0: 정상 로그인
            resPkt.Success = true;
            resPkt.Player = account.Player;
            foreach (RoomInfo roomInfo in RoomManager.Instance.GetRoomsInfo())
                resPkt.RoomInfo.Add(roomInfo);
            clientSession.Send(resPkt);

            clientSession.Account = account;
            clientSession.Player = account.Player;
        }
    }
}
