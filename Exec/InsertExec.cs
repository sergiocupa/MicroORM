

using MicroORM.Assembler;
using MicroORM.Database;
using MicroORM.Map;
using System.Text;


namespace MicroORM.Exec
{
    internal class InsertExec
    {


        internal static void Execute<T>(List<T> instances)
        {
            var type = typeof(T);
            var map = Mapped.Map.Where(w => w.Table.MappedType.Name == type.Name).FirstOrDefault();
            if (map == null) throw new Exception("Object '" + type.Name + "' not mapped");
            if (map.ListFields.Count <= 0) throw new Exception("No fields mapped to table '" + type.Name + "'");

            Field autoincremento = null;

            using (var client = DatabaseSetting.DefaultClient.Create())
            {
                var connec = client.GetConnection();
                connec.Open();

                try
                {
                    StringBuilder content_insert = new StringBuilder();

                    foreach(var instance in instances)
                    {
                        InsertAssembler.Assemble(instance, map, content_insert, ref autoincremento);
                        content_insert.Append(";");
                    }

                    var content = content_insert.ToString();
                    using (var cmd = client.CreateCommand(content))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    connec.Close();
                    throw ex;
                }
                connec.Close();
            }

        }


        internal static void Execute<T>(T instance)
        {
            var type = typeof(T);
            var map = Mapped.Map.Where(w => w.Table.MappedType.Name == type.Name).FirstOrDefault();
            if (map == null) throw new Exception("Object '" + type.Name + "' not mapped");
            if(map.ListFields.Count <= 0) throw new Exception("No fields mapped to table '" + type.Name + "'");

            Field autoincremento = null;

            using (var client = DatabaseSetting.DefaultClient.Create())
            {
                var connec = client.GetConnection();
                connec.Open();

                try
                {
                    StringBuilder content_insert = new StringBuilder();

                    InsertAssembler.Assemble(instance, map, content_insert, ref autoincremento);

                    client.GetSgbdStandards().ReturningPK_Before(content_insert, autoincremento);
                    content_insert.Append(";");

                    var content = content_insert.ToString();
                    using (var cmd = client.CreateCommand(content))
                    {
                        client.GetSgbdStandards().ReturningPK_Execute(instance, autoincremento, cmd);
                    }

                    client.GetSgbdStandards().ReturningPK_After(instance, autoincremento, client);
                }
                catch (Exception ex)
                {
                    connec.Close();
                    throw ex;
                }
                connec.Close();
            }

        }


    }
}
