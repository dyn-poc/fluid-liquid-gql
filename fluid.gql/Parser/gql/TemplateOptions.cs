using System.Dynamic;
using Fluid;
using fluid.gql.Helpers;
using Fluid.Values;
using Newtonsoft.Json.Linq;

namespace fluid.gql.Services;


     public static class AccountAccessGQLTemplateOptions
     {

      
          
          public static TemplateOptions ConfigureGQLAccountAccess(this TemplateOptions options)
          {
               options.ConfigureAccountAccess();
                var memberAccessStrategy = options.MemberAccessStrategy;
              
               
               memberAccessStrategy.Register<JObject, FluidValue>((source, name) =>
               {
                    var token = source.GetValue(name, StringComparison.OrdinalIgnoreCase);
                    return (token switch
                    {
                         JObject or JArray => ToFluidValue(token, options),
                         _ => ToFluidValue($"{source.Path}.{name}", options)
                    });
               });

               
       

               return options;
          }


          private static Task<FluidValue> ToFluidValue(object? input, TemplateOptions options) => Task.FromResult(FluidValue.Create(input, options));

     
   
}