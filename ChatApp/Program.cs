using System.Text;
using Application;
using Application.Options;
using ChatApp.Hubs;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace ChatApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            Log.Information("Starting up");

            var builder = WebApplication.CreateBuilder(args);

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Information("Environment is {env}", environment);
            if (string.IsNullOrEmpty(environment))
                throw new ArgumentException("Не удалось получить переменную среды ASPNETCORE_ENVIRONMENT");

            builder.Configuration
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

            builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));

            // Логирование через Serilog.
            builder.Services.AddSerilog((services, logConf) =>
            {
                logConf.ReadFrom.Configuration(builder.Configuration);
                logConf.ReadFrom.Services(services);
                logConf.Enrich.FromLogContext();
            });

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat API", Version = "v1" });
                c.AddSecurityDefinition("jwt_auth", new OpenApiSecurityScheme
                {
                    Description = "JWT authorization header using Bearer scheme. Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "jwt_auth"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            var jwtOptions = new JwtOptions();
            builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = jwtOptions.ValidateIssuer,
                        ValidIssuer = jwtOptions.ValidIssuer,
                        ValidateAudience = jwtOptions.ValidateLifetime,
                        ValidAudience = jwtOptions.ValidAudience,
                        ValidateLifetime = jwtOptions.ValidateLifetime,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chat API"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatHub>("/Chat");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();
            });

            app.Run();
        }
    }
}
