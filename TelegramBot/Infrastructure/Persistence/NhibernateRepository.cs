using System;
using System.Collections.Generic;
using NHibernate;

namespace TelegramBot.Persistence
{
    class NHibernateRepository<TDTO> : IRepository<TDTO> where TDTO : class
    {
        private readonly ISessionFactory _sessionFactory;

        public NHibernateRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Add(TDTO item)
        {
            InTransaction(s => s.Save(item));
        }

        public TDTO Get(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<TDTO>(id);
            }
        }

        public IList<TDTO> GetAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<TDTO>().List();
            }
        }

        public void Update(TDTO item)
        {
            InTransaction(s => s.Update(item));
        }

        public void Delete(TDTO item)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Delete(item);
            }
        }

        private void InTransaction(Action<ISession> action)
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    action(session);
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
