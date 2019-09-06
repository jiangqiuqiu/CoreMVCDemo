# CoreMVCDemo
根据52ABP视频
学习.NET CORE建立的网站



.NET CORE框架本质
https://www.cnblogs.com/jackyfei/p/10838586.html

public class Program
{
    public static void Main()
    => new WebHostBuilder()
        .UseKestrel()
        .Configure(app => app.Use(context => context.Response.WriteAsync("Hello World!")))
        .Build()
        .Run();
}

WebHostBuilder、Server（即Kestrel）、ApplicationBuilder(即app)三大重要的对象
WebHostBuilder这个父亲生出WebHost这个孩子，WebHost又生成整个ASP.NET Core最核心的内容，即由Server和中间件(Middleware)构成的管道Pipeline。
继续把Pipeline拆开，有个很重要的ApplicationBuilder对象，里面包含Middleware、RequestDelegate。至于HttpContext是独立共享的对象，贯穿在整个管道中间，至此7大对象全部出场完毕。

Configure是个什么玩意？看下代码：
public IWebHostBuilder Configure(Action<IApplicationBuilder> configure)
{
    _configures.Add(configure);
    return this;
}

我们看到他是一个接受IApplicationBuilder的委托！继续刨根问底，IApplicationBuilder是什么玩意？看下源码：
public interface IApplicationBuilder
{
    IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
    RequestDelegate Build();
}

他是一个注册中间件和生成Application的容器，那么Application是什么呢？源码没有这个对象，但是看代码（如下所示）我们可以知道他是真正的委托执行者（Handler），执行是一个动作可以理解为app，我猜想这是取名为ApplicationBuilder的原因。
public RequestDelegate Build()
{
    _middlewares.Reverse();
    return httpContext =>
    {
　　　   //_是一个有效的标识符，因此它可以用作参数名称
        RequestDelegate next = _ => { _.Response.StatusCode = 404; return Task.CompletedTask; };
　　　　
        foreach (var middleware in _middlewares)
        {
            next = middleware(next);
        }
        return next(httpContext);
    };
}

![image](https://github.com/jiangqiuqiu/MyResource/MyImages/COREProcess.png?raw=true)
WebHostBuilder开始Build的那一刻开始，WebHost被构造，Server被指定，Middlewares被指定，等WebHost真正启动的时候，Server开始监听，收到请求后，Middleware开始执行。
