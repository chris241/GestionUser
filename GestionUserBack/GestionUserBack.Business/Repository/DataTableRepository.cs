using GestionUserBack.Entity;
using GestionUserBack.Entity.Services;
using GestionUserBack.Utility.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Business.Repository
{
    public class DataTableRepository<T> where T : GestionUserBack.Entity.Entity
    {
        private static readonly MethodInfo OrderByMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderBy" && method.GetParameters().Length == 2);
        private static readonly MethodInfo OrderByDescendingMethod = typeof(Queryable).GetMethods().Single(method => method.Name == "OrderByDescending" && method.GetParameters().Length == 2);
        private string _dateFormat = "dd/MM/yyyy HH:mm:ss";
        public void setDateFormat(string format)
        {
            this._dateFormat = format;
        }
        public DataTable<T> GetAll(GetDataTableRequest request)
        {
            List<T> data = new List<T>();
            int total = 0;
            using (ISession session = NHibernateHelper.GetSessionFactory().OpenSession())
            {
                IQueryable<T> query = session.Query<T>();
                if (request != null)
                {
                    DynamicExpression<T> linqHelper = (DynamicExpression<T>)Helper.GetInstance(typeof(DynamicExpression<T>));
                    int skip = request.PageLength * (request.Page - 1);
                    total = query
                        .Where(linqHelper.CreateFilterStatement(request.Filters, this._dateFormat))
                        .Where(linqHelper.CreateSearchStatement(request.Search.Key, request.Search.Value, this._dateFormat))
                        .Select(x => x.Id).Count();
                    if (total > 0)
                    {
                        query = query
                        .Where(linqHelper.CreateFilterStatement(request.Filters, this._dateFormat))
                        .Where(linqHelper.CreateSearchStatement(request.Search.Key, request.Search.Value, this._dateFormat))
                        .AsQueryable();


                        foreach (KeyValuePair<string, bool> field in request.IsOrderByAsc)
                        {
                            if (field.Value)
                            {
                                query = this.OrderByProperty<T>(query, field.Key);
                            }
                            else
                            {
                                query = this.OrderByPropertyDescending<T>(query, field.Key);
                            }
                        }
                        query.Select(linqHelper.CreateNewFilteredStatement(request.Fields, this._dateFormat)).AsQueryable();

                        IEnumerable<T> lists = query
                        .Skip(skip).Take(request.PageLength);
                        data = lists.ToList<T>();
                    }
                }
                else
                {
                    total = query.Select(x => x.Id).Count();
                    if (total > 0)
                    {
                        data = query.Select(x => x).ToList<T>();

                    }
                }

            }
            return new DataTable<T>() { Total = total, Data = data };
        }



        public IQueryable<T> OrderByPropertyDescending<TEntity>(IQueryable<T> source, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return null;
            }
            ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            MethodInfo genericMethod = OrderByDescendingMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }

        public IQueryable<T> OrderByProperty<TEntity>(IQueryable<T> source, string propertyName)
        {
            if (typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
                BindingFlags.Public | BindingFlags.Instance) == null)
            {
                return null;
            }
            ParameterExpression paramterExpression = Expression.Parameter(typeof(T));
            Expression orderByProperty = Expression.Property(paramterExpression, propertyName);
            LambdaExpression lambda = Expression.Lambda(orderByProperty, paramterExpression);
            MethodInfo genericMethod =
              OrderByMethod.MakeGenericMethod(typeof(T), orderByProperty.Type);
            object ret = genericMethod.Invoke(null, new object[] { source, lambda });
            return (IQueryable<T>)ret;
        }
    }
}
