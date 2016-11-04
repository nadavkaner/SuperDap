using System.Collections;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public class StatisticsModel
    {
        public IEnumerable<ComputerProcessorTypeStatistics> ProcessorTypeComputersStatistics { get; set; }
        public IEnumerable<ComputerGraphicsCardStatistics> GraphicsCardComputersStatistics { get; set; }
    }

    public class ComputerProcessorTypeStatistics
    {
        public string ProcessorType { get; set; }
        public int NumberOfComputers { get; set; }
    }

    public class ComputerGraphicsCardStatistics
    {
        public string GraphicsCard { get; set; }
        public int NumberOfComputers { get; set; }
    }
}