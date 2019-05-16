using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Reseller
    {
        public int ResellerId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Country { get; set; }


    }
}
