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
                    {nameof(Unit.IsDeleted)}
                ) 
                VALUES 
                (
                    @{nameof(Unit.Id)},
                    @{nameof(Unit.Modified)},
                    @{nameof(Unit.SerialNumber)},
                    @{nameof(Unit.IsDeleted)}
                )", unit);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            return;
        }

        public async Task<PagedResult<Unit>> QueryAsync(UnitQuery query = null)
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
                u.{nameof(Unit.IsDeleted)}
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

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(Unit.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.IsDeleted != null)
                {
                    parameters.Add(nameof(query.IsDeleted), query.IsDeleted, DbType.DateTime);
                    sql += $@" AND {nameof(Unit.IsDeleted)} = @{nameof(query.IsDeleted)}";
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
                    {nameof(Unit.IsDeleted)} = @{nameof(Unit.IsDeleted)}
                    WHERE {nameof(Unit.Id)} = @{nameof(Unit.Id)}",
                record);
            }
        }
    }
}