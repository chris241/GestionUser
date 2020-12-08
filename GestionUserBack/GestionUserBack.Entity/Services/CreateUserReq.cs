using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity.Services
{
   public  class CreateUserReq
    {
        [JsonProperty("id")]
        public virtual Guid Id { get; set; }
        [JsonProperty("nom")]
        public virtual string Nom { get; set; }

        [JsonProperty("email")]
        public virtual string Email { get; set; }
        [JsonProperty("contact")]
        public virtual int Contact { get; set; }
    }
}
