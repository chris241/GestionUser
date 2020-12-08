using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity.Mappings
{

    public class EntityMap : ClassMap<Entity>
    {
        public EntityMap()
        {
            Id(x => x.Id).GeneratedBy.Guid();
            UseUnionSubclassForInheritanceMapping();
        }
    }
}
