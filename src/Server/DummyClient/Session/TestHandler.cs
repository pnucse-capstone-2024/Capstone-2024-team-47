using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient.Session
{
    public class TestHandler
    {
        public static void InvalidHandler(PacketSession session, IMessage packet)
        {
            Console.WriteLine("InvalidHandler");
        }

        public static void S_TestHandler(PacketSession session, IMessage packet)
        {
            ServerSession serverSession = session as ServerSession;
            S_Test testPkt = packet as S_Test;

            string lorem = testPkt.Lorem;
            Logger.InfoLog($"[Server]: {lorem}");
        }
    }
}
