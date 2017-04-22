namespace TelegramBot.Persistence
{
    public interface IPersistanceManager
    {
        void Save(object dto);
    }
}