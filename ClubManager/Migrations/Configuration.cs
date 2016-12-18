namespace ClubManager.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System;
    using Models.Clubs;
    using System.Collections.Generic;
    using System.Linq;
    using Models.ClubModels;

    internal sealed class Configuration : DbMigrationsConfiguration<ClubManager.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(ClubManager.Models.ApplicationDbContext context)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            PasswordHasher hasher = new PasswordHasher();

            context.Roles.AddOrUpdate(r => r.Name,
                new IdentityRole("Admin")
                , new IdentityRole("ClubAdmin")
                , new IdentityRole("Member")
                , new IdentityRole("Student"));

            context.Users.AddOrUpdate(u => u.Email,
                new ApplicationUser
                {
                    StudentId = "S00157097",
                    Email = "S00157097@mail.itsligo.ie",
                    UserName = "S00157097@mail.itsligo.ie",
                    PasswordHash = hasher.HashPassword("sS00157097$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    DateJoined = DateTime.Now
                },
                new ApplicationUser
                {
                    StudentId = "S00157098",
                    Email = "S00157098@mail.itsligo.ie",
                    UserName = "S00157098@mail.itsligo.ie",
                    PasswordHash = hasher.HashPassword("sS00157098$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    DateJoined = DateTime.Now
                },
                new ApplicationUser
                {
                    StudentId = "S00157099",
                    Email = "S00157099@mail.itsligo.ie",
                    UserName = "S00157099@mail.itsligo.ie",
                    PasswordHash = hasher.HashPassword("sS00157099$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    DateJoined = DateTime.Now
                },
                new ApplicationUser
                {
                    StudentId = "S00157100",
                    Email = "S00157100@mail.itsligo.ie",
                    UserName = "S00157100@mail.itsligo.ie",
                    PasswordHash = hasher.HashPassword("sS00157100$1"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    DateJoined = DateTime.Now
                });

            //context.Users.Find(new { Email = "S00157097@mail.itsligo.ie" });

            ApplicationUser admin = userManager.FindByEmail("S00157097@mail.itsligo.ie");
            if (admin != null)
                userManager.AddToRoles(admin.Id, new string[] { "Admin", "Member", "ClubAdmin" });

            ApplicationUser clubAdmin = userManager.FindByEmail("S00157098@mail.itsligo.ie");
            if (clubAdmin != null)
                userManager.AddToRoles(clubAdmin.Id, new string[] { "ClubAdmin" });

            ApplicationUser member = userManager.FindByEmail("S00157099@mail.itsligo.ie");
            if (member != null)
                userManager.AddToRoles(member.Id, new string[] { "Member" });

            ApplicationUser student = userManager.FindByEmail("S00157100@mail.itsligo.ie");
            if (student != null)
                userManager.AddToRoles(student.Id, new string[] { "Student" });

            //SeedStudents(context);
        }

        public void SeedStudents(ApplicationDbContext current)
        {
            List<Student> selectedStudents = new List<Student>();

            using (ClubDbContext ctx = new ClubDbContext())
            {
                var randomStudentSet = ctx.Students
                    .Select(s => new { s.StudentId, r = Guid.NewGuid() });

                List<string> subset = randomStudentSet
                    .OrderBy(s => s.r)
                    .Select(s => s.StudentId).Take(10).ToList();

                foreach (string s in subset)
                {
                    selectedStudents.Add(
                        ctx.Students.First(st => st.StudentId == s)
                        );
                }

                Club chosen = ctx.Clubs.First();

                foreach (Student s in selectedStudents)
                {
                    ctx.Members.AddOrUpdate(m => m.StudentId,
                        new Member
                        {
                            ClubId = chosen.ClubId,
                            StudentId = s.StudentId
                        });
                }
                ctx.SaveChanges();
            }

            foreach (Student s in selectedStudents)
            {
                current.Users.AddOrUpdate(u => u.StudentId,
                    new ApplicationUser
                    {
                        StudentId = s.StudentId,
                        UserName = s.StudentId + "@mail.itsligo.ie",
                        Email = s.StudentId + "@mail.itsligo.ie",
                        EmailConfirmed = true,
                        DateJoined = DateTime.Now,
                        PasswordHash = new PasswordHasher().HashPassword("s" + s.StudentId + "$1"),
                        SecurityStamp = Guid.NewGuid().ToString(),
                    });
            }

            current.SaveChanges();
        }
    }
}
