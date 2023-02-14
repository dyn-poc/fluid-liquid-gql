using System.Text.Encodings.Web;
using Fluid;
using Microsoft.Extensions.Caching.Memory;
 
namespace fluid.gql.Services
{
    public class LiquidTemplateManager : ILiquidTemplateManager
    {
        private readonly LiquidParser _parser;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;
 
        public LiquidTemplateManager(LiquidParser parser, IMemoryCache memoryCache,   IServiceProvider serviceProvider)
        {
            _parser = parser;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
         }

        public async Task<string?> RenderAsync(string source, TemplateContext context)
        {
            if (string.IsNullOrWhiteSpace(source))
                return default!;

             var result = GetCachedTemplate(source);

            return await result.RenderAsync(context);
        }

        private IFluidTemplate GetCachedTemplate(string source)
        {
            string error;

            var result = _memoryCache.GetOrCreate(
                source,
                e =>
                {
                    if (!TryParse(source, out var parsed, out error))
                        TryParse(error, out parsed, out error);

                    e.SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    return parsed;
                });
            return result;
        }

        public bool Validate(string template, out string error) => TryParse(template, out _, out error);
        
        private bool TryParse(string template, out IFluidTemplate result, out string error) => _parser.TryParse(template, out result, out error);
    }
    
    public interface ILiquidTemplateManager
    {
        /// <summary>
        /// Renders a Liquid template as a <see cref="string"/>.
        /// </summary>
        Task<string?> RenderAsync(string template, TemplateContext context);

        /// <summary>
        /// Validates a Liquid template.
        /// </summary>
        bool Validate(string template, out string error);
    }

}