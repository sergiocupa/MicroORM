

using System.Reflection;


namespace MicroORM.Map
{



    public class Map<T>
    {

        public FieldObjectPropertyMap<T> TableName(string name)
        {
            Root = new ElementMap();
            Root.TableName = name;
            Root.ObjectType = typeof(T);
            return (FieldObjectPropertyMap<T>)Root;
        }

        internal ElementMap Root;

    }


    public class FieldObjectPropertyMap<T> : ElementMap
    {

        public FieldNameDatabaseMap ObjectField(Func<T, object> keySelector)
        {
            if (keySelector != null && keySelector.Method != null)
            {
                var props = ObjectType.GetProperties();

                var exist = Fields.Where(w => w.ObjectProperty.Name == keySelector.Method.Name).FirstOrDefault();
                if(exist == null)
                {
                    SelectedField = new ElementMap();
                    SelectedField.ObjectProperty = props.Where(w => w.Name == keySelector.Method.Name).FirstOrDefault();
                    SelectedField.FieldType = ObjectProperty.PropertyType;
                    Fields.Add(SelectedField);
                }
            }
            return (FieldNameDatabaseMap)((ElementMap)this);
        }

    }


    public class FieldNameDatabaseMap : ElementMap
    {
        public FieldDatabasePropertyMap DataField(string name)
        {
            SelectedField.DataFieldName = name;
            return (FieldDatabasePropertyMap)((ElementMap)this);
        }
    }

    public class FieldDatabasePropertyMap : ElementMap
    {

        public FieldDatabasePropertyMap DecimalLength(int leng)
        {
            SelectedField.DecimalLeng = leng;
            return this;
        }

        public FieldDatabasePropertyMap DataLength(int leng)
        {
            SelectedField.FieldDataLength = leng;
            return this;
        }

        public FieldDatabasePropertyMap IsNotNull()
        {
            SelectedField.NotNull = true;
            return this;
        }

        public FieldDatabasePropertyMap IsPrimaryKey()
        {
            SelectedField.PrimaryKey = true;
            return this;
        }

        public FieldDatabasePropertyMap IsAutoIncrement()
        {
            SelectedField.AutoIncrement = true;
            return this;
        }

        public ElementMap DefaultValue(string value)
        {
            SelectedField.Default = value;
            return this;
        }

    }


    public class ElementMap
    {
        internal string TableName { get; set; }
        internal string DataFieldName { get; set; }
        internal int FieldDataLength { get; set; }
        internal int DecimalLeng { get; set; }
        internal bool NotNull { get; set; }
        internal bool PrimaryKey { get; set; }
        internal bool AutoIncrement { get; set; }
        internal string Default { get; set; }
        internal Type FieldType { get; set; }
        internal Type ObjectType { get; set; }
        internal PropertyInfo ObjectProperty { get; set; }
        internal List<ElementMap> Fields { get; set; }
        internal ElementMap SelectedField { get; set; }

        internal ElementMap()
        {
            Fields = new List<ElementMap>();
        }
    }

}
