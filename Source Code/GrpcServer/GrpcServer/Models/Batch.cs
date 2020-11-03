using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Models
{
    public class Batch
    {
        public int BatchID { get; set; }

        public List<double> RequestedSamples { get; set; }
    }
}
