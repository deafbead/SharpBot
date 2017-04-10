namespace TelegramBot.Bot.Commands.Quiz.Ranks
{
    interface IQuizRank
    {
        string Name { get; }
        int PointsRequired { get; }

        IQuizRank NextRank { get; }
    }
}
