using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.DataAccess;
using CoreMVCDemo.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMVCDemo
{
    public class Startup
    {

        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //配置应用程序所需的服务
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("StudentDBConnection")));

            services.AddMvc();
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();
            //services.AddScoped<IStudentRepository, MockStudentRepository>();
            //services.AddTransient<IStudentRepository, MockStudentRepository>();

            services.AddScoped<IStudentRepository,SQLStudentRepository>();
        }

        //配置应用程序的请求处理管道
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();

                /************
                 * 与 ASP.NET Core 中的大多数其他中间件组件一样，
                 * 我们也可以自定义UseDeveloperExceptionPage中间件。
                 * 每当您想要自定义中间件组件时，请始终记住您可能拥有相应的OPTIONS对象。
                 * 那么，要自定义UseDeveloperExceptionPage中间件
                 * SourceCodeLineCount属性指定在导致异常的代码行之前和之后要包含的代码行数。
                 * UseDeveloperExceptionPage中间件的位置尽可能的放置在其他中间件的位置前面，
                 * 因为如果管道中的后面的中间件组件引发异常，
                 * 它可以处理异常并显示Developer Exception页面
                 * **************/
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            else//如果不是开发者环境，使用统一处理
            {
                //
                //拦截异常
                //
                app.UseExceptionHandler("/Error");


                //
                //拦截404页面找不到信息
                //

                //app.UseStatusCodePages();//最不实用也不常用

                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");//推荐使用

                /* 302状态码:临时重定向
                 * UseStatusCodePagesWithRedirects:会发出重定向请求(302)，而地址栏的URL将发生更改
                 * 当发生真实错误的时候，它返回一个success status代码(200)，它在语义上显示的是不正确。
                 * UseStatusCodePagesWithReExecute:重新执行管道请求并返回原始状态代码（如404）
                 * 由于它是重新执行管道请求，而不是发出重定向请求时，我们可以看到在地址栏中保留了
                 * 原来的URL地址
                 */

               

            }

            //
            /*********UseDefaultFiles和UseStaticFiles中间件*****************/
            //
            //添加默认文件中间件
            //请注意：必须在UseStaticFiles之前,注册UseDefaultFiles来提供默认文件。
            //UseDefaultFiles是一个 URL 重写器，实际上并没有提供文件。
            //它只是将URL重写定位到默认文档，然后还是由静态文件中间件提供。
            //地址栏中显示的 URL 仍然是根节点的 URL，而不是重写的 URL。
            /*************
               以下是UseDefaultFiles中间件默认会去查找的地址信息
                - index.htm ----3
                - index.html -----4
                - default.htm   ---1
                - default.html  ---2
            *************/
            //app.UseDefaultFiles();

            ////将52abp.html指定为默认文档
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("52abp.html");
            ////添加默认文件中间件
            //app.UseDefaultFiles(defaultFilesOptions);


            ////UseStaticFiles()中间件仅提供 wwwroot 文件夹中的静态文件
            //app.UseStaticFiles();


            /********************UseFileServer 中间件
             * UseFileServer结合了UseStaticFiles，
             * UseDefaultFiles和UseDirectoryBrowser中间件的功能。
             * DirectoryBrowser中间件，支持目录浏览，并允许用户查看指定目录中的文件。
             * 我们可以用UseFileServer 中间件替换UseStaticFiles 和 UseDefaultFiles中间件。
             * *************************/
            //使用UseFileServer而不是UseDefaultFiles和UseStaticFiles
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("52abp1.html");
            app.UseFileServer(fileServerOptions);
            //对应用程序根 URL 的请求即http://localhost:10153由UseFileServer处理中间件和管道从那里反转

            //注意顺序
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc();//用于验证属性路由

            //app.Run(async (context) =>
            //{
            //    //throw new Exception("您的请求在管道中发生了一些异常，请检查。");
            //    await context.Response.WriteAsync("Hello World!Hosting Environment:"+env.EnvironmentName);
            //});
        }
    }
}
