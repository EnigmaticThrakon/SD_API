using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace AgerDevice.Redis
{
    public class SettingsHandler
    {
        private readonly ConnectionMultiplexer _redis;

        public SettingsHandler(ConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
        }
    }
}
