


namespace MicroORM.Util
{


    internal class DataTypeConverter
    {
        internal static IDataTypeConverter Create(Type _type)
        {
            var underlyingType = Nullable.GetUnderlyingType(_type);
            var nullable = (underlyingType != null);
            var type = underlyingType ?? _type;

            if (type.Name == "String")
            {
                return new StringDTC();
            }
            else if (type.Name == "Int32")
            {
                return new Int32DTC();
            }
            else if (type.Name == "DateTime")
            {
                return new DatetimeDTC();
            }
            else if (type.Name == "Int64")
            {
                return new Int64DTC();
            }
            else if (type.Name == "Decimal")
            {
                return new DecimalDTC();
            }
            else if (type.Name == "Int16")
            {
                return new Int16DTC();
            }
            else if (type.Name == "Boolean")
            {
                return new BoolDTC();
            }
            else if (type.IsEnum)
            {
                return new EnumDTC(type);
            }
            else
            {
                return new StringDTC();
            }
        }
    }


    internal class StringDTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? data_object.ToString() : "";
        }
    }

    internal class Int16DTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? short.Parse(data_object.ToString()) : (short)0;
        }
    }
    internal class Int32DTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? int.Parse(data_object.ToString()) : 0;
        }
    }
    internal class Int64DTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? long.Parse(data_object.ToString()) : (long)0;
        }
    }
    internal class BoolDTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? bool.Parse(data_object.ToString()) : false;
        }
    }
    internal class DecimalDTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? decimal.Parse(data_object.ToString()) : (decimal)0;
        }
    }
    internal class DatetimeDTC : IDataTypeConverter
    {
        public object Convert(object data_object)
        {
            return data_object != null ? DateTime.Parse(data_object.ToString()) : (short)0;
        }
    }
    internal class EnumDTC : IDataTypeConverter
    {

        internal EnumDTC(Type type)
        {
            this.type = type;
        }
        Type type;


        public object Convert(object data_object)
        {
            var va     = data_object != null ? int.Parse(data_object.ToString()) : 0;
            var result = Enum.ToObject(type, va);
            return result;
        }
    }



    public interface IDataTypeConverter
    {
        object Convert(object data_object);
    }
}
