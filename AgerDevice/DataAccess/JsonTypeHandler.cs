using System;
using System.Data;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AgerDevice.DataAccess
{
    public class JsonTypeHandler : SqlMapper.ITypeHandler
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public JsonTypeHandler(bool camelCase = true)
        {
            _jsonSerializerSettings = new JsonSerializerSettings();
            if (camelCase)
            {
                _jsonSerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
            }
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            if (value == null)
            {
                parameter.Value = null;
            }
            else
            {
                parameter.Value = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            }
        }

        public object? Parse(Type destinationType, object value)
        {
            if (value == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(value as string, destinationType);
        }
    }
}
