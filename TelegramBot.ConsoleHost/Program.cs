using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using TelegramBot.Bot;
using TelegramBot.IoC;

namespace TelegramBot.ConsoleHost
{
    class Program
    {
        private static readonly IKernel Kernel = CreateKernel();

        private static IKernel CreateKernel()
        {
            return new StandardKernel(new SQLitePersistenceModule(), new IoCBindings());
        }

        static void Main(string[] args)
        {
            Kernel.Get<IPollingBot>().Start().Wait();
        }
    }
}
