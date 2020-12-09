using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity
{
    public class DataTable<T> : Entity
    {
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("data")]
        public List<T> Data { get; set; }
    }
}
