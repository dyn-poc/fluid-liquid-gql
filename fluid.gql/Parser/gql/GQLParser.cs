using System.Linq.Expressions;
using Fluid;
using Fluid.Ast;
using Fluid.Ast.BinaryExpressions;
using Fluid.Values;
using Parlot.Fluent;
using BinaryExpression = Fluid.Ast.BinaryExpression;
using Expression = Fluid.Ast.Expression;

namespace fluid.gql;

public class GQLParser : FluidParser
{
    public GQLParser( )
    {
                 

       /*
       RegisterParserTag("gql", ArgumentsList, static async (a, w, e, c) =>
        {
            // a is a List<FilterArgument>
            // Do whatever you want here
            
            return Completion.Normal;
        });*/
       
     


        
        RegisteredOperators["=="] = (a, b) => new GQLExpression(a, b, "=");
        RegisteredOperators["!="] = (a, b) => new GQLExpression(a, b, "!=");
        RegisteredOperators[">="] = (a, b) => new GQLExpression(a, b, ">=");
        RegisteredOperators["<="] = (a, b) => new GQLExpression(a, b, ">=");
        RegisteredOperators[">"] = (a, b) => new GQLExpression(a, b, ">");
        RegisteredOperators["<"] = (a, b) => new GQLExpression(a, b, ">");


            
                
    }
    
    public sealed class GQLExpression : Expression
    {
        private readonly Expression _left;
        private readonly Expression _right;
        private readonly Expression _op;

        public GQLExpression(Expression left, Expression right, string op)
           
        {
            _left = left;
            _right = right;
            _op = new  LiteralExpression(new StringValue(op));
        }


        FluidValue Evaluate(string leftValue, string rightValue, string op) =>
           new StringValue( $"{leftValue} {op} {rightValue}");
            
            


        public override ValueTask<FluidValue> EvaluateAsync(TemplateContext context)
        {
            ValueTask<FluidValue> async_left = _left.EvaluateAsync(context);
            ValueTask<FluidValue> async_right = _right.EvaluateAsync(context);
            ValueTask<FluidValue> async_op = _op.EvaluateAsync(context);
            return async_left.IsCompletedSuccessfully && async_right.IsCompletedSuccessfully ? (ValueTask<FluidValue>) 
                Evaluate(async_left.Result.ToStringValue(),  async_right.Result.IsInteger()? async_right.Result.ToStringValue(): $"'{async_right.Result.ToStringValue()}'" ,async_op.Result.ToStringValue()) : Awaited(async_left, async_right,async_op );

        }

        
        private async ValueTask<FluidValue> Awaited(
            ValueTask<FluidValue> leftTask,
            ValueTask<FluidValue> rightTask,
            ValueTask<FluidValue> opTask

            )
        {
            var leftValue = await leftTask;
            var rightValue = await rightTask;
            var opValue = await opTask;
            var fluidValue = Evaluate(leftValue.ToStringValue(), rightValue.ToStringValue(), opValue.ToStringValue());
            leftValue = (FluidValue) null;
            return fluidValue;
        }
    }
        
    
}