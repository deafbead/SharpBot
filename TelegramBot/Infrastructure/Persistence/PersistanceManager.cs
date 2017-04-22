using NHibernate;

namespace TelegramBot.Persistence
{
    class PersistanceManager : IPersistanceManager
    {
        private readonly ISessionFactory _sessionFactory;

        public PersistanceManager(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Save(object dto)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(dto);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }

            }
        }
    }
}
