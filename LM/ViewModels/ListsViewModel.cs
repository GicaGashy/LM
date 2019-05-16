using LM.Models.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LM.ViewModels
{
    public class ListsViewModel
    {
        public ICollection<Software> Softwares{ get; set; }
        public ICollection<TechArea> TechAreas{ get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<SoftwareTeam> SoftwareTeams { get; set; }
        public ICollection<Team> Teams { get; set; }
        public Software[][] sortedSoftwares { get; set; }
        public string Color { get; set; }
    }
}
