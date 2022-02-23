

using MicroORM.Map;
using System.Text;


namespace MicroORM.Assembler
{
    internal class InsertAssembler
    {

        internal static void Assemble(object instance, Mapped map, StringBuilder content_insert, ref Field autoincremento)
        {
            content_insert.Append("insert into ");
            content_insert.Append(map.Table.NameInDatabase);
            content_insert.Append(" ( ");
            content_insert.Append(map.HeaderAllFields);
            content_insert.Append(" ) values ( ");

            int ix = 0;
            int CNT = map.ListFields.Count - 1;
            while (ix < CNT)
            {
                var field = map.ListFields[ix];
                PrepareField(field, instance, content_insert);
                if (field.IsAutoIncrement) autoincremento = field;
                ix++;
            }

            var fieldd = map.ListFields[ix];
            PrepareField(fieldd, instance, content_insert);
            if (fieldd.IsAutoIncrement) autoincremento = fieldd;

            content_insert.Append(" )");
        }



        private static void PrepareField(Field field, object instance, StringBuilder content_insert)
        {
            var value = field.ObjectProperty.GetValue(instance);
            var formatted = field.CommandFormatter.Format(value);
            content_insert.Append(formatted);
        }

    }
}
