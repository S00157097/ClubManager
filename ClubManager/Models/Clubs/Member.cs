using ClubManager.Models.Clubs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManager.Models.ClubModels
{
    [Table("Member")]
    public class Member
    {   
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key,Column(Order =1)]
        [Display(Name = "Member ID")]
        public int MemberId { get; set; }

        [Key, Column(Order = 2)]
        [Display(Name = "Club Id")]
        public int ClubId { get; set; }

        [Key, Column(Order = 3)]
        [Display(Name = "Student ID")]
        public string StudentId { get; set; }

        public bool Approved { get; set; }

        public virtual Club club { get; set; }
        public virtual Student student { get; set; }
    }
}