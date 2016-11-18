using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class DevelopmentTool
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { set; get; }

        public string ImagePath { set; get; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }

        [DefaultValue(0)]
        public int NumberOfUsers { get; set; }

        [DefaultValue(0)]
        public int NumberOfRaters { get; set; }

        [DefaultValue(0)]
        public decimal Rate { get; set; }

        public virtual Company Company { get; set; }

//        public virtual IList<Tag> Tags { get; set; }

//        public virtual IList<Rating> Ratings { get; set; }

        public SourceCodeLicense SourceCodeLicense { get; set; }
        public string SiteUrl { get; set; }
    }

    public class Rating
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int RateNumber { get; set; }

        public string Comment { get; set; }

        public ApplicationUser User { get; set; }
    }

    public class Tag
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public enum  SourceCodeLicense 
    {
        ClosedSource,
        OpenSource

    }
}