using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TBCBanking.API.ApiConfigurations;
using TBCBanking.Domain.Models.Configuration;
using TBCBanking.Domain.Repositories;
using TBCBanking.Domain.Services;
using TBCBanking.Infrastructure.Repositories;
using TBCBanking.Infrastructure.Repositories.DbEntities;
using TBCBanking.Infrastructure.Services;
using TBCBanking.Infrastructure.Services.Validators;

namespace TBCBanking.API
{
    public class Startup
    {
        private string[] SupportedCultures;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SupportedCultures = Configuration.GetSection(nameof(SupportedCultures)).Get<string[]>();

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddFluentValidation(c => c.RegisterValidatorsFromAssembly(typeof(TBCBankingRequestValidators).Assembly))
                .AddDataAnnotationsLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(SupportedCultures[0])
                    .AddSupportedCultures(SupportedCultures)
                    .AddSupportedUICultures(SupportedCultures);
            });
            services.AddSwagger();
            services.Configure<ApiDefaults>(Configuration.GetSection(nameof(ApiDefaults)));

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IFileStorageRepository, FileStorageRepository>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<InputValidationActionFilter>();
            services.AddDbContext<MainDBContext>(o => { o.UseSqlServer(Configuration.GetConnectionString("MainDB"), builder => builder.EnableRetryOnFailure()); });
            services.AddMemoryCache();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLocalization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.AddSwagger();
        }
    }
}
