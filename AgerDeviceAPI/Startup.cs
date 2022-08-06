using StackExchange.Redis;
using AgerDevice.DependencyResolution;
using Microsoft.OpenApi.Models;
using FluentMigrator.Runner;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using AgerDevice.DataAccess.Migrations;

namespace AgerDeviceAPI
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Need to figure out why the connection string was returning null from the Configuration.GetConnectionString("AgerDevice");
            //      I know it's accessible from Program.cs, but it's not here
            string connectionString = "server=localhost;port=3306;uid=agerDeviceApp;pwd=Grhb3DR?DPPTgnzD;database=AgerDevice;";
            services.AddTransient<Func<IDbConnection>>(t => () => new MySql.Data.MySqlClient.MySqlConnection(connectionString));
            services.AddAgerDevice();

            services.AddSignalR();
            services.AddLogging(c =>
            {
                c.AddConsole();
                c.AddDebug();
            });

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddMySql5()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(CreatingInitialTables).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgerDevice API", Version = "v1" });
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseCors(builder =>
                builder.SetIsOriginAllowed(_ => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
            );

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AgerDevice.Hubs.DeviceHub>("agerDeviceHub");
                endpoints.MapHub<AgerDevice.Hubs.MonitorHub>("/monitorHub");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgerDevice API");
            });

            InitializeDatabase(app);
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (IServiceScope scope = app.ApplicationServices.CreateScope())
            {
                ILogger<Startup> logger = scope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                logger.LogEmphasized("Migration Complete");
            }
        }
    }
}
