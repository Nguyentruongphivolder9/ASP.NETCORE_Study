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
            /*
                Phương thức ConfigureServices cho phép truy cập đến các dịch vụ, dependency được Inject vào
                Webhost. Hoặc bạn cũng có thể đưa thêm các dependency tại đây.
            */
        }

        // IHostingEnvironment  env cho phép truy cập các biến môi trường, thư mục nguồn, thư mục file.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Thêm StaticFileMiddleware vào pipeline - kích hoạt truy cập file tĩnh trong wwwroot
            // file wwwroot/html/helloworld.html truy cập được http://localhost:5000/html/helloworld.html
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Điểm cuỗi của pipeline khi địa chỉ truy cập là /Abc
                endpoints.Map("/home", async context =>
                {
                    string html = @"
                        <!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset=""UTF-8"">
                            <title>Trang web đầu tiên</title>
                            <link rel=""stylesheet"" href=""/css/bootstrap.min.css"" />
                            <script src=""/js/jquery.min.js""></script>
                            <script src=""/js/popper.min.js""></script>
                            <script src=""/js/bootstrap.min.js""></script>


                        </head>
                        <body>
                            <nav class=""navbar navbar-expand-lg navbar-dark bg-danger"">
                                    <a class=""navbar-brand"" href=""#"">Brand-Logo</a>
                                    <button class=""navbar-toggler"" type=""button"" data-toggle=""collapse"" data-target=""#my-nav-bar"" aria-controls=""my-nav-bar"" aria-expanded=""false"" aria-label=""Toggle navigation"">
                                            <span class=""navbar-toggler-icon""></span>
                                    </button>
                                    <div class=""collapse navbar-collapse"" id=""my-nav-bar"">
                                    <ul class=""navbar-nav"">
                                        <li class=""nav-item active"">
                                            <a class=""nav-link"" href=""#"">Trang chủ</a>
                                        </li>
                                    
                                        <li class=""nav-item"">
                                            <a class=""nav-link"" href=""#"">Học HTML</a>
                                        </li>
                                    
                                        <li class=""nav-item"">
                                            <a class=""nav-link disabled"" href=""#"">Gửi bài</a>
                                        </li>
                                </ul>
                                </div>
                            </nav> 
                            <p class=""display-4 text-danger"">Đây là trang đã có Bootstrap</p>
                        </body>
                        </html>
                    ";
                    await context.Response.WriteAsync(html);
                });

                // Điểm cuỗi của pipeline khi địa chỉ truy cập là /
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
