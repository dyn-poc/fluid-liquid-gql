using Fluid;
using Fluid.Ast;
using Fluid.Values;

namespace fluid.gql
{
    public class LiquidParser : FluidParser
    {
        public LiquidParser()
        {
            RegisterExpressionTag("allow", (exp, w, e, c) =>
            {
                var value = exp.EvaluateAsync(c).Result.ToBooleanValue();
                w.Write(value);
                return Statement.Normal();
            });
        }


        public void AddToLower(TemplateContext context)
        {
            var lowercase = new FunctionValue((args, context) =>
            {
                var firstArg = args.At(0).ToStringValue();
                var lower = firstArg.ToLowerInvariant();
                return new ValueTask<FluidValue>(new StringValue(lower));
            });

            context.SetValue("tolower", lowercase);
        }
    }
}