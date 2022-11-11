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
    public class UnitRepository : IUnitRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public UnitRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(Unit unit)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                connection.Open(); 

                await connection.ExecuteAsync($@"INSERT INTO Units
                (
                    {nameof(Unit.Id)},
                    {nameof(Unit.Modified)},
                    {nameof(Unit.SerialNumber)},
                    {nameof(Unit.IsDeleted)},
                    {nameof(Unit.IsConnected)},
                    {nameof(Unit.ConnectionId)},
                    {nameof(Unit.PairedIds)},
                    {nameof(Unit.IsAcquisitioning)},
                    {nameof(Unit.EnvironmentConfigurations)},
                    {nameof(Unit.Name)}
                ) 
                VALUES 
                (
                    @{nameof(Unit.Id)},
                    @{nameof(Unit.Modified)},
                    @{nameof(Unit.SerialNumber)},
                    @{nameof(Unit.IsDeleted)},
                    @{nameof(Unit.IsConnected)},
                    @{nameof(Unit.ConnectionId)},
                    @{nameof(Unit.PairedIds)},
                    @{nameof(Unit.IsAcquisitioning)},
                    @{nameof(Unit.EnvironmentConfigurations)},
                    @{nameof(Unit.Name)}
                )", unit);
            }
        }

        public async Task Delete(Guid id)
        {
            await Task.Run(() => { Console.WriteLine("Dumb Placeholder"); });
        }

        public async Task<PagedResult<Unit>> QueryAsync(UnitQuery? query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new UnitQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(UnitQuery.SortColumns.Modified);
                }

                string sql = $@"SELECT
                u.{nameof(Unit.Id)},
                u.{nameof(Unit.SerialNumber)},
                u.{nameof(Unit.Modified)},
                u.{nameof(Unit.IsDeleted)},
                u.{nameof(Unit.IsConnected)},
                u.{nameof(Unit.ConnectionId)},
                u.{nameof(Unit.PairedIds)},
                u.{nameof(Unit.IsAcquisitioning)},
                u.{nameof(Unit.EnvironmentConfigurations)},
                u.{nameof(Unit.Name)}
                FROM Units u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.String);
                    sql += $@" AND {nameof(Unit.Id)} = @{nameof(query.Id)}";
                }

                if (query.SerialNumber != null)
                {
                    parameters.Add(nameof(query.SerialNumber), query.SerialNumber, DbType.String);
                    sql += $@" AND {nameof(Unit.SerialNumber)} = @{nameof(query.SerialNumber)}";
                }

                if (query.IsAcquisitioning != null)
                {
                    parameters.Add(nameof(query.IsAcquisitioning), query.IsAcquisitioning, DbType.Boolean);
                    sql += $@" AND {nameof(Unit.IsAcquisitioning)} = @{nameof(query.IsAcquisitioning)}";
                }

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(Unit.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.IsDeleted != null)
                {
                    parameters.Add(nameof(query.IsDeleted), query.IsDeleted, DbType.Boolean);
                    sql += $@" AND {nameof(Unit.IsDeleted)} = @{nameof(query.IsDeleted)}";
                }

                if (query.IsConnected != null)
                {
                    parameters.Add(nameof(query.IsConnected), query.IsConnected, DbType.Boolean);
                    sql += $@" AND {nameof(Unit.IsConnected)} = @{nameof(query.IsConnected)}";
                }

                if (query.ConnectionId != null)
                {
                    parameters.Add(nameof(query.ConnectionId), query.ConnectionId, DbType.String);
                    sql += $@" AND {nameof(Unit.ConnectionId)} = @{nameof(query.ConnectionId)}";
                }

                if (query.Name != null)
                {
                    parameters.Add(nameof(query.Name), query.Name, DbType.String);
                    sql += $@" AND {nameof(Unit.Name)} = @{nameof(query.Name)}";
                }

                if(query.PairedId.HasValue) {
                    parameters.Add(nameof(query.PairedId), query.PairedId.Value, DbType.Guid);
                    sql += $@" AND {nameof(Unit.PairedIds)} like '%{query.PairedId.Value}%'";
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

                Task<IEnumerable<Unit>> records = connection.QueryAsync<Unit>(sql, parameters);

                return new PagedResult<Unit>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task UpdateAsync(Unit record)
        {
            using(IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"UPDATE Units SET
                    {nameof(Unit.SerialNumber)} = @{nameof(Unit.SerialNumber)},
                    {nameof(Unit.Modified)} = @{nameof(Unit.Modified)},
                    {nameof(Unit.IsDeleted)} = @{nameof(Unit.IsDeleted)},
                    {nameof(Unit.IsConnected)} = @{nameof(Unit.IsConnected)},
                    {nameof(Unit.ConnectionId)} = @{nameof(Unit.ConnectionId)},
                    {nameof(Unit.Name)} = @{nameof(Unit.Name)},
                    {nameof(Unit.IsAcquisitioning)} = @{nameof(Unit.IsAcquisitioning)},
                    {nameof(Unit.PairedIds)} = @{nameof(Unit.PairedIds)},
                    {nameof(Unit.EnvironmentConfigurations)} = @{nameof(Unit.EnvironmentConfigurations)}
                    WHERE {nameof(Unit.Id)} = @{nameof(Unit.Id)}",
                record);
            }
        }
    }
}