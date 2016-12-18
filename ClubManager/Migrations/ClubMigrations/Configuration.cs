namespace ClubManager.Migrations.ClubMigrations
{
    using Models.Clubs;
    using CsvHelper;
    using Models.ClubModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<ClubManager.Models.Clubs.ClubDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ClubMigrations";
        }

        protected override void Seed(ClubManager.Models.Clubs.ClubDbContext context)
        {
            context.Clubs.AddOrUpdate(c => c.ClubName,
                new Club
                {
                    ClubName = "The Tiddly Winks Club",
                    CreationDate = DateTime.Now,
                    clubEvents = new List<ClubEvent>()
                    {
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Arena"
                        },
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Main Canteen"
                        }
                    }
                },
                new Club
                {
                    ClubName = "The Chess Club",
                    CreationDate = DateTime.Now,
                    clubEvents = new List<ClubEvent>()
                    {
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Add(new TimeSpan(5, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Add(new TimeSpan(5, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Arena"
                        },
                        new ClubEvent
                        {
                            StartDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            EndDateTime = DateTime.Now.Subtract(new TimeSpan(3, 0, 0, 0, 0)),
                            Location = "Sligo",
                            Venue = "Main Canteen"
                        }
                    }
                });


            #region Reading_Student
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "ClubManager.Migrations.ClubMigrations.students.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.HasHeaderRecord = false;
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    var testStudents = csvReader.GetRecords<Student>().ToArray();
                    context.Students.AddOrUpdate(s => s.StudentId, testStudents);
                }
            }
            #endregion
        }
    }
}
