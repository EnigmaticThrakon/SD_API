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
    public class LinkRepository : ILinkRepository
    {
        private readonly Func<IDbConnection> _connectionFactory;
        public LinkRepository(Func<IDbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateAsync(Link unit)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                connection.Open(); 

                await connection.ExecuteAsync($@"INSERT INTO Links
                (
                    {nameof(Link.Id)},
                    {nameof(Link.Modified)},
                    {nameof(Link.GroupId)}
                ) 
                VALUES 
                (
                    @{nameof(Link.Id)},
                    @{nameof(Link.Modified)},
                    @{nameof(Link.GroupId)}
                )", unit);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            return;
        }

        public async Task<PagedResult<Link>> QueryAsync(LinkQuery query = null)
        {
            using (IDbConnection connection = _connectionFactory())
            {
                DynamicParameters parameters = new DynamicParameters();
                query = query ?? new LinkQuery();

                if (!query.Sort.Any())
                {
                    query.OrderByDescending(LinkQuery.SortColumns.Modified);
                }

                string sql = $@"SELECT
                u.{nameof(Link.Id)},
                u.{nameof(Link.GroupId)},
                u.{nameof(Link.Modified)},
                FROM Links u
                WHERE 1=1 ";

                if (query.Id != null)
                {
                    parameters.Add(nameof(query.Id), query.Id, DbType.String);
                    sql += $@" AND {nameof(Link.Id)} = @{nameof(query.Id)}";
                }

                if (query.Modified != null)
                {
                    parameters.Add(nameof(query.Modified), query.Modified, DbType.DateTime);
                    sql += $@" AND {nameof(Link.Modified)} = @{nameof(query.Modified)}";
                }

                if (query.GroupId != null)
                {
                    parameters.Add(nameof(query.GroupId), query.GroupId, DbType.Guid);
                    sql += $@" AND {nameof(Link.GroupId)} = @{nameof(query.GroupId)}";
                }

                Task<int> totalRecords = connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Links");
                Task<int> filteredRecords = connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM ({sql}) AS Results", parameters);

                sql += $" ORDER BY {query.GetOrderByString()}";

                if (query.Take > 0)
                {
                    query.Skip = query.Skip ?? 0;

                    parameters.Add(nameof(query.Skip), query.Skip, DbType.Int32);
                    parameters.Add(nameof(query.Take), query.Take, DbType.Int32);

                    sql += $" LIMIT @{nameof(query.Skip)}, @{nameof(query.Take)}";
                }

                Task<IEnumerable<Link>> records = connection.QueryAsync<Link>(sql, parameters);

                return new PagedResult<Link>(await records, await totalRecords, await filteredRecords);
            }
        }

        public async Task UpdateAsync(Link record)
        {
            using(IDbConnection connection = _connectionFactory())
            {
                await connection.ExecuteAsync($@"UPDATE Links SET
                    {nameof(Link.GroupId)} = @{nameof(Link.GroupId)},
                    {nameof(Link.Modified)} = @{nameof(Link.Modified)},
                    WHERE {nameof(Link.Id)} = @{nameof(Link.Id)}",
                record);
            }
        }
    }
}