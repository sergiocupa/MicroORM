

using MicroORM.Map;
using System.Data.Common;
using System.Text;


namespace MicroORM.Database
{





    public interface IDatabaseClient : IDisposable
    {


        IDatabaseClient Create();
        DbCommand CreateCommand(string cmd);
        //void AddParameter(string Name, object Value);
        public ISgbdStandards GetSgbdStandards();
        public DbConnection GetConnection();


    }


    public interface ISgbdStandards
    {
        void Function_Length(StringBuilder string_builder);

        void ReturningPK_Before(StringBuilder content_insert, Field field);
        void ReturningPK_Execute(object target, Field field, DbCommand command);
        void ReturningPK_After(object target, Field field, IDatabaseClient client);
    }


    public interface IDatabaseStartupper
    {

        void Startup(string database_name);

    }
}
