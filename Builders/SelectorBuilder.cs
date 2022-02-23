


using MicroORM.Map;
using System.Linq.Expressions;


namespace MicroORM.Builder
{

    internal class SelectorBuilder
    {


        internal static List<Field> BuildToSelect<T>(Expression<Func<T, object>> selector, Mapped map)
        {
            List<Field> fields = new List<Field>();

            var ex = selector.Body;
            var mInitExp = ex as MemberInitExpression;
            if (mInitExp == null)
            {
                var newExp = ex as NewExpression;
                if (newExp != null)
                {
                    var args = newExp.Arguments;
                    var Members = newExp.Members;
                    int i = 0;
                    while (i < Members.Count)
                    {
                        var member = args[i] as MemberExpression;
                        var imember = Members[i];

                        if (member != null)
                        {
                            Field column = null;
                            if (map.Fields.TryGetValue(member.Member.Name, out column))
                            {
                                fields.Add(column);
                            }
                        }
                        i++;
                    }
                }
            }
            else
            {
                throw new Exception("Not implemented");
            }
            return fields;
        }


        internal static List<Field> BuildToUpdate<T>(Expression<Func<T, object>> selector, Mapped map)
        {
            List<Field> fields = new List<Field>();

            if (selector != null)
            {
                var ex = selector.Body;
                var mInitExp = ex as MemberInitExpression;

                var bind = mInitExp.Bindings;
                foreach (MemberAssignment item in bind)
                {
                    string nome = item.Member.Name;
                    object valor = null;

                    var exCall = item.Expression;
                    var member = exCall as MemberExpression;
                    var method = member.Expression as MethodCallExpression;

                    if (member != null)
                    {
                        if (method != null)
                        {
                            string msg = "" +
                            "Não foi implementado Array ou List. " +
                            "Não recomentado uso de array ou list porque o index pode ser alterado antes da compilação.";

                            var dtype = method.Method.DeclaringType;
                            if (dtype.IsGenericType && dtype.Name.StartsWith("List"))
                            {
                                throw new Exception(msg);
                            }
                            else if (dtype.IsArray)
                            {
                                throw new Exception(msg);
                            }
                        }
                        else
                        {
                            valor = ExpressionBuilder.Evaluate(member);
                        }
                    }
                    else
                    {
                        var consta = exCall as ConstantExpression;
                        if (consta != null)
                        {
                            valor = consta.Value;
                        }
                        else
                        {
                            valor = ExpressionBuilder.Evaluate(exCall);
                        }
                    }

                    if (valor != null)
                    {
                        var t = valor.GetType();
                        if (t.IsEnum)
                        {
                            valor = (int)valor;
                        }
                    }

                    Field field = null;
                    if (map.Fields.TryGetValue(nome, out field))
                    {
                        var fg = new Field(field);
                        fg.ValueToUpdate = valor;
                        fields.Add(fg);
                    }
                }
            }
            return fields;
        }

    }
}
