﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSCTask6.Models
{
    public class CustomerModel
    {

        [Required]
        public string Name { get; set; }
        public string Email { get; set; }


    }
}