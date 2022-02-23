

using System.Data.Common;


namespace MicroORM.Util
{




    public interface IDataReaderGeneralizer
    {
        object GetValue(DbDataReader reader, int index);
    }
    internal class StringDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetString(index) : "";
            return value;
        }
    }
    internal class Int32DTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetInt32(index) : 0;
            return value;
        }
    }
    internal class Int32NullableDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (int?)reader.GetInt32(index) : null;
            return value;
        }
    }
    internal class Int16DTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetInt16(index) : 0;
            return value;
        }
    }
    internal class Int16NullableDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (short?)reader.GetInt16(index) : null;
            return value;
        }
    }
    internal class Int64DTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetInt64(index) : 0;
            return value;
        }
    }
    internal class Int64NullableDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (long?)reader.GetInt64(index) : null;
            return value;
        }
    }
    internal class DateTimeDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetDateTime(index) : new DateTime(0);
            return value;
        }
    }
    internal class DateTimeNullabelDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (DateTime?)reader.GetDateTime(index) : null;
            return value;
        }
    }
    internal class BoolDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetBoolean(index) : false;
            return value;
        }
    }
    internal class BoolNullableDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (bool?)reader.GetBoolean(index) : null;
            return value;
        }
    }
    internal class DecimalDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? reader.GetDecimal(index) : 0;
            return value;
        }
    }
    internal class DecimalNullableDTG : IDataReaderGeneralizer
    {
        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? (decimal?)reader.GetDecimal(index) : null;
            return value;
        }
    }
    internal class EnumDTG : IDataReaderGeneralizer
    {
        internal EnumDTG(Type type)
        {
            this.type = type;
        }
        Type type;

        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? Enum.ToObject(type, reader.GetInt32(index)) : Enum.ToObject(type, 0);
            return value;
        }
    }
    internal class EnumNullableDTG : IDataReaderGeneralizer
    {
        internal EnumNullableDTG(Type type)
        {
            this.type = type;
        }
        Type type;

        public object GetValue(DbDataReader reader, int index)
        {
            var value = !reader.IsDBNull(index) ? Enum.ToObject(type, reader.GetInt32(index)) : null;
            return value;
        }
    }

    internal class DataTypeGeneralizer
    {

        internal static TypeCode StractDataType(Type _type)
        {
            var underlyingType = Nullable.GetUnderlyingType(_type);
            var nullable = (underlyingType != null);
            var type = underlyingType ?? _type;

            if (type.Name == "String")
            {
                return TypeCode.String;
            }
            else if (type.Name == "Int32")
            {
                return TypeCode.Int32;
            }
            else if (type.Name == "DateTime")
            {
                return TypeCode.DateTime;
            }
            else if (type.Name == "Int64")
            {
                return TypeCode.Int64;
            }
            else if (type.Name == "Decimal")
            {
                return TypeCode.Decimal;
            }
            else if (type.Name == "Int16")
            {
                return TypeCode.Int16;
            }
            else if (type.Name == "Boolean")
            {
                return TypeCode.Boolean;
            }
            else if (type.IsEnum)
            {
                return TypeCode.Int32;
            }
            else
            {
                return TypeCode.String;
            }
        }

        internal static IDataReaderGeneralizer Create(Type _type)
        {
            var underlyingType = Nullable.GetUnderlyingType(_type);
            var nullable = (underlyingType != null);
            var type = underlyingType ?? _type;

            if (!nullable)
            {
                if (type.Name == "String")
                {
                    return new StringDTG();
                }
                else if (type.Name == "Int32")
                {
                    return new Int32DTG();
                }
                else if (type.Name == "DateTime")
                {
                    return new DateTimeDTG();
                }
                else if (type.Name == "Int64")
                {
                    return new Int64DTG();
                }
                else if (type.Name == "Decimal")
                {
                    return new DecimalDTG();
                }
                else if (type.Name == "Int16")
                {
                    return new Int16DTG();
                }
                else if (type.Name == "Boolean")
                {
                    return new BoolDTG();
                }
                else if (type.IsEnum)
                {
                    return new EnumDTG(type);
                }
                else
                {
                    return new StringDTG();
                }
            }
            else
            {
                if (type.Name == "String")
                {
                    return new StringDTG();
                }
                else if (type.Name == "Int32")
                {
                    return new Int32NullableDTG();
                }
                else if (type.Name == "DateTime")
                {
                    return new DateTimeNullabelDTG();
                }
                else if (type.Name == "Int64")
                {
                    return new Int64NullableDTG();
                }
                else if (type.Name == "Decimal")
                {
                    return new DecimalNullableDTG();
                }
                else if (type.Name == "Int16")
                {
                    return new Int16NullableDTG();
                }
                else if (type.Name == "Boolean")
                {
                    return new BoolNullableDTG();
                }
                else if (type.IsEnum)
                {
                    return new EnumNullableDTG(type);
                }
                else
                {
                    return new StringDTG();
                }
            }
        }
    }
}
