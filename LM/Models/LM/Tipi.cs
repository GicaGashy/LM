using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Tipi
    {
        [Required]
        public int TipiId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please select an importance rating")]
        public int Rate { get; set; }

        //relationship stuff

    }
}
