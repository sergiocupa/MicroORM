

using MicroORM.Assembler;
using MicroORM.Builder;
using MicroORM.Database;
using MicroORM.Map;
using System.Data.Common;
using System.Linq.Expressions;


namespace MicroORM.Exec
{
    internal class SelectExec
    {



        internal static List<T> Execute<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> selector)
        {
            List<T> result = new List<T>();

            var type = typeof(T);
            var map = Mapped.Map.Where(w => w.Table.MappedType.Name == type.Name).FirstOrDefault();
            if (map == null) throw new Exception("Object '" + type.Name + "' not mapped");

            using (var client = DatabaseSetting.DefaultClient.Create())
            {
                var connec = client.GetConnection();
                connec.Open();

                try
                {
                    var cmd = SelectAssembler.Assemble(where, selector, map);
                    var rc = client.CreateCommand(cmd.Content);
                    var reader = rc.ExecuteReader();
                    while (reader.Read())
                    {
                        var instance = Read<T>(reader, cmd);
                        result.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    connec.Close();
                    throw ex;
                }
                connec.Close();
            }

            return result;
        }


        private static T Read<T>(DbDataReader reader, PreparedCommand cmd)
        {
            T instance = Activator.CreateInstance<T>();

            foreach (var field in cmd.Fields)
            {
                try
                {
                    var value = field.DataReaderType.GetValue(reader, field.ReaderIndex);
                    if (field.ObjectProperty != null)
                    {
                        field.ObjectProperty.SetValue(instance, value);
                    }
                    else
                    {
                        // TO-DO: ...
                    }
                }
                catch (Exception ex)
                {
                    // Log ...
                    throw ex;
                }
            }

            return instance;
        }


        


    }
}
