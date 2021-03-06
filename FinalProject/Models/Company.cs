﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public virtual DevelopmentTool MostPopularDevelopmentTool { get; set; }
        public virtual ICollection<DevelopmentTool> DevelopmentTools { get; set; }
        public Coordinates Coordinates { get; set; }
        public double TotalRevenue { get; set; }
        public double NumberOfEmployees { get; set; }
        public virtual ICollection<RevenueForYear> RevenuePerYears { get; set; }
        public string ImagePath { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class RevenueForYear
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public double Revenue { get; set; }
    }
}