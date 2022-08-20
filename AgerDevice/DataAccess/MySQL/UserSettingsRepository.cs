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
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public UserSettingsRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(UserSettings userSettings)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                connection.Open(); 

                await connection.ExecuteAsync($@"INSERT INTO UserSettings
                (
                    {nameof(UserSettings.Id)},
                    {nameof(UserSettings.Modified)},
                    {nameof(UserSettings.UserName)},
                    {nameof(UserSettings.GroupId)},
                    {nameof(UserSettings.GroupsEnabled)}
                ) 
                VALUES 
                (
                    @{nameof(UserSettings.Id)},
                    @{nameof(UserSettings.Modified)},
                    @{nameof(UserSettings.UserName)},
                    @{nameof(UserSettings.GroupId)},
                    @{nameof(UserSettings.GroupsEnabled)}
                )", userSettings);
            }
        }

        public async Task Delete(Guid id)
        {
            using(IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"DELETE FROM UserSettings WHERE Id = '" + id.ToString() + "';");
            }
        }

        public async Task<PagedResult<UserSettings>> QueryAsync(UserSettingsQuery? query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new UserSettingsQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(UserSettingsQuery.SortColumns.Modified);
                }

                string sql = $@"SELECT
                u.{nameof(UserSettings.Id)},
                u.{nameof(UserSettings.Modified)},
                u.{nameof(UserSettings.UserName)},
                u.{nameof(UserSettings.GroupsEnabled)},
                u.{nameof(UserSettings.GroupId)}
                FROM UserSettings u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.Guid);
                    sql += $@" AND {nameof(UserSettings.Id)} = @{nameof(query.Id)}";
                }

                if (query.GroupsEnabled != null)
                {
                    parameters.Add(nameof(query.GroupsEnabled), query.GroupsEnabled, DbType.Boolean);
                    sql += $@" AND {nameof(UserSettings.GroupsEnabled)} = @{nameof(query.GroupsEnabled)}";
                }

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(UserSettings.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.UserName != null)
                {
                    parameters.Add(nameof(query.UserName), query.UserName, DbType.String);
                    sql += $@" AND {nameof(UserSettings.UserName)} = @{nameof(query.UserName)}";
                }

                if (query.GroupId != null)
                {
                    parameters.Add(nameof(query.GroupId), query.GroupId, DbType.Guid);
                    sql += $@" AND {nameof(UserSettings.GroupId)} = @{nameof(query.GroupId)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM UserSettings");
                Task<int> filteredRecords = connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) AS Results", parameters);

                sql += $" ORDER BY {query.GetOrderByString()}";

                if (query.Take > 0)
                {
                    query.Skip = query.Skip ?? 0;

                    parameters.Add(nameof(query.Skip), query.Skip, DbType.Int32);
                    parameters.Add(nameof(query.Take), query.Take, DbType.Int32);

                    sql += $" LIMIT @{nameof(query.Skip)}, @{nameof(query.Take)}";
                }

                Task<IEnumerable<UserSettings>> records = connection.QueryAsync<UserSettings>(sql, parameters);

                return new PagedResult<UserSettings>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task UpdateAsync(UserSettings record)
        {
            using(IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"UPDATE UserSettings SET
                    {nameof(UserSettings.GroupsEnabled)} = @{nameof(UserSettings.GroupsEnabled)},
                    {nameof(UserSettings.Modified)} = @{nameof(UserSettings.Modified)},
                    {nameof(UserSettings.UserName)} = @{nameof(UserSettings.UserName)},
                    {nameof(UserSettings.GroupId)} = @{nameof(UserSettings.GroupId)}
                    WHERE {nameof(UserSettings.Id)} = @{nameof(UserSettings.Id)}",
                record);
            }
        }
    }
}