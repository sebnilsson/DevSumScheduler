using DevSumScheduler.Data;
using DevSumScheduler.WebApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevSumScheduler.WebApp
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
            services.AddMemoryCache();
            services.AddSingleton<IDataProvider>(service =>
            {
                var memoryCache = service.GetService<IMemoryCache>();
                var dataProvider = new HttpDataProvider();

                return new CachedDataProvider(memoryCache, dataProvider);
            });
            services.AddSingleton<ISpeakerDataProvider>(service =>
            {

                var memoryCache = service.GetService<IMemoryCache>();
                var dataProvider = new HttpSpeakerDataProvider();

                return new CachedSpeakerDataProvider(memoryCache, dataProvider);
            });
            services.AddScoped<DayDataService>();
            services.AddScoped<SpeakerDataService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}