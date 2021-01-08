using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CSCTask6.Models
{
    public class FirebaseInput
    {
        [Required]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}