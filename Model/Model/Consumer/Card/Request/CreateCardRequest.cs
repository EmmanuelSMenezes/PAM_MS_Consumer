using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model 
{ 
    public class CreateCardRequest
    {
        public Guid Consumer_id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Validity { get; set; }
        public string Document { get; set; }
        [JsonIgnore]
        public Guid Created_by { get; set; }
    }
}
