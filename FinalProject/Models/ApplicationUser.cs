using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<Item> PurchasedItems { get; set; }
    }
}