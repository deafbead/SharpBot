using System.Configuration;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Ninject.Modules;
using TelegramBot.Persistence;
using Configuration = NHibernate.Cfg.Configuration;

namespace TelegramBot.IoC
{
    public class SQLitePersistenceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactory>()
                .ToConstant(GetSessionFactory(ConfigurationManager.AppSettings["persistence"], false));
        }

        private static ISessionFactory GetSessionFactory(string path, bool overwrite)
        {
            return Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard
                        .UsingFile(path)
                )
                .Mappings(m =>
                    m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(cfg =>
                {
                    if (overwrite || !File.Exists(path))
                        BuildSchema(cfg, path);
                })
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            new SchemaExport(config).Create(false, true);
        }

    }
}
