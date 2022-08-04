using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace AgerDevice.Redis
{
    public class UnitsHandler
    {
        private readonly ConnectionMultiplexer _redis;

        public UnitsHandler(ConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
        }
    }
}
