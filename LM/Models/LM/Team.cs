using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }

        //Relationship stuff

        //Relationship with Department
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        //Relationship with AppUser
        public ICollection<AppUser>AppUsers { get; set; }

        //Relationship with SoftwareTeam
        public List<SoftwareTeam>SoftwareTeams{ get; set; }
    }
}
