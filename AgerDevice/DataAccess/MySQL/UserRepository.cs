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
                connection.Open(); 

                await connection.ExecuteAsync($@"INSERT INTO Users
                (
                    {nameof(User.Id)},
                    {nameof(User.Modified)},
                    {nameof(User.SerialNumber)},
                    {nameof(User.IsDeleted)},
                    {nameof(User.LastConnected)}
                ) 
                VALUES 
                (
                    @{nameof(User.Id)},
                    @{nameof(User.Modified)},
                    @{nameof(User.SerialNumber)},
                    @{nameof(User.IsDeleted)},
                    @{nameof(User.LastConnected)}
                )", user);
            }
        }

        public async Task DeleteAsync(Guid id)
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
                u.{nameof(User.LastConnected)},
                u.{nameof(User.SerialNumber)},
                u.{nameof(User.Modified)},
                u.{nameof(User.IsDeleted)}
                FROM Users u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.String);
                    sql += $@" AND {nameof(User.Id)} = @{nameof(query.Id)}";
                }

                if (query.SerialNumber != null)
                {
                    parameters.Add(nameof(query.SerialNumber), query.SerialNumber, DbType.String);
                    sql += $@" AND {nameof(User.SerialNumber)} = @{nameof(query.SerialNumber)}";
                }

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(User.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.IsDeleted != null)
                {
                    parameters.Add(nameof(query.IsDeleted), query.IsDeleted, DbType.DateTime);
                    sql += $@" AND {nameof(User.IsDeleted)} = @{nameof(query.IsDeleted)}";
                }

                if (query.LastConnected != null)
                {
                    parameters.Add(nameof(query.LastConnected), query.LastConnected, DbType.DateTime);
                    sql += $@" AND {nameof(User.LastConnected)} = @{nameof(query.LastConnected)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Users");
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
                await connection.ExecuteAsync($@"UPDATE Users SET
                    {nameof(User.SerialNumber)} = @{nameof(User.SerialNumber)},
                    {nameof(User.Modified)} = @{nameof(User.Modified)},
                    {nameof(User.IsDeleted)} = @{nameof(User.IsDeleted)},
                    {nameof(User.LastConnected)} = @{nameof(User.LastConnected)}
                    WHERE {nameof(User.Id)} = @{nameof(User.Id)}",
                record);
            }
        }
    }
}