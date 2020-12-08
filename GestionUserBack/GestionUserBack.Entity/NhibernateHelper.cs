using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity
{
    public class NHibernateHelper
    {
        private static ISessionFactory SessionFactory = null;
        public static ISessionFactory GetSessionFactory()
        {
            if (SessionFactory == null)
            {
                var cfg = MySQLConfiguration.Standard
                    .ConnectionString(c =>
                        c.Is(
                        "Server=localhost; Port=3306; Database=gestion_user; Uid=root; Pwd=;default command timeout=20000"));
                SessionFactory = Fluently.Configure()
                       .Database(cfg)
                             .Mappings(m =>
                               m.FluentMappings.AddFromAssemblyOf<User>()
                             )
               .ExposeConfiguration(BuildSchema)
                .ExposeConfiguration(x =>
                {
                    x.SetInterceptor(new HelperSqlStatementInterceptor());
                })
                .BuildSessionFactory();
            }
            return SessionFactory;
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            new SchemaUpdate(config)
            .Execute(true, true);
        }
        public class HelperSqlStatementInterceptor : EmptyInterceptor
        {
            public override SqlString OnPrepareStatement(SqlString sql)
            {
                Trace.WriteLine(sql.ToString());
                return sql;
            }
        }
    }
}
