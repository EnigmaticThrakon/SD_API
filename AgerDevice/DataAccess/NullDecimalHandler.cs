using System;
using System.Data;
using Dapper;

namespace AgerDevice.DataAccess
{
    public class NullableDecimalHandler : SqlMapper.TypeHandler<decimal?>
    {
        public override decimal? Parse(object value)
        {
            if (value == null || value is DBNull)
            {
                return null;
            }
            return Convert.ToDecimal(value);
        }

        public override void SetValue(IDbDataParameter parameter, decimal? value)
        {
            if (value.HasValue)
            {
                parameter.Value = value.Value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }
    }
}