using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Account
{
    public class Account
    {
        static int _playerIdGenerator = 1;

        string _id;
        string _pw;
        Player _player;

        public string Id { get { return _id; } }    
        public Player Player { get { return _player; } }

        public Account(string id, string pw)
        {
            _id = id;
            _pw = pw;

            // 최초 플레이어 생성 시 설정
            _player = new Player();
            _player.PlayerId = Interlocked.Increment(ref _playerIdGenerator);
            
            _player.PlayerInfo = new PlayerInfo();
            _player.PlayerInfo.PlayerName = id;
            _player.PlayerInfo.Hp = 100;
            _player.PlayerInfo.Exp = 0;
            _player.PlayerInfo.Attack = 15;
            _player.PlayerInfo.MapId = 1;
            _player.PlayerInfo.Level = 1;
            
            _player.PlayerState = new PlayerState();
            _player.PlayerState.PosX = 0;
            _player.PlayerState.PosY = 0;
            _player.PlayerState.MoveDir = MoveDir.Right;
            _player.PlayerState.BaseState = BaseState.Idle;
        }

        public bool PasswordVerify(string pw)
        {
            return _pw.Equals(pw);
        }

    }
}
