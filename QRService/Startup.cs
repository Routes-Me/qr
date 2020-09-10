using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QRService.Abstraction;
using QRService.Repository;
using QRService.Helper.Models;

namespace QRService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<QRService.Models.DBModels.qrserviceContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IQRCodesRepository, QRCodesRepository>();
            services.AddScoped<IQRCodesAdvertisementsRepository, QRCodesAdvertisementsRepository>();
            services.AddScoped<IQRCodesUsersRepository, QRCodesUsersRepository>();
            services.AddScoped<IRedeemauthoritiesRepository, RedeemauthoritiesRepository>();
            services.AddScoped<IRedeemedCodesRepository, RedeemedCodesRepository>();

            var azureConfigSection = Configuration.GetSection("AzureStorageBlobConfig");
            services.Configure<AzureStorageBlobConfig>(azureConfigSection);
            var azureConfig = azureConfigSection.Get<AzureStorageBlobConfig>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
