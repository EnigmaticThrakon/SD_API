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
    public class RunRepository : IRunsRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public RunRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(Run record)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                connection.Open();

                await connection.ExecuteAsync($@"INSERT INTO Runs
                (
                    {nameof(Run.Id)},
                    {nameof(Run.Modified)},
                    {nameof(Run.AssociatedUnit)},
                    {nameof(Run.StartTime)},
                    {nameof(Run.EndTime)},
                    {nameof(Run.Duration)},
                    {nameof(Run.NumEntries)}
                ) 
                VALUES 
                (
                    @{nameof(Run.Id)},
                    @{nameof(Run.Modified)},
                    @{nameof(Run.AssociatedUnit)},
                    @{nameof(Run.StartTime)},
                    @{nameof(Run.EndTime)},
                    @{nameof(Run.Duration)},
                    @{nameof(Run.NumEntries)}
                )", record);

                string newTableName = record.Id.ToString().Replace("-", "");
                await connection.ExecuteAsync($@"CREATE TABLE {newTableName} LIKE RunDataTemplate");
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"DELETE FROM Runs WHERE Id = '" + id.ToString() + "';");
            }
        }

        public async Task<PagedResult<Run>> QueryAsync(RunQuery? query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new RunQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(RunQuery.SortColumns.Modified);
                }

                string sql = $@"SELECT
                u.{nameof(Run.Id)},
                u.{nameof(Run.Modified)},
                u.{nameof(Run.AssociatedUnit)},
                u.{nameof(Run.StartTime)},
                u.{nameof(Run.EndTime)},
                u.{nameof(Run.Duration)},
                u.{nameof(Run.NumEntries)}
                FROM Runs u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.Guid);
                    sql += $@" AND {nameof(Run.Id)} = @{nameof(query.Id)}";
                }

                if (query.AssociatedUnit != null)
                {
                    parameters.Add(nameof(query.AssociatedUnit), query.AssociatedUnit, DbType.Guid);
                    sql += $@" AND {nameof(Run.AssociatedUnit)} = @{nameof(query.AssociatedUnit)}";
                }

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(Run.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.StartTime != null)
                {
                    parameters.Add(nameof(query.StartTime), query.StartTime, DbType.DateTime);
                    sql += $@" AND {nameof(Run.StartTime)} = @{nameof(query.StartTime)}";
                }

                if (query.EndTime != null)
                {
                    parameters.Add(nameof(query.EndTime), query.EndTime, DbType.DateTime);
                    sql += $@" AND {nameof(Run.EndTime)} = @{nameof(query.EndTime)}";
                }

                if (query.Duration != null)
                {
                    parameters.Add(nameof(query.Duration), query.Duration, DbType.Int32);
                    sql += $@" AND {nameof(Run.Duration)} = @{nameof(query.Duration)}";
                }

                if (query.NumEntries != null)
                {
                    parameters.Add(nameof(query.NumEntries), query.NumEntries, DbType.Int64);
                    sql += $@" AND {nameof(Run.NumEntries)} = @{nameof(query.NumEntries)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Runs");
                Task<int> filteredRecords = connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) AS Results", parameters);

                sql += $" ORDER BY {query.GetOrderByString()}";

                if (query.Take > 0)
                {
                    query.Skip = query.Skip ?? 0;

                    parameters.Add(nameof(query.Skip), query.Skip, DbType.Int32);
                    parameters.Add(nameof(query.Take), query.Take, DbType.Int32);

                    sql += $" LIMIT @{nameof(query.Skip)}, @{nameof(query.Take)}";
                }

                Task<IEnumerable<Run>> records = connection.QueryAsync<Run>(sql, parameters);

                return new PagedResult<Run>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task UpdateAsync(Run record)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"UPDATE Runs SET
                    {nameof(Run.Id)} = @{nameof(Run.Id)},
                    {nameof(Run.Modified)} = @{nameof(Run.Modified)},
                    {nameof(Run.AssociatedUnit)} = @{nameof(Run.AssociatedUnit)},
                    {nameof(Run.StartTime)} = @{nameof(Run.StartTime)},
                    {nameof(Run.EndTime)} = @{nameof(Run.EndTime)},
                    {nameof(Run.Duration)} = @{nameof(Run.Duration)},
                    {nameof(Run.NumEntries)} = @{nameof(Run.NumEntries)}
                    WHERE {nameof(Run.Id)} = @{nameof(Run.Id)}",
                record);
            }
        }
    }
}