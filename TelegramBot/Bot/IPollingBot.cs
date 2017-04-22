using System.Threading.Tasks;

namespace TelegramBot.Bot
{
    public interface IPollingBot : IBot
    {
        bool IsRunning { get; }

        Task Start();
        void Stop();
    }
}