using System;
using System.Collections.Generic;
using NHibernate;

namespace TelegramBot.Persistence
{
    class NHibernateRepository<TDTO> : IRepository<TDTO> where TDTO : class
    {
        private readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            _session = session;
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
                action(_session);
                transaction.Commit();
            }
        }
    }
}
