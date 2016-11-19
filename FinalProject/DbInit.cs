using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject
{
    public class DbInit : DropCreateDatabaseAlways<Context>
    {
        protected override void Seed(Context context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(userStore);

            var role = new IdentityUserRole { Role = new IdentityRole("Admin") };
            const string adminId = "132bd545-ee26-42fe-b982-390989bfe9c9";
            var adminUser = new ApplicationUser() { Id = adminId, UserName = "admin" };
            adminUser.Roles.Add(role);
            IdentityResult result = manager.Create(adminUser, "123123");

            var role2 = new IdentityUserRole { Role = new IdentityRole("ApplicationUser") };
            const string regularUserid = "b96ce3c0-436b-4eba-8c20-f0af0030a0f2";
            var regularUser = new ApplicationUser() { Id = regularUserid, UserName = "regular" };
            regularUser.Roles.Add(role2);
            IdentityResult result2 = manager.Create(regularUser, "123123");

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
                    ImagePath = "https://confluence.jetbrains.com/download/attachments/15797318/WI?version=2&modificationDate=1449749629000&api=v2",
                    SiteUrl = "https://jetbrains.com/webstorm",
                    Price = 120,
                    LastUpdate = DateTime.Now,
                    Company = jetBrains,
                    NumberOfRaters = 3,
                    Rate = 5,
                    NumberOfUsers = 50000,
                    SourceCodeLicense = SourceCodeLicense.ClosedSource,
                    Comments = new List<Comment>
                    {
                        new Comment { Id = Guid.NewGuid(), Text = "Awsome development tool, i use iut all the time", User = adminUser, Date = DateTime.Now.AddSeconds(0) },
                        new Comment { Id = Guid.NewGuid(), Text = "Yep, i build some exrecises for colman with it, was great", User = regularUser, Date = DateTime.Now.AddSeconds(10) },
                        new Comment { Id = Guid.NewGuid(), Text = "I'm in the theard year there!", User = adminUser, Date = DateTime.Now.AddSeconds(20) },
                    }
                },
                new DevelopmentTool
                {
                    Id = Guid.NewGuid(),
                    Name = "Google App Engine",
                    Description = "App Engine's environments, the standard environment and the flexible environment (in beta), support a host of programming languages.",
                    Price = 0,
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
                    Price = 0,
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
                    TotalRevenue = 12400,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 2000, Year = 2012},  
                        new RevenueForYear { Revenue = 4000, Year = 2013},  
                        new RevenueForYear { Revenue = 1500, Year = 2014},  
                        new RevenueForYear { Revenue = 2500, Year = 2015},  
                        new RevenueForYear { Revenue = 2400, Year = 2016},  
                    },
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
                    TotalRevenue = 85320,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 5000, Year = 2012},
                        new RevenueForYear { Revenue = 24000, Year = 2013},
                        new RevenueForYear { Revenue = 10000, Year = 2014},
                        new RevenueForYear { Revenue = 32000, Year = 2015},
                        new RevenueForYear { Revenue = 5400, Year = 2016},
                    },
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
                    TotalRevenue = 115000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 10000, Year = 2012},
                        new RevenueForYear { Revenue = 8000, Year = 2013},
                        new RevenueForYear { Revenue = 15000, Year = 2014},
                        new RevenueForYear { Revenue = 29000, Year = 2015},
                        new RevenueForYear { Revenue = 38000, Year = 2016},
                    },
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
                    TotalRevenue = 105000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 8000, Year = 2012},
                        new RevenueForYear { Revenue = 36000, Year = 2013},
                        new RevenueForYear { Revenue = 15000, Year = 2014},
                        new RevenueForYear { Revenue = 41000, Year = 2015},
                        new RevenueForYear { Revenue = 50000, Year = 2016},
                    },
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
                    TotalRevenue = 56000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 8000, Year = 2012},
                        new RevenueForYear { Revenue = 10000, Year = 2013},
                        new RevenueForYear { Revenue = 8500, Year = 2014},
                        new RevenueForYear { Revenue = 9800, Year = 2015},
                        new RevenueForYear { Revenue = 1555, Year = 2016},
                    },
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
                    TotalRevenue = 85000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 16666, Year = 2012},
                        new RevenueForYear { Revenue = 23000, Year = 2013},
                        new RevenueForYear { Revenue = 15000, Year = 2014},
                        new RevenueForYear { Revenue = 41000, Year = 2015},
                        new RevenueForYear { Revenue = 30000, Year = 2016},
                    },
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
                    TotalRevenue = 15000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 1000, Year = 2012},
                        new RevenueForYear { Revenue = 5000, Year = 2013},
                        new RevenueForYear { Revenue = 2300, Year = 2014},
                        new RevenueForYear { Revenue = 1999, Year = 2015},
                        new RevenueForYear { Revenue = 4000, Year = 2016},
                    },
                    ImagePath = "https://d1qb2nb5cznatu.cloudfront.net/startups/i/21569-c86e305ed0dda7ef283d565cce8195fe-medium_jpg.jpg?buster=1425419948"
                },
                new Company
                {
                    CompanyId = Guid.NewGuid(),
                    Name = "Slack",
                    Location = "Canada",
                    Coordinates = new Coordinates()
                    {
                        Lat = 49.2827, 
                        Long = -123.1207
                    },
                    TotalRevenue = 46000,
                    RevenuePerYears = new List<RevenueForYear>
                    {
                        new RevenueForYear { Revenue = 3000, Year = 2012},
                        new RevenueForYear { Revenue = 4800, Year = 2013},
                        new RevenueForYear { Revenue = 6777, Year = 2014},
                        new RevenueForYear { Revenue = 4900, Year = 2015},
                        new RevenueForYear { Revenue = 16670, Year = 2016},
                    },
                    ImagePath = "https://s-media-cache-ak0.pinimg.com/originals/2b/26/43/2b26437d72e949db88e62d251c736c45.jpg"
                }
            };
        }
    }
}