using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using AgerDevice.Core.Query;

namespace AgerDevice.Redis
{
    public class UsersHandler
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly string _tableName;

        public UsersHandler(ConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
            _tableName = "Users";
        }

        public List<string> QueryUsers(UsersQuery query)
        {
        }
    }
}
