using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class TestHandler
    {
        public static void C_TestHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Test testPkt = packet as C_Test;

            string lorem = testPkt.Lorem;
            
            S_Test resPkt = new S_Test();
            resPkt.Lorem = lorem;
            clientSession.Send(resPkt);
        }
    }
}
