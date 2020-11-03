using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Models
{
    public class ClientRFW
    {
        public int RFWID { get; set; }

        public BenchmarkType BenchmarkType {get; set; }

        public WorkloadMetric WorkloadMetric { get; set; }

        public int BatchUnit { get; set; }

        public int BatchID { get; set; }

        public int BatchSize { get; set; }
    }

    public enum BenchmarkType
    {
        DVDTesting = 0,
        DVDTraining = 1,
        NDBenchTesting = 2,
        NDBenchTraining = 3
        
    }

    public enum WorkloadMetric
    {
        CPU = 0,
        NetworkIn = 1,
        NetworkOut = 2,
        Memory = 3
    }
}
