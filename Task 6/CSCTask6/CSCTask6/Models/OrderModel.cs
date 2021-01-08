using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSCTask6.Models
{
    public class OrderModel
    {
        [Required]
        public string OrderReq { get; set; }
        public string Price { get; set; }
    }
}