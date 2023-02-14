using fluid.gql.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace fluid.gql.Extensions
{
    public static class LiquidServiceCollectionExtensions
    {
        public static void AddLiquidExpressionEvaluator(this IServiceCollection services)
        {
            services.AddMemoryCache();
             services
                .TryAddScoped<LiquidEvaluate>();
            services.AddScoped<ILiquidTemplateManager, LiquidTemplateManager>();
            services.AddSingleton<LiquidParser>();
        }
 
    }
}