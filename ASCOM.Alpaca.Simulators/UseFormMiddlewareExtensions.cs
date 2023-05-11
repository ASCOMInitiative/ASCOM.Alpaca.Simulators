using Microsoft.AspNetCore.Builder;

namespace ASCOM.Alpaca.Simulators
{
    public static class UseFormIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseFormIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FormIdMiddleware>();
        }
    }
}
