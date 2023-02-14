using System.Dynamic;
using Fluid;
using fluid.gql.Helpers;
using Fluid.Values;
using Newtonsoft.Json.Linq;

namespace fluid.gql.Services;


     public static class AccountAccessOptions
     {

      
          
          public static TemplateOptions ConfigureAccountAccess(this TemplateOptions options)
          {
               var memberAccessStrategy = options.MemberAccessStrategy;

               options.ValueConverters.Add(x => x is JObject o ? new ObjectValue(o) : null);
               options.ValueConverters.Add(x => x is JValue v ? v.Value : null);
               options.ValueConverters.Add(x => x is ExpandoObject e ? new ObjectValue(e) : null);
               memberAccessStrategy.Register<LiquidPropertyAccessor, FluidValue>((x, name) => x.GetValueAsync(name));
               memberAccessStrategy.Register<LiquidObjectAccessor<JObject>, JObject>((x, name) => x.GetValueAsync(name));

            
               //good place to check schema
               memberAccessStrategy.Register<RuntimeExecutionContext, FluidValue>("profile", x => ToFluidValue(x.Account["profile"], options));
               memberAccessStrategy.Register<RuntimeExecutionContext, FluidValue>("data", x => ToFluidValue(x.Account["data"], options));
               memberAccessStrategy.Register<RuntimeExecutionContext, FluidValue>("preferences", x => ToFluidValue(x.Account["preferences"], options));
               memberAccessStrategy.Register<RuntimeExecutionContext, FluidValue>("subscriptions", x => ToFluidValue(x.Account["subscriptions"], options));
           
               memberAccessStrategy.Register<ExpandoObject>();
               memberAccessStrategy.Register<JObject>();
               memberAccessStrategy.Register<JValue>(o => o.Value);

               memberAccessStrategy.Register<ExpandoObject, object>((x, name) => ((IDictionary<string, object>)x)[name]);
               memberAccessStrategy.Register<JObject, object?>((source, name) => source.GetValue(name, StringComparison.OrdinalIgnoreCase));

       

               return options;
          }


          private static Task<FluidValue> ToFluidValue(object? input, TemplateOptions options) => Task.FromResult(FluidValue.Create(input, options));

     
   
}