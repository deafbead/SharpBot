using TelegramBot.API;

namespace TelegramBot.Bot
{
    public interface IBot
    {
        ApiClient ApiClient { get; }
    }
}