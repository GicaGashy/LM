using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Software
    {
        public int SoftwareId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public DateTime LicenseStart { get; set; }
        public DateTime LicenseEnd { get; set; }
        public string UseCases { get; set; }
        public string Description { get; set; }

        //relationship stuff

        //relationship with techarea
        public int TechAreaId { get; set; }
        public TechArea TechArea { get; set; }

        //relationship with type
        public int TipiId { get; set; }
        public Tipi Tipi { get; set; }

        //relationship with user
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        //relationship with SoftwareTeam
        public List<SoftwareTeam> SoftwareTeams { get; set; }
        
    }
}
