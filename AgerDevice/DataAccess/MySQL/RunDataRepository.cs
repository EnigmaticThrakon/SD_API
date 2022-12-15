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
    public class RunDataRepository : IRunDataRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public RunDataRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(RunData record)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                connection.Open();

                string tableName = record.AssociatedRun.ToString().Replace("-", "");
                await connection.ExecuteAsync("INSERT INTO " + tableName + $@" 
                (
                    {nameof(RunData.Timestamp)},
                    {nameof(RunData.Temperature)},
                    {nameof(RunData.Weight)},
                    {nameof(RunData.AirFlow)},
                    {nameof(RunData.Humidity)},
                    {nameof(RunData.DoorClosed)}
                ) 
                VALUES 
                (
                    @{nameof(RunData.Timestamp)},
                    @{nameof(RunData.Temperature)},
                    @{nameof(RunData.Weight)},
                    @{nameof(RunData.AirFlow)},
                    @{nameof(RunData.Humidity)},
                    @{nameof(RunData.DoorClosed)}
                )", record);
            }
        }

        public async Task<PagedResult<RunData>> QueryAsync(RunDataQuery? query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new RunDataQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(RunDataQuery.SortColumns.Modified);
                }

                string tableName = query.AssociatedRun.ToString().Replace("-", "");
                string sql = $@"SELECT
                u.{nameof(RunData.Timestamp)},
                u.{nameof(RunData.Temperature)},
                u.{nameof(RunData.Weight)},
                u.{nameof(RunData.AirFlow)},
                u.{nameof(RunData.Humidity)},
                u.{nameof(RunData.DoorClosed)}
                FROM " + tableName + $@" u
                WHERE 1=1 ";

                if (query.Timestamp != null)
                {
                    parameters.Add(nameof(query.Timestamp), query.Timestamp, DbType.DateTime);
                    sql += $@" AND {nameof(RunData.Timestamp)} = @{nameof(query.Timestamp)}";
                }

                if (query.Temperature != null)
                {
                    parameters.Add(nameof(query.Temperature), query.Temperature, DbType.Decimal);
                    sql += $@" AND {nameof(RunData.Temperature)} = @{nameof(query.Temperature)}";
                }

                if (query.Weight != null)
                {
                    parameters.Add(nameof(query.Weight), query.Weight, DbType.Decimal);
                    sql += $@" AND {nameof(RunData.Weight)} = @{nameof(query.Weight)}";
                }

                if (query.AirFlow != null)
                {
                    parameters.Add(nameof(query.AirFlow), query.AirFlow, DbType.Decimal);
                    sql += $@" AND {nameof(RunData.AirFlow)} = @{nameof(query.AirFlow)}";
                }

                if (query.Humidity != null)
                {
                    parameters.Add(nameof(query.Humidity), query.Humidity, DbType.Decimal);
                    sql += $@" AND {nameof(RunData.Humidity)} = @{nameof(query.Humidity)}";
                }

                if (query.DoorClosed != null)
                {
                    parameters.Add(nameof(query.DoorClosed), query.DoorClosed, DbType.Boolean);
                    sql += $@" AND {nameof(RunData.DoorClosed)} = @{nameof(query.DoorClosed)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM " + query.AssociatedRun.ToString().Replace("-", ""));
                Task<int> filteredRecords = connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) AS Results", parameters);

                sql += $" ORDER BY {query.GetOrderByString()}";

                if (query.Take > 0)
                {
                    query.Skip = query.Skip ?? 0;

                    parameters.Add(nameof(query.Skip), query.Skip, DbType.Int32);
                    parameters.Add(nameof(query.Take), query.Take, DbType.Int32);

                    sql += $" LIMIT @{nameof(query.Skip)}, @{nameof(query.Take)}";
                }

                Task<IEnumerable<RunData>> records = connection.QueryAsync<RunData>(sql, parameters);

                return new PagedResult<RunData>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task Delete(Guid id)
        {
            return;
        }

        public async Task UpdateAsync(RunData record)
        {
            return;
        }
    }
}