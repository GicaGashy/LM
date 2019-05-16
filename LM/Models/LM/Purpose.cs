using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Purpose
    {
        [Required]
        public int PurposeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
