using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class TestOptionsMiddleware : IMiddleware
{
    TestOptions _testOptions { get; set; }
    ProductNames _productNames { get; set; }
    public TestOptionsMiddleware(IOptions<TestOptions> options, ProductNames productNames)
    {
        _testOptions = options.Value;
        _productNames = productNames;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await context.Response.WriteAsync("Show options in TestOptionsMiddleware\n");

        var stringBuilder = new StringBuilder();

        stringBuilder.Append("TESTOPTIONS\n");
        stringBuilder.Append($"opt_key1 = {_testOptions.opt_key1}\n");
        stringBuilder.Append($"TestOptions.opt_key2.k1 = {_testOptions.opt_key2.k1}\n");
        stringBuilder.Append($"TestOptions.opt_key2.k2 = {_testOptions.opt_key2.k2}\n");

        foreach (var prName in _productNames.GetNames())
        {
            stringBuilder.Append(prName + "\n");
        }

        await context.Response.WriteAsync(stringBuilder.ToString());
        await next(context);
    }
}