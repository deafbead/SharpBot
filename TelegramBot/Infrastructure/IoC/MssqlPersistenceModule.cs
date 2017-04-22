using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Ninject.Modules;
using TelegramBot.Bot;

namespace TelegramBot.Infrastructure.IoC
{
    public class MSSQLPersistenceModule : NinjectModule
    {

        public static ISessionFactory CreateSessionFactory()
        {
            string connectionString = ConfigurationManager.AppSettings["persistence"];

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<IBot>())
                //.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true)) //uncomment on first use
                .BuildSessionFactory();
        }
        
        public override void Load()
        {
            Bind<ISessionFactory>().ToConstant(CreateSessionFactory());
        }
    }
}
