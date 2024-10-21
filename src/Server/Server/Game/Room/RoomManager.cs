using Google.Protobuf;
using Server.Session;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;
using Google.Protobuf.Protocol;

namespace Server.Game.Room
{
    public class RoomManager
    {
        // <RoomId, Room>
        Dictionary<int, Room> _rooms = new();

        static RoomManager _instance = new RoomManager();
        public static RoomManager Instance { get => _instance; }

        RoomManager()
        {
            _rooms.Add(1, new Room(1));
            _rooms.Add(2, new Room(2));
            _rooms.Add(3, new Room(3));
        }

        public void Update()
        {
            foreach (Room room in _rooms.Values)
                room.Update();
        }
        
        public void Broadcast(IMessage packet)
        {
            foreach (Room room in _rooms.Values)
                room.Push(room.Broadcast, packet);
        }

        public RoomInfo? GetRoomInfo(int roomId)
        {
            Room? room = GetRoom(roomId);
            if (room == null) 
                return null;

            RoomInfo roomInfo = new RoomInfo();
            roomInfo.RoomId = room.RoomId;
            roomInfo.RoomTitle = room.Name;
            return roomInfo;
        }

        public List<RoomInfo> GetRoomsInfo()
        {
            List<RoomInfo> roomInfos = new List<RoomInfo>();
            foreach (Room room in  _rooms.Values)
            {
                RoomInfo roomInfo = new RoomInfo();
                roomInfo.RoomId = room.RoomId;
                roomInfo.RoomTitle = room.Name;
                roomInfos.Add(roomInfo);
            }
            return roomInfos;
        }

        public Room? GetRoom(int roomId)
        {
            _rooms.TryGetValue(roomId, out Room? room);
            return room;
        }
    }
}
