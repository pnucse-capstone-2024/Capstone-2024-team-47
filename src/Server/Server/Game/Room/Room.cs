using Google.Protobuf;
using NetworkCore.Job;
using Server.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore;

namespace Server.Game.Room
{
    public class Room : JobSerializer
    {
        object _lock = new object();

        public int RoomId { get; set; }
        public string Name { get; set; }

        // <SessionId, ClientSession>
        Dictionary<int, ClientSession> _sessions = new();

        // <MapId, Map>
        Dictionary<int, Map> _maps = new();

        public Room(int roomId)
        {
            RoomId = roomId;
            Name = $"Room {roomId}";

            _maps.Add(1, new Map(1));
            _maps.Add(2, new Map(2));
            _maps.Add(3, new Map(3));
            _maps.Add(4, new Map(4));
            _maps.Add(5, new Map(5));
        }

        public void Update()
        {
            foreach (Map map in _maps.Values)
                map.Update();

            Flush();
        }

        public bool Enter(ClientSession session)
        {
            bool ret = true;

            lock (_lock)
            {
                ret &= _sessions.TryAdd(session.SessionId, session);
            }

            return ret;
        }

        public bool Leave(ClientSession session)
        {
            bool ret = true;

            lock ( _lock)
            {
                ret &= _sessions.Remove(session.SessionId);
            }

            return ret;
        }

        public Map? GetMap(int mapId)
        {
            _maps.TryGetValue(mapId, out Map? map);
            return map;
        }

        public void Broadcast(IMessage packet)
        {
            foreach (ClientSession session in _sessions.Values)
                session.Send(packet);
        }

        public List<ClientSession> GetSessions()
        {
            List<ClientSession> clientSessions = new List<ClientSession>(_sessions.Values);
            return clientSessions;
        }
    }
}
