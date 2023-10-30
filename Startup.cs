using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App.NetCore_Study
{
    public class Startup
    {
        IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var mailSettings = _configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);

            services.AddTransient<SendMailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                endpoints.MapGet("/TestSendMail", async context =>
                {
                    using var client = new SmtpClient("localhost");
                    var message = await MailUtils.MailUtils.SendMail("nguyentruongphi15032003@gmail.com", "nguyentruongphi15032003@gmail.com", "Test", "Xin chao", client);
                    await context.Response.WriteAsync(message);
                });

                endpoints.MapGet("/TestSendGmail", async context =>
                {
                    // tai khoan gmail chua xac minh 2 buoc moi dung duoc
                    var _gmail = "nguyentruongphi15032003@gmail.com";
                    var _password = "";

                    using var client = new SmtpClient("smtp.gmail.com");
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_gmail, _password);

                    var message = await MailUtils.MailUtils.SendGmail("nguyentruongphi15032003@gmail.com", "nguyentruongphi15032003@gmail.com", "Test", "Xin chao", client);
                    await context.Response.WriteAsync(message);
                });

                endpoints.MapGet("/TestSendGmailService", async context =>
                {
                    var sendMailService = context.RequestServices.GetService<SendMailService>();

                    var mailContent = new MailContent();
                    mailContent.To = "nguyentruongphi15032003@gmail.com";
                    mailContent.Subject = "KIEM TRA THU MAIL";
                    mailContent.Body = "<h1>Test mail service</h1><i>Xin chao my friends</i>";

                    var result = await sendMailService.SendMail(mailContent);

                    await context.Response.WriteAsync(result);
                });
            });
        }
    }
}
