

using MicroORM.Builder;
using MicroORM.Map;
using System.Linq.Expressions;
using System.Text;


namespace MicroORM.Assembler
{
    internal class SelectAssembler
    {



        internal static PreparedCommand Assemble<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> selector, Mapped map)
        {
            PreparedCommand result = new PreparedCommand();
            var header = Build(selector, map);
            var wherer = new WhereResult();
            WhereBuilder.Build(where, wherer);

            StringBuilder sb = new StringBuilder();
            sb.Append("select ");
            sb.Append(header.Content);
            sb.Append(" from ");
            sb.Append(map.Table.NameInDatabase);

            if (where != null)
            {
                sb.Append(" where ");
                sb.Append(wherer.Content.ToString());
            }

            result.Content = sb.ToString();
            result.Fields = header.Fields;
            return result;
        }


        private static PreparedCommand Build<T>(Expression<Func<T, object>> selector, Mapped map)
        {
            PreparedCommand result = new PreparedCommand();
            if (selector != null)
            {
                result.Fields = SelectorBuilder.BuildToSelect(selector, map);
                result.Content = string.Join(",", result.Fields.Select(s => s.NameInDatabase));
                return result;
            }
            else
            {
                result.Fields = map.Fields.Select(s => s.Value).ToList();
                result.Content = map.HeaderAllFields;
                return result;
            }
        }

    }
}
