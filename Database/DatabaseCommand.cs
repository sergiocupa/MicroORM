

using MicroORM.Exec;
using System.Linq.Expressions;


namespace MicroORM.Database
{
    public class DatabaseCommand<T> : IDatabaseCommand<T>
    {



        public void Delete(Expression<Func<T, bool>> where)
        {
            DeleteExec.Execute(where);
        }

        public void Insert(T instance)
        {
            InsertExec.Execute(instance);
        }

        public List<T> Select(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> selector = null)
        {
            var result = SelectExec.Execute(where, selector);
            return result;
        }

        public void Update(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> selector = null)
        {
            UpdateExec.Execute(where, selector);
        }
    }


    public interface IDatabaseCommand<T>
    {

        List<T> Select(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> selector = null);

        void Insert(T instance);

        void Update(Expression<Func<T, bool>> where = null, Expression<Func<T, object>> selector = null);

        void Delete(Expression<Func<T, bool>> where);



    }
}
