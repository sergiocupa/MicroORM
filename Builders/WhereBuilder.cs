


using System.Linq.Expressions;
using System.Text;


namespace MicroORM.Builder
{

    internal class WhereBuilder
    {

        internal static bool Build<T>(Expression<Func<T, bool>> predicate, WhereResult result)
        {
            if (predicate == null) return false;

            var body = predicate.Body;
            Build(body, result);
            return true;
        }

        private static bool Build(Expression predicate, WhereResult result)
        {
            if (predicate == null) return false;

            var biexp = predicate as BinaryExpression;

            if(biexp != null)
            {
                result.Content.Append("(");

                var left = biexp.Left as MemberExpression;
                var res = Build(left, result);
                if(!res)
                {
                    //var value = ExpressionBuilder.Evaluate(left);
                    //result.Content.Append(value);
                    result.Content.Append(left.Member.Name);// TO-DO: Convensionou para poder testar. Implementar possibilidade de usar Member No Left e Rigth
                }

                result.Content.Append(" ");
                result.Content.Append(ExtractNode(biexp.NodeType));
                result.Content.Append(" ");

                var right = biexp.Right as MemberExpression;
                var resr = Build(right, result);
                if (!resr)
                {
                    var value = ExpressionBuilder.Evaluate(right);
                    result.Content.Append(value);
                }

                result.Content.Append(")");
                return true;
            }
            else
            {
                return false;
            }
        }


        private static string ExtractNode(ExpressionType etype)
        {
            return "*";// TO-DO:  Implementar
        }


    }


    internal class WhereResult
    {
        public StringBuilder Content { get; set; }
    }
}
