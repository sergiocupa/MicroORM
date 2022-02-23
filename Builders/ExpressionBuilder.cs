

using System.Linq.Expressions;
using System.Reflection;


namespace MicroORM.Builder
{
    internal class ExpressionBuilder
    {
        internal static object Evaluate(Expression expression)
        {
            if (expression == null) return null;

            switch (expression)
            {
                case ConstantExpression e:
                    return e.Value;

                case MemberExpression e when e.Member is PropertyInfo property:
                    return property.GetValue(Evaluate(e.Expression));

                case MemberExpression e when e.Member is FieldInfo field:
                    return field.GetValue(Evaluate(e.Expression));

                case ListInitExpression e when e.NewExpression.Arguments.Count() == 0:
                    var collection = e.NewExpression.Constructor.Invoke(new object[0]);
                    foreach (var i in e.Initializers)
                    {
                        i.AddMethod.Invoke(
                            collection,
                            i.Arguments
                                .Select(
                                    a => Evaluate(a)
                                )
                                .ToArray()
                        );
                    }
                    return collection;

                case MethodCallExpression e:
                    return e.Method.Invoke(Evaluate(e.Object),e.Arguments.Select( a => Evaluate(a)).ToArray());

                case UnaryExpression e:
                    {
                        var memb = e.Operand as MemberExpression;

                        if (memb.Member is PropertyInfo prop)
                        {
                            var val = prop.GetValue(Evaluate(memb.Expression));
                            return val;
                        }

                        if (memb.Member is FieldInfo field)
                        {
                            var val = field.GetValue(Evaluate(memb.Expression));
                            return val;
                        }

                        throw new NotSupportedException();
                    }

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
