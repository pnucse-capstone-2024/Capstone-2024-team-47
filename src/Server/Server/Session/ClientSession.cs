using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;
using Server.Game.Room;
using Server.Game.Account;
using Google.Protobuf.Protocol;

namespace Server.Session
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }
        public Account Account { get; set; }
        public Player Player { get; set; }
        public int RoomId { get; set; }
        public int MapId { get; set; }

        ClientPacketHandler clientPacketHandler = new();

        public override void OnConnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Client {SessionId}]: Connected.");
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Logger.InfoLog($"[Client {SessionId}]: Disconnected.");

            Room? room = RoomManager.Instance.GetRoom(RoomId);
            if (room == null)
                return;

            Map? map = room.GetMap(MapId);
            if (map == null) 
                return;

            map.Leave(this);

            S_LeaveMap leaveMapPkt = new S_LeaveMap();
            leaveMapPkt.Success = true;
            leaveMapPkt.PlayerId = Player.PlayerId;
            map.Broadcast(leaveMapPkt);

            room.Leave(this);
        }

        public override void OnRecvPacket(ArraySegment<byte> buffer)
        {
            clientPacketHandler.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numOfBytes)
        {

        }
    }
}
