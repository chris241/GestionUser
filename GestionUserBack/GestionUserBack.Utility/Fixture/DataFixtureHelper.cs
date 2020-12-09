using GestionUserBack.Entity;
using GestionUserBack.Utility.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Utility.Fixture
{
    public class DataFixtureHelper : Singleton<DataFixtureHelper>
    {
        public void CreateUser()
        {
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Get<User>(Guid.Empty);
                        session.SaveOrUpdate(new User()
                        {
                            Nom = "Superman",
                            Contact="034 37 876 30",
                            Email = "Superman@gmail.com",
                            DateCreate = DateTime.Now,
                        });
                        session.SaveOrUpdate(new User()
                        {
                            Nom = "Batman",
                            Email = "Batman@gmail.com",
                            Contact = "034 37 876 31",
                            DateCreate = DateTime.Now
                        });
                        session.SaveOrUpdate(new User()
                        {
                            Nom = "IronMan",
                            Contact = "034 37 876 32",
                            Email = "IronMan@gmail.com",
                            DateCreate = DateTime.Now
                        });
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
