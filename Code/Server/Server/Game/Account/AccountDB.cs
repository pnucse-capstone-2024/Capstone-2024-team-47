using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Account
{
    public class AccountDB
    {
        object _lock = new object();
        static AccountDB _instance = new AccountDB();
        public static AccountDB Instance { get { return _instance; } }

        // id, Account
        Dictionary<string, Account> _accounts = new Dictionary<string, Account>();

        AccountDB()
        {
            _accounts.Clear();
        }

        public int AddAccount(string id, string pw)
        {
            if (isExistAccount(id))
                return 1;

            int ret = 0;
            lock (_lock)
            {
                ret = _accounts.TryAdd(id, new Account(id, pw)) ? 0 : 1;
            }
            return ret;
        }
        
        public bool isExistAccount(string id)
        {
            return _accounts.ContainsKey(id);
        }

        public Account? GetAccount(string id) 
        {
            _accounts.TryGetValue(id, out Account? account);
            return account;
        }
    }
}
