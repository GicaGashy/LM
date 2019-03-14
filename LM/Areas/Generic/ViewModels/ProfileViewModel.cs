using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Areas.Generic.ViewModels
{
    public class ProfileViewModel
    {
        public AppUser AppUser { get; set; }
        public Team Team { get; set; }
    }
}
