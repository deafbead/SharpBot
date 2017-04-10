﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using Ninject;
using Ninject.Modules;
using TelegramBot.API;
using TelegramBot.Bot;
using TelegramBot.Bot.Commands;
using TelegramBot.Bot.Commands.Quiz;
using TelegramBot.Bot.Commands.Quiz.Ranks;
using TelegramBot.Bot.Games.Score;
using TelegramBot.Bot.Replies;
using TelegramBot.Bot.Updates;
using TelegramBot.Logging;
using TelegramBot.Persistence;
using TelegramBot.Util;

namespace TelegramBot.IoC
{
    class IoCBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<BotImpl>().ToSelf();
            Bind<ApiClient>().ToConstant(new ApiClient(ConfigurationManager.AppSettings["token"]));
            Bind<ICommandInvoker>().To<CommandInvoker>();
            Bind<IUpdatesProvider>().To<UpdatesProvider>();
            Bind<IReplySender>().To<ReplySender>();
            Bind<ILogger>().To<ConsoleLogger>();
            Bind<IBot>().To<BotImpl>();
            Bind<IThrottleFilter>().To<ThrottleFilter>();
            Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));
            Bind<IRecordsTable>().To<RecordsTable>();
            Bind<IQuizRanksProvider>().To<QuizRanksProvider>();
            Bind<ISession>()
                .ToConstant(
                    NHibernateConfiguration.GetSessionFactory(ConfigurationManager.AppSettings["persistence"], false).OpenSession());
        }
    }
}
