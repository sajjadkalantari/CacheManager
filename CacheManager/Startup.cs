using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CacheManager.Repositories;
using CacheManager.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CacheManager
{
    public class Startup
    {
        public delegate IDistributedCache ServiceResolver(string key);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();                     

            services.AddDistributedMemoryCache();
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = "localhost:6379";
                option.InstanceName = "master";
            });

            services.AddTransient<IBookService, BookService>();
            services.AddTransient<HttpClient>();
            services.AddSingleton<IApiEndPointsSetting>(Configuration.GetSection("ApiEndPoints").Get<ApiEndPointsSetting>());
            services.AddTransient<IRepository<int, Book>, BookRepository>();
            services.AddTransient<IRepository<int, Comment>, CommentRepository>();
            
            var cacheSettings = Configuration.GetSection("CacheOptions").Get<CacheSetting[]>();
            services.AddSingleton<ICacheManager>(serviceProvider =>
                new SampleCacheManager(CacheManagerFactory.CreateCacheHierachy(serviceProvider.GetServices<IDistributedCache>().ToList(), cacheSettings.ToList<ICacheSetting>()))
            );

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
