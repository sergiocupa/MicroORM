

using MicroORM.Map;


namespace MicroORM.Builder
{
    internal class PreparedCommand
    {
        public string Content { get; set; }
        public List<Field> Fields { get; set; }
    }
}
