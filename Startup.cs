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
        // Đăng ký các dịch vụ sử dụng bởi ứng dụng, services là một DI container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache(); // Thêm dịch vụ dùng bộ nhớ lưu cache (session sử dụng dịch vụ này)
            services.AddSession();                // Thêm  dịch vụ Session, dịch vụ này cunng cấp Middleware: 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Thêm StaticFileMiddleware - nếu Request là yêu cầu truy cập file tĩnh,
            // Nó trả ngay về Response nội dung file và là điểm cuối pipeline, nếu  khác
            // nó gọi  Middleware phía sau trong Pipeline
            app.UseStaticFiles();
            // Thêm SessionMiddleware:  khôi phục, thiết lập - tạo ra session
            // gán context.Session, sau đó chuyển gọi ngay middleware
            // tiếp trong pipeline
            app.UseSession();
            app.UseCheckAccess();
            // Thêm EndpointRoutingMiddleware: ánh xạ Request gọi đến Endpoint (Middleware cuối)
            // phù hợp định nghĩa bởi EndpointMiddleware
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //  EndPoint(2) -  Middleware khi truy cập /Home với phương thức GET - nó làm Middleware cuối Pipeline
                endpoints.MapGet("/Home", async context =>
                {

                    int? count = context.Session.GetInt32("count");
                    count = (count != null) ? count + 1 : 1;
                    context.Session.SetInt32("count", count.Value);
                    await context.Response.WriteAsync($"Home page! {count}");

                });
            });

            // EndPoint(3)  app.Run tham số là hàm delegate tham số là HttpContex
            // - nó tạo điểm cuối của pipeline.
            app.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Page not found");
            });
        }
    }
}
