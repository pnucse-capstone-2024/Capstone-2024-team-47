using Google.Protobuf.Protocol;
using NetworkCore.Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient.Session
{
    public class ServerPacketHandler : PacketHandler
    {
        public ServerPacketHandler()
        {
            AddHandler();
            AddOnRecv();
        }

        protected override void AddHandler()
        {
            // Register
            _onRecv.Add((ushort)MsgId.SRegister, MakePacket<S_Register>);

            // Login
            _onRecv.Add((ushort)MsgId.SLogin, MakePacket<S_Login>);

            // Room
            _onRecv.Add((ushort)MsgId.SEnterroom, MakePacket<S_EnterRoom>);
            _onRecv.Add((ushort)MsgId.SLeaveroom, MakePacket<S_LeaveRoom>);

            // Map
            _onRecv.Add((ushort)MsgId.SEntermap, MakePacket<S_EnterMap>);
            _onRecv.Add((ushort)MsgId.SLeavemap, MakePacket<S_LeaveMap>);
            _onRecv.Add((ushort)MsgId.SEntermapbroadcast, MakePacket<S_EnterMapBroadcast>);

            // Chat
            _onRecv.Add((ushort)MsgId.SChat, MakePacket<S_Chat>);

            // Move
            _onRecv.Add((ushort)MsgId.SMovestart, MakePacket<S_MoveStart>);
            _onRecv.Add((ushort)MsgId.SMoving, MakePacket<S_Moving>);
            _onRecv.Add((ushort)MsgId.SMoveend, MakePacket<S_MoveEnd>);

            // Jump
            _onRecv.Add((ushort)MsgId.SJump, MakePacket<S_Jump>);
            _onRecv.Add((ushort)MsgId.SFall, MakePacket<S_Fall>);
            _onRecv.Add((ushort)MsgId.SLand, MakePacket<S_Land>);

            // Skill
            _onRecv.Add((ushort)MsgId.SSkill, MakePacket<S_Skill>);

            // Hit
            _onRecv.Add((ushort)MsgId.SHit, MakePacket<S_Hit>);

            // Test
            _onRecv.Add((ushort)MsgId.STest, MakePacket<S_Test>);
        }

        protected override void AddOnRecv()
        {
            // Register
            _handler.Add((ushort)MsgId.SRegister, TestHandler.InvalidHandler);

            // Login
            _handler.Add((ushort)MsgId.SLogin, TestHandler.InvalidHandler);

            // Room
            _handler.Add((ushort)MsgId.SEnterroom, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SLeaveroom, TestHandler.InvalidHandler);

            // Map
            _handler.Add((ushort)MsgId.SEntermap, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SLeavemap, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SEntermapbroadcast, TestHandler.InvalidHandler);

            // Chat
            _handler.Add((ushort)MsgId.SChat, TestHandler.InvalidHandler);

            // Move
            _handler.Add((ushort)MsgId.SMovestart, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SMoving, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SMoveend, TestHandler.InvalidHandler);

            // Jump
            _handler.Add((ushort)MsgId.SJump, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SFall, TestHandler.InvalidHandler);
            _handler.Add((ushort)MsgId.SLand, TestHandler.InvalidHandler);

            // Skill
            _handler.Add((ushort)MsgId.SSkill, TestHandler.InvalidHandler);

            // Hit
            _handler.Add((ushort)MsgId.SHit, TestHandler.InvalidHandler);

            // Test
            _handler.Add((ushort)MsgId.STest, TestHandler.S_TestHandler);
        }
    }
}
