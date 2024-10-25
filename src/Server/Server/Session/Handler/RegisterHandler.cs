using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class RegisterHandler
    {
        public static void C_RegisterHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Register registerPkt = packet as C_Register;

            string id = registerPkt.Id;
            string pw = registerPkt.Pw;

            S_Register resPkt = new S_Register();
            int ret = AccountDB.Instance.AddAccount(id, pw);
            
            if (ret == 0)
            {
                // 0: 성공
                resPkt.Success = true;
            }
            else
            {
                // 1: 이미 존재하는 아이디
                resPkt.Success = false;
                resPkt.ErrorCode = ret;
            }
            clientSession.Send(resPkt);
        }

    }
}
