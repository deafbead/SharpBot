using System.Collections.Generic;

namespace TelegramBot.Persistence
{
    public interface IRepository<T>
    {
        void Add(T item);

        T Get(int id);

        IList<T> GetAll();

        void Update(T item);

        void Delete(T item);

    }
}