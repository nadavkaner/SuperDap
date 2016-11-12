using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Models
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime AvailableFrom { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [DefaultValue(0)]
        public int NumberOfPurchases { get; set; }
        public virtual Company Company { get; set; }
        public Guid StoreId { get; set; }
    }

    public class Computer : Item
    {
        [DefaultValue(false)]
        public bool HasTouchScreen { get; set; }
        [Required]
        public GraphicsCardManufacturer GraphicsCardManufacturer { get; set; }
        [Required]
        public ProcessorType ProcessorType { get; set; }
    }

    public enum GraphicsCardManufacturer
    {
        Intel,
        Nvidia,
        Amd
    }
    public enum ProcessorType
    {
        i7,
        i5,
        i3
    }
}