

namespace MicroORM.Util
{
    public interface IDataCommandFormatter
    {
        string Format(object data);
    }


    public class StringDCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return data != null ? "'"+data+"'" : "null";
        }
    }



    internal class Int16DCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? data.ToString() : "null");
        }
    }
    internal class Int32DCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? data.ToString() : "null");
        }
    }
    internal class Int64DCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? data.ToString() : "null");
        }
    }
    internal class BoolDCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? data.ToString() : "null");
        }
    }
    internal class DecimalDCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? data.ToString() : "null");
        }
    }
    internal class DatetimeDCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? "'" + data + "'" : "null");
        }
    }
    internal class EnumDCF : IDataCommandFormatter
    {
        public string Format(object data)
        {
            return (data != null ? ((int)data).ToString() : "null");
        }
    }






    public class DataCommandFormatter
    {


        public static IDataCommandFormatter Create(Type _type)
        {
            var underlyingType = Nullable.GetUnderlyingType(_type);
            var nullable = (underlyingType != null);
            var type = underlyingType ?? _type;

            if (type.Name == "String")
            {
                return new StringDCF();
            }
            else if (type.Name == "Int32")
            {
                return new Int32DCF();
            }
            else if (type.Name == "DateTime")
            {
                return new DatetimeDCF();
            }
            else if (type.Name == "Int64")
            {
                return new Int64DCF();
            }
            else if (type.Name == "Decimal")
            {
                return new DecimalDCF();
            }
            else if (type.Name == "Int16")
            {
                return new Int16DCF();
            }
            else if (type.Name == "Boolean")
            {
                return new BoolDCF();
            }
            else if (type.IsEnum)
            {
                return new EnumDCF();
            }
            else
            {
                return new StringDCF();
            }
        }

    }

}
