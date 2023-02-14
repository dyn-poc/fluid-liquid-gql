using System.Text.Encodings.Web;
using Fluid;
using Fluid.Values;

namespace fluid.gql.Services
{
    public class LiquidEvaluate
    {
        private readonly ILiquidTemplateManager _liquidTemplateManager;

        public LiquidEvaluate(ILiquidTemplateManager liquidTemplateManager)
        {
            _liquidTemplateManager = liquidTemplateManager;
        }


        public async Task<bool?> EvaluateBoolAsync(string expression, RuntimeExecutionContext context, CancellationToken cancellationToken)
        {
            var templateContext = new TemplateContext(context, new TemplateOptions().ConfigureAccountAccess());
            var result = await _liquidTemplateManager.RenderAsync(expression, templateContext);
            return string.IsNullOrWhiteSpace(result) ? default : bool.Parse(result);
        }

   
        public async Task<string?> EvaluateGQLAsync(string expression, RuntimeExecutionContext context, CancellationToken cancellationToken)
        {
            var templateContext = new TemplateContext(context, new TemplateOptions().ConfigureGQLAccountAccess());
            new GQLParser().TryParse(expression, out var template, out var error);
            var result = await template.RenderAsync(templateContext);
            return string.IsNullOrWhiteSpace(result) ? default : result;
        }
    
    
    }
  
}