using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class SoftwareTeam
    {
        public int SoftwareId { get; set; }
        public Software Software { get; set; }

        public int TeamId { get; set; }
        public Team Team{ get; set; }
    }
}
