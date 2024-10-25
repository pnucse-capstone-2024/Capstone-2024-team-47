using Google.Protobuf;
using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using Server.Game.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Session.Handler
{
    public class ChatHandler
    {
        public static void C_ChatHandler(PacketSession session, IMessage packet)
        {
            ClientSession clientSession = session as ClientSession;
            C_Chat chatPkt = packet as C_Chat;

            int playerId        = clientSession.Player.PlayerId;
            ChatType chatType   = chatPkt.ChatType;
            string chat         = chatPkt.Chat;

            S_Chat resPkt = new S_Chat();
            resPkt.Success = true;
            resPkt.ChatType = chatType;
            resPkt.PlayerId = playerId;
            resPkt.Chat = chat;

            switch (chatType)
            {
                case ChatType.All:
                    RoomManager.Instance.Broadcast(resPkt);
                    break;
                case ChatType.Room:
                    {
                        Room? room = RoomManager.Instance.GetRoom(clientSession.RoomId);
                        if (room != null)
                            room.Push(room.Broadcast, resPkt);
                        break;
                    }
                case ChatType.Map:
                    {
                        Room? room = RoomManager.Instance.GetRoom(clientSession.RoomId);
                        if (room != null)
                        {
                            Map? map = room.GetMap(clientSession.MapId);
                            if (map != null) 
                                map.Push(map.Broadcast, resPkt);
                        }    
                        break;
                    }
            }
        }
    }
}
