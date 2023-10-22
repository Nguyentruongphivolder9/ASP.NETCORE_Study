using Microsoft.AspNetCore.Builder;

public static class UseMiddlewares
{
    // Mở rộng cho IApplicationBuilder phương thức UseCheckAccess
    public static IApplicationBuilder UseCheckAccess(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CheckAccessMiddleware>();
    }
}