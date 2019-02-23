using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Team
    {
        [Required]
        public int TeamId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        //Relationship stuff

        //Relationship with Department
        [Required]
        [Range(1, int.MaxValue , ErrorMessage = "Please select a department")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        //Relationship with AppUser
        public ICollection<AppUser>AppUsers { get; set; }

        //Relationship with SoftwareTeam
        public List<SoftwareTeam>SoftwareTeams{ get; set; }
    }
}
