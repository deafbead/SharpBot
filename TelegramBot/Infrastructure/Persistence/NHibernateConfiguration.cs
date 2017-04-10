using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace TelegramBot.Persistence
{
    static class NHibernateConfiguration
    {
        public static ISessionFactory GetSessionFactory(string path, bool overwrite)
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
