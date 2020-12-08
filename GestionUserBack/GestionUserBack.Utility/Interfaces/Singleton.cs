using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Utility.Interfaces
{
    public abstract class Singleton<T>
    {
        private static T _instance;

        public static T GetInstance()
        {
            if (_instance == null)
            {
                _instance = (T)Activator.CreateInstance(typeof(T));
            }
            return _instance;
        }
    }
}
