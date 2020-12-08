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
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("dateCreate")]
        public virtual DateTime DateCreate { get; set; }

        [JsonProperty("dateModify")]
        public virtual DateTime DateModify { get; set; }

        [JsonProperty("email")]
        public virtual string Email { get; set; }
    }
}
