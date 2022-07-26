﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgerDevice.Core.Repositories;
using AgerDevice.Managers;
using AgerDevice.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AgerDevice.DependencyResolution
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAgerDevice(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddRepositories()
                .AddManagers()
                .AddServices();
        }

        public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUserRepository, DataAccess.MySQL.UserRepository>();
            serviceCollection.AddSingleton<IUnitRepository, DataAccess.MySQL.UnitRepository>();
            serviceCollection.AddSingleton<IRunsRepository, DataAccess.MySQL.RunRepository>();
            serviceCollection.AddSingleton<IRunDataRepository, DataAccess.MySQL.RunDataRepository>();
            
            //serviceCollection.AddSingleton<IRepository, DataAccess.MySQL.Repository>();

            return serviceCollection;
        }

        public static IServiceCollection AddManagers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<UserManager>();
            serviceCollection.AddTransient<UnitManager>();
            serviceCollection.AddTransient<RunDataManager>();
            serviceCollection.AddTransient<RunsManager>();
            //serviceCollection.AddTransient<Manager>();

            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHostedService, AcquisitionService>();
            serviceCollection.AddSingleton<AcquisitionService>(t => t.GetServices<IHostedService>().Where(x => x is AcquisitionService).Cast<AcquisitionService>().SingleOrDefault());

            return serviceCollection;
        }
    }
}
