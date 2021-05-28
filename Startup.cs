using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

using ImpactApi.Entities;
using ImpactApi.Services;
using ImpactApi.Settings;
using Microsoft.Extensions.Hosting;

using Pomelo.EntityFrameworkCore.MySql;

namespace ImpactApi
{
    public class Startup
    {
        private readonly string _devCorsPolicy = "DevCorsPolicy";
        private readonly string _prodCorsPolicy = "ProdCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_devCorsPolicy, builder =>
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

                options.AddPolicy(_prodCorsPolicy, builder =>
                    builder.WithOrigins("https://impact.moe")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            var databaseSettings = Configuration.GetSection(nameof(DatabaseSettings));
            var jwtSettings = Configuration.GetSection(nameof(JwtSettings));

            services.Configure<JwtSettings>(Configuration.GetSection(nameof(JwtSettings)));

            services.AddSingleton<JwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            string connectionString = "Host=" + databaseSettings.GetSection("Host").Value + ";" +
                "Port=" + databaseSettings.GetSection("Port").Value + ";" +
                "Database=" + databaseSettings.GetSection("Database").Value + ";" +
                "Uid=" + databaseSettings.GetSection("Uid").Value + ";" +
                "Password=" + databaseSettings.GetSection("Password").Value + ";";

            services.AddDbContext<ImpactDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ImpactDbContext>()
            .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.GetSection("ValidIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("ValidAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecurityKey").Value))
                };
            });

            services.AddScoped<JwtService>();

            services.AddControllers().AddNewtonsoftJson(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(_devCorsPolicy);
            }
            else
            {
                app.UseExceptionHandler("/error");

                app.UseCors(_prodCorsPolicy);
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
