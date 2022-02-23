

using MicroORM.Util;
using System.Reflection;


namespace MicroORM.Map
{

   
    public class Mapped
    {

        internal Table Table { get; set; }
        internal Dictionary<string, Field> Fields { get; set; } // KEY => NomePropriedade
        internal List<Field> ListFields { get; set; } 
        internal eMappedObjectStatus PreparationStatus { get; set; }
        internal static List<Mapped> Map { get; set; }
        internal static bool IsMapped { get; set; }
        public string BasicInsertCommand { get; set; }
        internal string HeaderAllFields { get; set; }

        static Mapped()
        {
            Map = new List<Mapped>();
            IsMapped = false;
        }
    }

    public class Table
    {
        public string NameInDatabase { get; set; }
        public Type MappedType { get; set; }
    }

    public class Field
    {
        public Mapped Map { get; set; }
        public Table Table { get; set; }
        public IDataReaderGeneralizer DataReaderType { get; set; }
        public IDataTypeConverter TypeConverter { get; set; }
        public IDataCommandFormatter CommandFormatter { get; set; }
        public TypeCode TypeInDatabase { get; set; }
        public int ReaderIndex { get; set; }
        public string NameInDatabase { get; set; }
        public int DecimalPlaces { get; set; }
        public int Length { get; set; }
        public bool IsNotNull { get; set; }
        internal bool IsPK { get; set; }
        public bool IsAutoIncrement { get; set; }
        public int OrdinalInDatabase { get; set; }
        public string DefaultValue { get; set; }
        public PropertyInfo ObjectProperty { get; set; }
        public Type PropertyType { get; set; }
        public object ValueToUpdate { get; set; }


        public Field(Field instance)
        {
            if(instance != null)
            {
                DataReaderType    = instance.DataReaderType;
                DecimalPlaces     = instance.DecimalPlaces;
                DefaultValue      = instance.DefaultValue;
                IsAutoIncrement   = instance.IsAutoIncrement;
                IsNotNull         = instance.IsNotNull;
                IsPK              = instance.IsPK;
                Length            = instance.Length;
                NameInDatabase    = instance.NameInDatabase;
                ObjectProperty    = instance.ObjectProperty;
                OrdinalInDatabase = instance.OrdinalInDatabase;
                PropertyType      = instance.PropertyType;
                ReaderIndex       = instance.ReaderIndex;
                TypeInDatabase    = instance.TypeInDatabase;
            }
        }

        public Field()
        {

        }

    }

    public enum eMappedObjectStatus
    {
        Unknown                         = 0,
        Prepared                        = 1,
        IgnoreByFkReferenceNotYetMapped = 2
    }
}
