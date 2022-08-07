using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using AgerDevice.Core.Models;
using AgerDevice.Core.Query;
using AgerDevice.Core.Repositories;

namespace AgerDevice.DataAccess.MySQL
{
    public class UserRepository : IUserRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public UserRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(User user)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"INSERT INTO Users
                (
                    {nameof(User.Id)},
                ) 
                VALUES 
                (
                    @{nameof(User.Id)},
                )", user);
            }
        }

        public async Task DeleteAsync(string id)
        {
            return;
        }

        public async Task<PagedResult<User>> QueryAsync(UserQuery query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new UserQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(UserQuery.SortColumns.Modified);
                }

                string sql = $@"SELECT
                u.{nameof(User.Id)},
                FROM Units u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.String);
                    sql += $@" AND {nameof(User.Id)} = @{nameof(query.Id)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Units");
                Task<int> filteredRecords = connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) AS Results", parameters);

                sql += $" ORDER BY {query.GetOrderByString()}";

                if (query.Take > 0)
                {
                    query.Skip = query.Skip ?? 0;

                    parameters.Add(nameof(query.Skip), query.Skip, DbType.Int32);
                    parameters.Add(nameof(query.Take), query.Take, DbType.Int32);

                    sql += $" LIMIT @{nameof(query.Skip)}, @{nameof(query.Take)}";
                }

                Task<IEnumerable<User>> records = connection.QueryAsync<User>(sql, parameters);

                return new PagedResult<User>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task UpdateAsync(User record)
        {
            using(IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"UPDATE Units SET
                    {nameof(User.DeviceId)} = @{nameof(User.DeviceId)},
                    WHERE {nameof(User.Id)} = @{nameof(User.Id)}",
                record);
            }
        }
    }
}