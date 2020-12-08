using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity
{
    public abstract class Entity
    {
        [JsonProperty("id")]
        public virtual Guid Id { get; set; }
    }
}
