using System.Collections;
using System.Collections.Generic;

namespace FinalProject.Models
{
    public class StatisticsModel
    {
        public IEnumerable<CompanyRevenue> CompanyRevenues { get; set; }
    }

    public class CompanyRevenue
    {
        public string Company { get; set; }
        public double Revenue{ get; set; }
        public IEnumerable<RevenueForYear> RevenueForYear { get; set; }
    }
}