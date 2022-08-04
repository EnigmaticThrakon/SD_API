using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Services;
using AgerDevice.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgerDevice.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAgerDevice(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddAgerDeviceRedisHandlers()
                .AddAgerDeviceServices();
        }

        public static IServiceCollection AddAgerDeviceRedisHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<UsersHandler>();
            serviceCollection.AddSingleton<SettingsHandler>();
            serviceCollection.AddSingleton<UnitsHandler>();

            return serviceCollection;
        }

        public static IServiceCollection AddAgerDeviceServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHostedService, AcquisitionService>();
            serviceCollection.AddSingleton<AcquisitionService>(t => t.GetServices<IHostedService>().Where(x => x is AcquisitionService).Cast<AcquisitionService>().SingleOrDefault());

            return serviceCollection;
        }
    }
}
