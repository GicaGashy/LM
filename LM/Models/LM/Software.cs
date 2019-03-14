﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LM.Models.LM
{
    public class Software
    {
        public int SoftwareId { get; set; }
        [Required]
        public string Name { get; set; }

        public string Version { get; set; }

        public string Producer { get; set; }

        [Required]
        [Display(Name = "LicenseStart")]
        public DateTime? LicenseStart { get; set; }

        [Display(Name = "LicenseEnd")]
        [Required]
        public DateTime? LicenseEnd { get; set; }

        public string UseCases { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a correct Tech Area from the list")]
        public int TechAreaId { get; set; }
        public TechArea TechArea { get; set; }

        //relationship with type
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a correct type from the list")]
        public int TipiId { get; set; }
        public Tipi Tipi { get; set; }

        //relationship with user
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        //relationship with SoftwareTeam
        public List<SoftwareTeam> SoftwareTeams { get; set; }
        public ICollection<Team> Teams { get; set; }
        [NotMapped]
        public ICollection<int> TeamIds { get; set; }

        public double DaysLeft()
        {
            DateTime now = DateTime.Now;
            TimeSpan difference = (TimeSpan)(LicenseEnd - LicenseStart);
            TimeSpan daysPassed = (TimeSpan)(now - LicenseStart);
            return Math.Ceiling(difference.TotalDays - daysPassed.TotalDays);
        }

    }
}
