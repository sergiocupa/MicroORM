

using MicroORM.Builder;
using MicroORM.Map;
using System.Linq.Expressions;
using System.Text;


namespace MicroORM.Assembler
{
    internal class DeleteAssembler
    {
        internal static PreparedCommand Assemble<T>(Expression<Func<T, bool>> where, Mapped map)
        {
            PreparedCommand result = new PreparedCommand();
            var wherer = new WhereResult();
            WhereBuilder.Build(where, wherer);

            StringBuilder sb = new StringBuilder();
            sb.Append("delete from ");
            sb.Append(map.Table.NameInDatabase);

            if (where != null)
            {
                sb.Append(" where ");
                sb.Append(wherer.Content.ToString());
            }

            result.Content = sb.ToString();
            return result;
        }
    }
}
