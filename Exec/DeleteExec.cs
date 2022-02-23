

using MicroORM.Assembler;
using MicroORM.Database;
using MicroORM.Map;
using System.Linq.Expressions;


namespace MicroORM.Exec
{
    internal class DeleteExec
    {

        internal static List<T> Execute<T>(Expression<Func<T, bool>> where)
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
                    var cmd = DeleteAssembler.Assemble(where, map);
                    var rc = client.CreateCommand(cmd.Content);
                    var reader = rc.ExecuteNonQuery();
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

        


    }
}
