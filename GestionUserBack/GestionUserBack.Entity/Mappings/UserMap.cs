﻿using FluentNHibernate.Mapping;
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
            Map(x => x.Name);
            Map(x => x.DateCreate);
            Map(x => x.DateModify).Nullable();
            Map(x => x.Email).Unique();
        }
    }
}
