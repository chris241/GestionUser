using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUserBack.Entity.Services
{
    public class GetDataTableRequest
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("pageLength")]
        public int PageLength { get; set; }
        [JsonProperty("fields")]
        public List<string> Fields { get; set; }
        [JsonProperty("filters")]
        public List<KeyValuePair<string, string>> Filters { get; set; }
        [JsonProperty("search")]
        public KeyValuePair<string, List<string>> Search { get; set; }
        [JsonProperty("isOrderByAsc")]
        public List<KeyValuePair<string, bool>> IsOrderByAsc { get; set; }

        public GetDataTableRequest()
        {
            this.PageLength = 10;
            this.Fields = new List<string>();
            this.Filters = new List<KeyValuePair<string, string>>();
            this.Search = new KeyValuePair<string, List<string>>();
            this.IsOrderByAsc = new List<KeyValuePair<string, bool>>();
        }
    }
}
