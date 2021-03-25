using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ImpactApi.Services;
using ImpactApi.Models;

namespace ImpactApi
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
            services.Configure<ImpactDatabaseSettings>(
                Configuration.GetSection(nameof(ImpactDatabaseSettings)));

            services.AddSingleton<IImpactDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ImpactDatabaseSettings>>().Value);

            services.AddSingleton<ImpactDatabaseService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
