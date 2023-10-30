using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.NetCore_Study
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession((option) =>
            {
                option.Cookie.Name = "philogic";
                option.IOTimeout = new TimeSpan(0, 30, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession(); //SessionMiddleware

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGet("/readWriteSession", async context =>
                {
                    int? count;
                    count = context.Session.GetInt32("count"); // read session

                    if (count == null)
                    {
                        count = 0;
                    }

                    count += 1;

                    context.Session.SetInt32("count", count.Value); // save session

                    await context.Response.WriteAsync($"Số lần truy cập readWriteSession là: {count}");
                });
            });
        }
    }
}
