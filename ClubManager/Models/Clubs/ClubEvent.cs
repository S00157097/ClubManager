using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManager.Models.ClubModels
{
    [Table("ClubEvent")]
    public class ClubEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Event ID")]
        public int EventId { get; set; }

        public string Venue { get; set; }

        public string Location { get; set; }

        [Display(Name = "Start Date Time")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy @ hh:mm}")]
        public DateTime? StartDateTime { get; set; }

        [Display(Name = "End Date Time")]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy @ hh:mm}")]
        public DateTime? EndDateTime { get; set; }

        [ForeignKey("associatedClub")]
        public int ClubId { get; set; }

        public virtual Club associatedClub { get; set; }

        public virtual ICollection<Member> attendees { get; set; }

    }
}