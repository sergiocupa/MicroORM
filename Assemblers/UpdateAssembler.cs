

using MicroORM.Builder;
using MicroORM.Map;
using System.Linq.Expressions;
using System.Text;


namespace MicroORM.Assembler
{

    internal class UpdateAssembler
    {


        internal static PreparedCommand Assemble<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> selector, Mapped map)
        {
            PreparedCommand result = new PreparedCommand();
            var wherer = new WhereResult();
            WhereBuilder.Build(where, wherer);

            var fields = SelectorBuilder.BuildToUpdate(selector, map);

            StringBuilder sb = new StringBuilder();
            sb.Append("update ");
            sb.Append(map.Table.NameInDatabase);
            sb.Append(" set ");

            int ix = 0;
            int CNT = map.ListFields.Count - 1;
            while (ix < CNT)
            {
                var field = map.ListFields[ix];
                PrepareField(field, sb);
                sb.Append(",");
                ix++;
            }

            var fieldd = map.ListFields[ix];
            PrepareField(fieldd, sb);

            if (where != null)
            {
                sb.Append(" where ");
                sb.Append(wherer.Content.ToString());
            }

            result.Content = sb.ToString();
            return result;
        }


        private static void PrepareField(Field field, StringBuilder content_insert)
        {
            content_insert.Append(field.NameInDatabase);
            content_insert.Append(" = ");
            var formatted = field.CommandFormatter.Format(field.ValueToUpdate);
            content_insert.Append(formatted);
        }

    }
}
