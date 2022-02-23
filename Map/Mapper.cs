

using MicroORM.Util;
using System.Reflection;
using System.Text;


namespace MicroORM.Map
{


    internal class MapBuilder
    {



        internal void Build()
        {
            AssemblyFinder af = new AssemblyFinder(SourceAssembly);
            Types = af.Find("");

            if (!Mapped.IsMapped && Types.Count > 0)
            {
                Mappeds.Clear();

                var classes = new List<Type>(Types);
                var map_type = typeof(Map<>);

                int i = classes.Count;
                while (classes.Count > 0)
                {
                    i--;
                    if (i < 0) i = classes.Count - 1;
                    var iMap = classes[i];

                    var existe = Mapped.Map.Where(w => w.Table.MappedType.Name == iMap.Name).FirstOrDefault();
                    if (existe != null)
                    {
                        classes.RemoveAt(i);
                        continue;
                    }

                    var instancia = Activator.CreateInstance(iMap);
                    var metodo = iMap.GetMethod("Create");
                    if (metodo != null)
                    {
                        try
                        {
                            var generict     = iMap.GetGenericArguments()[0];
                            var instype      = map_type.MakeGenericType(generict);
                            var map_instance = Activator.CreateInstance(instype);

                            metodo.Invoke(instancia, new object[] { map_instance });

                            var field = instype.GetField("Root");
                            var root = (ElementMap)field.GetValue(map_instance);

                            if (root != null)
                            {
                                var prep = MapPreparer.Prepare(root, Mapped.Map);

                                if (prep.PreparationStatus == eMappedObjectStatus.IgnoreByFkReferenceNotYetMapped)
                                {
                                    continue;
                                }

                                var map = new Mapped();
                                map.Table = prep.Table;
                                map.Fields = prep.Fields;

                                Mapped.Map.Add(map);
                                Mappeds.Add(prep);
                                classes.RemoveAt(i);
                            }
                            else throw new Exception("Não foi possível gerar mapa para a classe '" + iMap.Name + "'");
                        }
                        catch (Exception ex)
                        {
                            var n = new Exception("Não foi possível executar mapeamento do tipo " + iMap.Name, ex);
                            throw n;
                        }
                    }
                    else throw new Exception("Metodo 'Mapear' não foi encontrado");
                }
                PrepareBasicQuery();

                Mapped.IsMapped = true;
            }
        }


        internal static void PrepareBasicQuery()
        {
            if (Mapped.Map.Count > 0)
            {
                foreach (var tabela in Mapped.Map)
                {
                    if (tabela.Fields.Count > 0)
                    {
                        var fieldl = tabela.Fields.Select(s => s.Value).ToList();

                        StringBuilder Query = new StringBuilder();
                        Query.Append("insert into ");
                        Query.Append(tabela.Table.NameInDatabase);
                        Query.Append(" (");
                        StringBuilder sNome = new StringBuilder();
                        StringBuilder sVari = new StringBuilder();

                        int CNT = fieldl.Count;
                        int i = 0;
                        while (i < CNT)
                        {
                            if (!fieldl[i].IsAutoIncrement)
                            {
                                sNome.Append(fieldl[i].NameInDatabase);
                                sNome.Append(",");
                                sVari.Append("@");
                                sVari.Append(fieldl[i].NameInDatabase);
                                sVari.Append(",");
                            }
                            i++;
                        }

                        var ss = sNome.ToString();
                        var vv = sVari.ToString();

                        if (ss.Length > 0 && ss[ss.Length - 1] == ',')
                        {
                            ss = ss.Substring(0, ss.Length - 1);
                        }
                        if (vv.Length > 0 && vv[vv.Length - 1] == ',')
                        {
                            vv = vv.Substring(0, vv.Length - 1);
                        }

                        Query.Append(ss);
                        Query.Append(") values (");
                        Query.Append(vv);
                        Query.Append(") ");
                        tabela.BasicInsertCommand = Query.ToString();
                        tabela.HeaderAllFields    = string.Join(",", fieldl.Select(s => s.NameInDatabase));
                    }
                }
            }
        }




        public MapBuilder(Assembly source)
        {
            SourceAssembly = source;

            Types    = new List<Type>();
            Mappeds  = new List<Mapped>();
        }

        private Assembly SourceAssembly;
        //private static Type ObjectMapperType;
        private List<Type> Types;
        internal List<Mapped> Mappeds { get; set; }


        static MapBuilder()
        {
            //ObjectMapperType = typeof(IObjectMapper<>);
        }


    }



    internal class MapPreparer
    {

        internal static Mapped Prepare(ElementMap created_map, List<Mapped> maps)
        {
            var tabela = new Mapped();
            ElementMap _item = null;
            int index = 0;
            try
            {
                if (created_map.TableName == null) throw new Exception("Não foi definido tabela");

                tabela.Table = new Table();
                tabela.Table.NameInDatabase = created_map.TableName;
                tabela.Fields = new Dictionary<string, Field>();

                if (created_map.Fields.Count > 0)
                {
                    foreach (var item in created_map.Fields)
                    {
                        _item = item;
                        Field camp = new Field();

                        camp.Table            = tabela.Table;
                        camp.Map              = tabela;
                        camp.ObjectProperty   = item.ObjectProperty;
                        camp.PropertyType     = item.ObjectProperty.PropertyType;
                        camp.DataReaderType   = DataTypeGeneralizer.Create(item.ObjectProperty.PropertyType);
                        camp.TypeInDatabase   = DataTypeGeneralizer.StractDataType(item.ObjectProperty.PropertyType);
                        camp.TypeConverter    = DataTypeConverter.Create(item.ObjectProperty.PropertyType);
                        camp.CommandFormatter = DataCommandFormatter.Create(item.ObjectProperty.PropertyType);
                        camp.NameInDatabase   = item.DataFieldName;
                        camp.DecimalPlaces    = item.DecimalLeng;
                        camp.DefaultValue     = item.Default;
                        camp.IsAutoIncrement  = item.AutoIncrement;
                        camp.IsNotNull        = item.NotNull;
                        camp.IsPK             = item.PrimaryKey;
                        camp.Length           = item.FieldDataLength;

                        Field f = null;
                        if(tabela.Fields.TryGetValue(camp.NameInDatabase,out f))
                        {
                            camp.ReaderIndex = index;
                            tabela.Fields.Add(camp.NameInDatabase,camp);
                            tabela.ListFields.Add(camp);
                            index++;
                        }
                        else
                        {
                            string msg = "Mapeamento redundante da propriedade '" + camp.NameInDatabase + "' " +
                                         "do objeto '" + tabela.Table.MappedType.Name + "'";
                            throw new Exception(msg);
                        }
                    }

                }
                tabela.PreparationStatus = eMappedObjectStatus.Prepared;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tabela;
        }
    }

}
