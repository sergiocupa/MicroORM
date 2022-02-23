

using MicroORM.Assembler;
using MicroORM.Database;
using MicroORM.Map;
using System.Linq.Expressions;


namespace MicroORM.Exec
{
    internal class UpdateExec
    {


        internal static List<T> Execute<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> selector)
        {
            List<T> result = new List<T>();
            if (selector == null) return result;

            var type = typeof(T);
            var map = Mapped.Map.Where(w => w.Table.MappedType.Name == type.Name).FirstOrDefault();
            if (map == null) throw new Exception("Object '" + type.Name + "' not mapped");

            using (var client = DatabaseSetting.DefaultClient.Create())
            {
                var connec = client.GetConnection();
                connec.Open();

                try
                {
                    var cmd = UpdateAssembler.Assemble<T>(where, selector, map);
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
