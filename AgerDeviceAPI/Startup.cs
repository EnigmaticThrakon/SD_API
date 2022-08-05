using StackExchange.Redis;
using AgerDevice.DependencyResolution;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace AgerDeviceAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ConnectionMultiplexer>(t => ConnectionMultiplexer.Connect("localhost"));
            services.AddAgerDevice();

            services.AddSignalR();
            services.AddLogging(c =>
            {
                c.AddConsole();
                c.AddDebug();
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgerDevice API", Version = "v1" });
            });
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgerDevice API", Version = "v1" });

            //    //c.DocumentFilter<LowercaseDocumentFilter>();
            //    c.OperationFilter<SecurityRequirementsOperationFilter>();

            //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //        Name = "Authorization",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey
            //    });

            //    var apiXmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            //    //var libraryXmlPath = Path.Combine(AppContext.BaseDirectory, $"VetGuardian.xml");

            //    c.IncludeXmlComments(apiXmlPath);
            //    //c.IncludeXmlComments(libraryXmlPath);
            //});

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
        }
    }
}
