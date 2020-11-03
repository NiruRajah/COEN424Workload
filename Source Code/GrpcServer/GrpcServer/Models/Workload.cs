using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Models
{
    public class Workload
    {
        public double CPUUtilization_Average { get; set; }

        public double NetworkIn_Average { get; set; }

        public double NetworkOut_Average { get; set; }

        public double MemoryUtilization_Average { get; set; }

        public double FinalTarget { get; set; }
    }
}
