using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class ClientRFW
    {
        public int ID { get; set; }

        public BenchmarkType BenchmarkType {get; set; }

        public WorkloadMetric WorkloadMetric { get; set; }

        public int BatchUnit { get; set; }

        public int BatchID { get; set; }

        public int BatchSize { get; set; }
    }

    public enum BenchmarkType
    {
        DVDTraining = 0,
        DVDTesting = 1,
        NDBenchTraining = 2,
        NDBenchTesting = 3
    }

    public enum WorkloadMetric
    {
        CPU = 0,
        NetworkIn = 1,
        NetworkOut = 2,
        Memory = 3
    }
}
