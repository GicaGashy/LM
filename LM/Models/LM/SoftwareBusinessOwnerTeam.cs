using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class SoftwareBusinessOwnerTeam
    {
        public int SoftwareId { get; set; }
        public Software Software { get; set; }

        public int BusinessOwnerTeamId { get; set; }
        public Team BusinessOwnerTeam { get; set; }
    }
}
