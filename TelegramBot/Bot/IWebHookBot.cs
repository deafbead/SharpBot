using System.Threading.Tasks;
using TelegramBot.API.Models;

namespace TelegramBot.Bot
{
    public interface IWebHookBot : IBot
    {

        Task ProcessUpdate(Update update);
    }
}