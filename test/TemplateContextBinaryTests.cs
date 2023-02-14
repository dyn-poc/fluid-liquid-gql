using System.Globalization;
using Fluid;
using Fluid.Ast;
using fluid.gql;
using fluid.gql.Extensions;
using fluid.gql.Services;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using AccountAccess = fluid.gql.AccountAccess;

namespace test;

public class TemplateContextBinaryTests
{
#if COMPILED
        private static FluidParser _parser = new FluidParser().Compile();
#else
        private static FluidParser _parser = new FluidParser();
#endif


        [Theory]
        [InlineData("profile.country == 'usa'", true)]
        [InlineData("profile.country startswith 'usa' ", true)]
        [InlineData("profile.country in ['eu'] ", false)]
        [InlineData("profile.country == 'eu'", false)]

        public async Task allow_section(string expression, bool expected)
        {
            // NB: Based on a previous implementation what would cache accessors too aggressively

            var services = new ServiceCollection();
            services.AddLiquidExpressionEvaluator();
           var sp= services.BuildServiceProvider();
            
             LiquidEvaluate parser = sp.GetRequiredService<LiquidEvaluate>();
             Assert.Equal(expected, await parser.EvaluateBoolAsync($"{{% allow {expression} %}}", new RuntimeExecutionContext()
             {
                 Account = new AccountAccess(JObject.FromObject(new
                 {
                     country = "usa"
                 }))
             }, CancellationToken.None));
        }

      
        [Theory]
        [InlineData("{{profile.country == 'usa'}}", true)]
        [InlineData("{{profile.country == 'eu'}}", false)]

        public async Task expression(string expression, bool expected)
        {
            // NB: Based on a previous implementation what would cache accessors too aggressively

            var services = new ServiceCollection();
            services.AddLiquidExpressionEvaluator();
            var sp= services.BuildServiceProvider();
            
            LiquidEvaluate parser = sp.GetRequiredService<LiquidEvaluate>();
            Assert.Equal(expected, await parser.EvaluateBoolAsync(expression, new RuntimeExecutionContext()
            {
                Account = new AccountAccess(JObject.FromObject(new
                {
                    country = "usa"
                }))
            }, CancellationToken.None));
        }

        [Theory]
        [InlineData("{{profile.country == 'usa'}}", "profile.country = 'usa'")]
        [InlineData("{{profile.country != 'eu'}}", "profile.country != 'eu'")]
        [InlineData("{{profile.age > 38}}", "profile.age > 38")]

        public async Task gql_render(string expression, string expected)
        {
            // NB: Based on a previous implementation what would cache accessors too aggressively

            var services = new ServiceCollection();
            services.AddLiquidExpressionEvaluator();
            var sp= services.BuildServiceProvider();
            
            LiquidEvaluate parser = sp.GetRequiredService<LiquidEvaluate>();
            Assert.Equal(expected, await parser.EvaluateGQLAsync(expression, new RuntimeExecutionContext()
            {
                Account = new AccountAccess(JObject.FromObject(new
                {
                    country = "usa"
                }))
            }, CancellationToken.None));
        }

    }