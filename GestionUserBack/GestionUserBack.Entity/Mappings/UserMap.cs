using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity.Mappings
{
    public class UserMap : SubclassMap<User>
    {
        public UserMap()
        {
            Abstract();
            Map(x => x.Nom).Not.Nullable();
            Map(x => x.DateCreate).Not.Nullable();
            Map(x => x.Contact).Not.Nullable();
            Map(x => x.DateModify).Nullable();
            Map(x => x.Email).Unique().Not.Nullable();
        }
    }
}
