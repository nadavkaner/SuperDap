using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Company
    {
        [Key]
        public Guid CompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }

        public virtual Computer MostPopularComputer { get; set; }
        public Guid MostPopularComputerId { get; set; }
        public virtual ICollection<Computer> Computers { get; set; }
        public Coordinates Coordinates { get; set; }
        public double Revenue { get; set; }
        public string ImagePath { get; set; }
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}