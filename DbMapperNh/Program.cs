using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var types = new List<Type>() { typeof(Color3Map) };
            var mapper = new ModelMapper();
            mapper.AddMappings(types);
            var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = "host=localhost;database=sandbox;user id=postgres;password=Asztal15";
                db.Driver<NpgsqlDriver>();
                db.Dialect<PostgreSQLDialect>();
                db.LogFormattedSql = true;
                db.LogSqlInConsole = true;
            });
            configuration.SetNamingStrategy(DbNamingStrategy.Instance); // add before mappings!

            // add xml configs: configuration.AddAssembly(Assembly.GetExecutingAssembly());
            configuration.AddDeserializedMapping(mapping, null);
            //SchemaMetadataUpdater.QuoteTableAndColumns(configuration); //configuration.SetProperty("hbm2ddl.keywords", "auto-quote");

            var sefact = configuration.BuildSessionFactory();

            using (var session = sefact.OpenSession())
            {
                //var colors = session.Query<Color3>().Where(r => r.Id > 0).ToList();


                //var newCol = new Color3 { Name = "new1" };
                //session.Save(newCol);
            }

            using (var session = sefact.OpenSession())
            {
                var newCol = new Color3 { Name = "new1" };
                var ret = session.Save(newCol);
                Console.WriteLine(ret.ToString());
            }

            using (var session = sefact.OpenStatelessSession())
            {
                var newCol = new Color3 { Name = "new1" };
                var ret = session.Insert(newCol);
                Console.WriteLine(ret.ToString());
            }

            Console.ReadLine();
        }
    }

    public class Color3Map : ClassMapping<Color3>
    {
        public Color3Map()
        {
            Schema("public");
            Lazy(false);
            //Id(x => x.Id, map => { map.Generator(Generators.Identity, p => p.Params(new { sequence = "`Color3_Id_seq`" })); map.Column("`Id`"); });
            Id(x => x.Id, map =>
            {
                map.Generator(Generators.TriggerIdentity, p => p.Params(new
                {
                    sequence = "Color3_Id_seq"
                }
                ));
            });
            Property(x => x.Name, map => { map.NotNullable(true); });
            Property(x => x.Description);
        }
    }

    //public class Color3Map : ClassMapping<Color3>
    //{
    //    public Color3Map()
    //    {
    //        Table("`Color3`");
    //        Schema("public");
    //        Lazy(false);
    //        //Id(x => x.Id, map => { map.Generator(Generators.Identity, p => p.Params(new { sequence = "`Color3_Id_seq`" })); map.Column("`Id`"); });
    //        Id(x => x.Id, map => { map.Generator(Generators.TriggerIdentity, p => p.Params(new { sequence = "`Color3_Id_seq`" })); map.Column("`Id`"); });
    //        Property(x => x.Name, map => { map.Column("`Name`"); map.NotNullable(true); });
    //        Property(x => x.Description, map => { map.Column("`Description`"); });
    //    }
    //}

    public class Color3
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class DbNamingStrategy : INamingStrategy
    {
        private static readonly INamingStrategy ImprovedNamingStrategy = NHibernate.Cfg.ImprovedNamingStrategy.Instance;

        private static DbNamingStrategy _namingStrategy;
        public static INamingStrategy Instance => _namingStrategy ?? (_namingStrategy = new DbNamingStrategy());

        protected DbNamingStrategy()
        {
        }

        public string ClassToTableName(string className)
        {
            return $"\"{className}\"";
        }

        public string ColumnName(string columnName)
        {
            return $"\"{columnName}\"";
        }

        public string LogicalColumnName(string columnName, string propertyName)
        {
            return ImprovedNamingStrategy.LogicalColumnName(columnName, propertyName);
        }

        public string PropertyToColumnName(string propertyName)
        {
            return ImprovedNamingStrategy.PropertyToColumnName(propertyName);
        }

        public string PropertyToTableName(string className, string propertyName)
        {
            return ImprovedNamingStrategy.PropertyToTableName(className, propertyName);
        }

        public string TableName(string tableName)
        {
            return ImprovedNamingStrategy.TableName(tableName);
        }
    }
}
