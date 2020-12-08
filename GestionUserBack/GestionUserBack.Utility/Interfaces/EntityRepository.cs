using GestionUserBack.Entity;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Utility.Interfaces
{
    public class EntityRepository<T> where T : GestionUserBack.Entity.Entity
    {
        public async Task<T> FindById(Guid id)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                return await session.GetAsync<T>(id);
            }
        }

        public async Task<List<T>> FindByAll(int skip = 0, int take = -1)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                IQueryable<T> query = session.Query<T>();
                if (skip > 0)
                {
                    query = query.Skip(skip);
                }
                if (take > -1)
                {
                    query = query.Take(take);
                }
                return await query.ToListAsync<T>();
            }
        }


        public void SaveOrUpdate(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }
        public async Task SaveOrUpdateAsync(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        await session.SaveOrUpdateAsync(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }


        public async Task Delete(T entity)
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        await session.DeleteAsync(entity);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }

    }
}
