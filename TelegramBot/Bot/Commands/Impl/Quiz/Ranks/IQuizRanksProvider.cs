namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    interface IQuizRanksProvider
    {
        IQuizRank GetRank(int points);
    }
}