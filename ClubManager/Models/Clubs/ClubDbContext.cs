using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ClubManager.Models.ClubModels;

namespace ClubManager.Models.Clubs
{
    public class ClubDbContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubEvent> ClubEvents { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Student> Students { get; set; }

        public ClubDbContext()
            : base("DefaultConnection")
        {
        }

        public static ClubDbContext Create()
        {
            return new ClubDbContext();
        }
    }
}