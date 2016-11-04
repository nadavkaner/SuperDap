using System.Collections.Generic;

namespace FinalProject.Models
{
    public class ComputersModel
    {
        public IEnumerable<Computer> Computers { get; set; }
        public GraphicsCardManufacturer? GraphicsCardManufacturerFilter { get; set; }
        public ProcessorType? ProcessorTypeFilter { get; set; }
        public bool? TouchScreenFilter { get; set; }
    }
}