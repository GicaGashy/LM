using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Department
    {
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        //Relationship stuff

        //Relatinship with team
        public ICollection<Team> Teams{ get; set; }
    }
}
