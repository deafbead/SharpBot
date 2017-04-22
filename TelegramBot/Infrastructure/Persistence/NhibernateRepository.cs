using System;
using System.Collections.Generic;
using NHibernate;

namespace TelegramBot.Persistence
{
    class NHibernateRepository<TDTO> : IRepository<TDTO>, IDisposable where TDTO : class
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _session = _sessionFactory.OpenSession();
        }

        public void Add(TDTO item)
        {
            InTransaction(s => s.Save(item));
        }

        public TDTO Get(int id)
        {

            return _session.Get<TDTO>(id);

        }

        public IList<TDTO> GetAll()
        {
            return _session.QueryOver<TDTO>().List();
        }

        public void Update(TDTO item)
        {
            InTransaction(s => s.Update(item));
        }

        public void Delete(TDTO item)
        {

            _session.Delete(item);

        }

        private void InTransaction(Action<ISession> action)
        {
            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    action(_session);
                    transaction.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }

            }
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
