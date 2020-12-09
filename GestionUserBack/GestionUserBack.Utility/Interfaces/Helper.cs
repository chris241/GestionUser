using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Utility.Interfaces
{
    public abstract class Helper
    {
        private static List<Helper> helpers = null;
        public static Helper GetInstance(Type concreteType)
        {
            if (helpers == null)
            {
                helpers = new List<Helper>();
            }
            Helper helper = helpers.FirstOrDefault(x =>
            {
                Type type = x.GetType();
                return (type.FullName == concreteType.FullName);
            });
            if (helper == null)
            {
                helpers.Add((Helper)Activator.CreateInstance(concreteType));
                helper = helpers.FirstOrDefault(x => x.GetType().FullName == concreteType.FullName);
            }
            return helper;
        }
    }
}

