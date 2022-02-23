

using System.Reflection;


namespace MicroORM.Map
{

    internal class AssemblyFinder
    {

        internal List<Type> Find(string search_filter)
        {
            List<Type> result = new List<Type>();

            var tps = SourceAssembly.GetTypes();
            var valid = tps.Where(g => g.GetInterface(typeof(IObjectMapper<>).Name) != null).ToList();
            if (valid.Count > 0)
            {
                result.AddRange(valid);
            }

            var assembliesg = SourceAssembly.GetReferencedAssemblies();
            var filtered    = !string.IsNullOrEmpty(search_filter) ? assembliesg.Where(g => g.Name.StartsWith(search_filter)).ToList() : assembliesg.ToList();

            foreach (var item in filtered)
            {
                var dll = Assembly.Load(item);
                var valids = dll.GetTypes().Where(g => g.GetInterface(typeof(IObjectMapper<>).Name) != null).ToList();
                if (valids.Count > 0)
                {
                    result.AddRange(valids);
                }
            }
            return result;
        }


        private Assembly SourceAssembly;

        internal AssemblyFinder(Assembly sourceAssembly)
        {
            SourceAssembly = sourceAssembly;
        }

    }
}
