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
        
        public string CompanyId { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public SourceCodeLicense SourceCodeLicense { get; set; }
        public string SiteUrl { get; set; }
    }

    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
    
        public string Text { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        public DateTime Date { get; set; }
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