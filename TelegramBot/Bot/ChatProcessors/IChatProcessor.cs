using System.Threading.Tasks;
using TelegramBot.API.Models;

namespace TelegramBot.Bot.ChatProcessors
{
    public interface IChatProcessor
    {
        Task ProcessUpdate(Update update);
    }
}