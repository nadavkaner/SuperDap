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

            var stores = new List<Store>
            {
                new Store
                {
                    StoreId = Guid.NewGuid(),
                    Name = "Ivory",
                    Location = "Tel Aviv",
                    Coordinates = new Coordinates()
                    {
                        Lat = 32.077263, 
                        Long = 34.777505
                    },
                    HasSecurity = false,
                    HasCrippleEntrance = true
                },
                new Store
                {
                    StoreId = Guid.NewGuid(),
                    Name = "KSP",
                    Location = "Haifa",
                    Coordinates = new Coordinates()
                    {
                        Lat = 32.803085, 
                        Long = 34.986049
                    },
                    HasSecurity = true,
                    HasCrippleEntrance = false
                }
            };

            stores.ForEach(s => context.Stores.Add(s));
            context.SaveChanges();

            stores = context.Stores.ToList();

            var computers = new List<Computer>
            {
                new Computer
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Dell 1",
                    HasTouchScreen = true,
                    Category = "Laptops",
                    Price = 4400,
                    GraphicsCardManufacturer = GraphicsCardManufacturer.Nvidia,
                    ProcessorType = ProcessorType.i7,
                    AvailableFrom = DateTime.Now,
                    Store = stores[0]
                },
                new Computer
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Dell 2",
                    HasTouchScreen = false,
                    Category = "Laptops",
                    Price = 2000,
                    GraphicsCardManufacturer = GraphicsCardManufacturer.Amd,
                    ProcessorType = ProcessorType.i3,
                    AvailableFrom = DateTime.Now,
                    Store = stores[0]
                },
                new Computer
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Lenovo 1",
                    HasTouchScreen = true,
                    Category = "Laptops",
                    Price = 2500,
                    GraphicsCardManufacturer = GraphicsCardManufacturer.Intel,
                    ProcessorType = ProcessorType.i5,
                    AvailableFrom = DateTime.Now,
                    Store = stores[0]
                },
                new Computer
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Asus 1",
                    HasTouchScreen = false,
                    Category = "Laptops",
                    Price = 4200,
                    GraphicsCardManufacturer = GraphicsCardManufacturer.Nvidia,
                    ProcessorType = ProcessorType.i7,
                    AvailableFrom = DateTime.Now,
                    Store = stores[1]
                },
                new Computer
                {
                    ItemId = Guid.NewGuid(),
                    Name = "HP 1",
                    HasTouchScreen = false,
                    Category = "Laptops",
                    Price = 3500,
                    GraphicsCardManufacturer = GraphicsCardManufacturer.Intel,
                    ProcessorType = ProcessorType.i7,
                    AvailableFrom = DateTime.Now,
                    Store = stores[1]
                }
            };

            computers.ForEach(c => context.Computers.Add(c));
            context.SaveChanges();

            computers = context.Computers.ToList();
         
            stores[0].MostPopularComputer = computers.First();
            stores[1].MostPopularComputer = computers.Last();

            context.SaveChanges();

            base.Seed(context);
        }
    }
}