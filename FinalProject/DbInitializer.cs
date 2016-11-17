using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebGrease.Css.Extensions;

namespace FinalProject
{
    public class DbInitializer : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(userStore);

            var role = new IdentityUserRole { Role = new IdentityRole("Admin") };
            var user = new ApplicationUser() { UserName = "admin"};
            user.Roles.Add(role);
            IdentityResult result = manager.Create(user, "123123");

            var role2 = new IdentityUserRole { Role = new IdentityRole("Noob") };
            var user2 = new ApplicationUser() { UserName = "noob" };
            user.Roles.Add(role2);
            IdentityResult result2 = manager.Create(user2, "123123");
            var companies = CreateCompanies();

            companies.ForEach(s => context.Companies.Add(s));
            context.SaveChanges();

            companies = context.Companies.ToList();
            var jetBrains = companies.First(x => x.Name == "JetBrains");
            var google = companies.First(x => x.Name == "Google");
            var microsoft = companies.First(x => x.Name == "Microsoft");
            var computers = new List<DevelopmentTool>
            {
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "Resharper",
                    Description = "Resharper is cool",
                    Price = 4400,
                    LastUpdate = DateTime.Now,
                    Company = jetBrains,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.ClosedSource
                    
                },
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "IntelliJ",
                    Price = 2000,
                    Description = "IntelliJ is cool",
                    LastUpdate = DateTime.Now,
                    Company = jetBrains,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.ClosedSource
                },
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "WebStorm",
                    Description = "WebStorm is cool",
                    LastUpdate = DateTime.Now,
                    Company = jetBrains,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.ClosedSource
                },
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "Google App Engine",
                    Description = "App Engine's environments, the standard environment and the flexible environment (in beta), support a host of programming languages.",
                    LastUpdate = DateTime.Now,
                    Company = google,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.OpenSource
                },
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "Visual Studio",
                    Description = "Fully-featured IDE, productivity for any apps",
                    LastUpdate = DateTime.Now,
                    Company = microsoft,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.OpenSource
                }
            };

            computers.ForEach(c => context.DevelopmentTools.Add(c));
            context.SaveChanges();

            computers = context.DevelopmentTools.ToList();

            foreach (var company in companies)
            {
                company.MostPopularDevelopmentTool = computers.FirstOrDefault(x => x.Company.Name == company.Name);
            }

            context.SaveChanges();

            base.Seed(context);
        }

        private static List<Company> CreateCompanies()
        {
            return new List<Company>
            {
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "JetBrains",
                    Location = "Czech Republic",
                    Coordinates = new Coordinates()
                    {
                        Lat = 50.0755, 
                        Long = 14.4378
                    },
                    Revenue = 1240000,
                    ImagePath = "https://lh3.googleusercontent.com/-4urMhkpWddg/AAAAAAAAAAI/AAAAAAAAEgI/gPlHf7c0lNo/s0-c-k-no-ns/photo.jpg",
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Microsoft",
                    Location = "Washington",
                    Coordinates = new Coordinates()
                    {
                        Lat = 47.6740, 
                        Long = -122.1215
                    },
                    Revenue = 8532000000,
                    ImagePath = "http://i.giphy.com/12ayMrhdZf0Ptu.gif",
//                    ImagePath = "http://www.freeiconspng.com/uploads/microsoft-new-logo-simple-0.png",
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Google",
                    Location = "California",
                    Coordinates = new Coordinates()
                    {
                        Lat = 37.3861, 
                        Long = -122.0839
                    },
                    Revenue = 7454000000,
                    ImagePath = "http://images.dailytech.com/nimage/G_is_For_Google_New_Logo_Thumb.png"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Facebook",
                    Location = "California",
                    Coordinates = new Coordinates()
                    {
                        Lat = 37.4530, 
                        Long = -122.1817
                    },
                    Revenue = 1754000000,
                    ImagePath = "http://i.giphy.com/ijEiXYEo9DBxm.gif"
//                    ImagePath = "https://www.facebook.com/images/fb_icon_325x325.png"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Unity",
                    Location = "California",
                    Coordinates = new Coordinates()
                    {
                        Lat = 37.7749, 
                        Long = -122.4194
                    },
                    Revenue = 154000000,
                    ImagePath = "https://tecnorbita.files.wordpress.com/2015/03/unity3d-logo.png"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Git hub",
                    Location = "California",
                    Coordinates = new Coordinates()
                    {
                        Lat = 37.7749, 
                        Long = -122.4194
                    },
                    Revenue = 1000000,
                    ImagePath = "https://assets-cdn.github.com/images/modules/logos_page/Octocat.png"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "InVision",
                    Location = "Germany",
                    Coordinates = new Coordinates()
                    {
                        Lat = 51.2964, 
                        Long = 6.8402
                    },
                    Revenue = 35000000,
                    ImagePath = "https://d1qb2nb5cznatu.cloudfront.net/startups/i/21569-c86e305ed0dda7ef283d565cce8195fe-medium_jpg.jpg?buster=1425419948"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Slack Technologies",
                    Location = "Canada",
                    Coordinates = new Coordinates()
                    {
                        Lat = 49.2827, 
                        Long = -123.1207
                    },
                    Revenue = 30000000,
                    ImagePath = "https://s-media-cache-ak0.pinimg.com/originals/2b/26/43/2b26437d72e949db88e62d251c736c45.jpg"
                }
            };
        }
    }
}