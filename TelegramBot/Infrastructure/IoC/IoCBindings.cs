using System;
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
using TelegramBot.Bot.ChatProcessors;
using TelegramBot.Bot.Commands;
using TelegramBot.Bot.Commands.Attributes;
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
    public class IoCBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<WebHookBot>().ToSelf();
            Bind<ApiClient>().ToConstant(new ApiClient(ConfigurationManager.AppSettings["token"]));
            Bind<ICommandInvoker>().To<ReflectionCommandInvoker>().Named("common");

            Bind<ICommandInvoker>()
                .To<ReflectionCommandInvoker>()
                .Named("personal")
                .WithConstructorArgument(typeof(Type), typeof(PersonalCommandAttribute));

            Bind<IPollingUpdatesProvider>().To<PollingUpdatesProvider>();
            Bind<IReplySender>().To<ReplySender>();
            Bind<ILogger>().To<ConsoleLogger>();
            Bind<IWebHookBot>().To<WebHookBot>();
            Bind<IPollingBot>().To<PollingBot>();
            Bind<IThrottleFilter>().To<ThrottleFilter>();
            Bind(typeof(IRepository<>)).To(typeof(NHibernateRepository<>));
            Bind<IRecordsTable>().To<RecordsTable>();
            Bind<IQuizRanksProvider>().To<QuizRanksProvider>();
            Bind<IChatProcessor>().To<ChatProcessor>();
            
            Bind<IChatProcessorFactory>().ToConstant(ChatProcessorFactoryBuilder.Create().ConfigureChat(ConfigureChats).BuildFactory());
            Bind<IPersistanceManager>().To<PersistanceManager>();
        }

        private void ConfigureChats(ChatConfiguration cfg)
        {
            long commonChatId = CommonChat.ChatId;/*Convert.ToInt64(ConfigurationManager.AppSettings["commonChatId"]);*/

            var commonProcessor = new ChatProcessor(Kernel.Get<ICommandInvoker>("common"), Kernel.Get<IReplySender>());
            cfg.BindProcessor(commonProcessor).ToChats(commonChatId);

            var personalProcessor = new ChatProcessor(Kernel.Get<ICommandInvoker>("personal"), Kernel.Get<IReplySender>());
            cfg.BindProcessor(personalProcessor).ToEverythingExcept(commonChatId);
        }
    }
}
