

namespace MicroORM.Database
{
    public abstract class ADatabaseStartupper : IDatabaseStartupper
    {
        public void Startup(string database_name)
        {
            BeforeStartup();

            CreateDatabase(database_name);

        }


        
        private void CreateDatabase(string database_name)
        {
            if(!DatabaseExists(database_name))
            {
                Execute<bool>("create database " + database_name);
            }
        }
        private bool DatabaseExists(string database_name)
        {
            var ext = Execute<bool>("select true from pg_database where datname='" + database_name + "'");
            return ext;
        }

        private T Execute<T>(string cmd)
        {
            T result = default(T);
            using (var client = DatabaseSetting.DefaultClient.Create())
            {
                var connec = client.GetConnection();
                connec.Open();

                try
                {
                    var rc = client.CreateCommand(cmd);
                    var reader = rc.ExecuteScalar();
                    result = (T)reader;
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


        protected abstract void BeforeStartup();


    }
}
