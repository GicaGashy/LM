using System;
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

        public string Vendor { get; set; }

        //Licenses
        [Display(Name = "LicenseStart")]
        public DateTime? LicenseStart { get; set; }

        [Display(Name = "LicenseEnd")]
        public DateTime? LicenseEnd { get; set; }

        [Range(0, int.MaxValue, ErrorMessage ="Number of licenses cannot be negative")]
        public int? TotalLicenses { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of licenses cannot be negative")]
        public int? LicensesInUse { get; set; }

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

        //relatinoship with SoftwareBusinessOwner
        /*
        public List<SoftwareBusinessOwnerTeam>SoftwareBusinessOwnerTeams{ get; set; }
        public ICollection<Team> BusinessOwnerTeams { get; set; }
        public virtual ICollection<Team> BusinesOwnerTeams{ get; set; }
        */
        public IList<SoftwareBusinessOwnerTeam> SoftwareBusinessOwnerTeams { get; set; }
        [NotMapped]
        public ICollection<int>BusinessOwnerTeamIds { get; set; }
        [NotMapped]
        public ICollection<Team> BusinessOwnerTeams { get; set; }


        //relationship with ItOwner
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a correct IT Owner from the list")]
        [NotMapped]
        public int ItOwnerId { get; set; }
        public Team ItOwner { get; set; }

        [StringLength(255)]
        public string ItOwnerName { get; set; }

        //relationship with BusinessOwner one to many
        /*
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a correct Business Owner from the list")]
        [NotMapped]
        public int BusinessOwnerId { get; set; }
        public Team BusinessOwner { get; set; }
        */

        [Required(ErrorMessage = "Please select a correct Audience from the list")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a correct Audience from the list")]
        //relationship with Purpose
        public int PurposeId { get; set; }
        public Purpose Purpose { get; set; }

        //relationship with Resller
        public int? ResellerId { get; set; }
        public Reseller Reseller { get; set; }


        //bool: usage, internet, mobile
        [Required]
        public bool IsUsed { get; set; }
        [Required]
        public bool IsInternetFacing { get; set; }
        [Required]
        public bool IsMobile { get; set; }

        //Standardizes (date of entry)
        public DateTime? Standardized { get; set; }
        //Review (date of edit)
        public DateTime? Review { get; set; }

        [Range(0, 4)]
        public int Confidentiality { get; set; }
        [NotMapped]
        public string[] ConfidentialityComments { get; set; } = new string[] {"None", "Public", "Restricted", "Confidential", "Secret" };

        [Range(0, 4)]
        public int Integrity { get; set; }
        [NotMapped]
        public string[] IntegrityComments { get; set; } = new string[] { "Null", "None", "Standard", "Enhanced", "Unalterable" };

        [Range(0, 4)]
        public int Availability { get; set; }
        [NotMapped]
        public string[] AvailabilityComments { get; set; } = new string[] { "None", "Minimum Service", "Medium Avaiilability", "High Availability", "No downtime tolerated" };

        [Range(0, 4)]
        public int Traceability { get; set; }
        [NotMapped]
        public string[] TraceabilityComments { get; set; } = new string[] { "Null", "None", "Minimal", "Simple", "Detailed" };


        public double DaysLeft()
        {
            DateTime now = DateTime.Now;
            TimeSpan difference = (TimeSpan)(LicenseEnd - LicenseStart);
            TimeSpan daysPassed = (TimeSpan)(now - LicenseStart);
            return difference.TotalDays - daysPassed.TotalDays;
        }

        public double TotalDays()
        {
            TimeSpan total = (TimeSpan)(LicenseEnd - LicenseStart);
            return total.TotalDays;
        }

        public double DaysPassed()
        {
            DateTime now = DateTime.Now;
            TimeSpan daysPassed = (TimeSpan)(now - LicenseStart);
            return daysPassed.TotalDays;
        }

        public decimal AverageCiatScore()
        {
            decimal d = ((this.Confidentiality + this.Integrity + this.Availability + this.Traceability) / (decimal) 4.00);

            return decimal.Round(d, 2, MidpointRounding.AwayFromZero);
        }

        public string GetFinalScore()
        {
            string c = "Confidentiality Score = " + Confidentiality + " ( " + ConfidentialityComments[Confidentiality] + " ) ";
            string i = "Integrity Score = " + Integrity + " ( " + IntegrityComments[Integrity] + " ) ";
            string a = "Availability Score = " + Availability + " ( " + AvailabilityComments[Availability] + " ) ";
            string t = "Traceability Score = " + Traceability + " ( " + TraceabilityComments[Traceability] + " ) ";

            return c + "\n" + i + "\n" + a + "\n" + t + "\nTotal score: " + AverageCiatScore();
        }

        public string GetConfidentialityComment(int i)
        {
            try
            {
                return ConfidentialityComments[i];
            }
            catch (IndexOutOfRangeException e)
            {
                return "Most probably the value was 0 \n" + e.ToString();
            }
        }

        public string GetIntegrityComment(int i)
        {
            return IntegrityComments[i];
        }

        public string GetAvailabilityComment(int i)
        {
            return AvailabilityComments[i];
        }

        public string GetTraceabilityComment(int i)
        {
            return TraceabilityComments[i];
        }


    }
}
