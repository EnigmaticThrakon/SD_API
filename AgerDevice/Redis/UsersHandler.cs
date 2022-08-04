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
        private readonly UserDataKeys _userDataKeys;

        public UsersHandler(ConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer;
            _userDataKeys = new UserDataKeys();
        }

        public async Task<Guid> ApplyUsername(string username)
        {
            IDatabase database = _redis.GetDatabase();
            RedisValue[] results = await database.ListRangeAsync(_userDataKeys.TableName + ':' + _userDataKeys.UsernameList);

            if(results.Any(t => t.ToString() == username)){
                return Guid.Empty;
            }
                
            Guid userId = Guid.NewGuid();
            await database.ListLeftPushAsync(_userDataKeys.TableName + ':' + _userDataKeys.UsernameList, username);
            await database.ListLeftPushAsync(_userDataKeys.TableName + ':' + _userDataKeys.UserIdList, userId.ToString());

            await database.StringSetAsync(_userDataKeys.TableName + ':' + username, userId.ToString());

            string primaryUserKey = _userDataKeys.TableName + ':' + userId.ToString() + ':';
            await database.StringSetAsync(primaryUserKey + _userDataKeys.Username, username);
            await database.StringSetAsync(primaryUserKey + _userDataKeys.IsDeleted, "false");
            await database.StringSetAsync(primaryUserKey + _userDataKeys.LastModified, DateTime.Now.ToString());
            return userId;
        }
    }
}
