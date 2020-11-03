using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Models
{
    public class ServerRFD
    {
        public int RFDID { get; set; }

        public int LastBatchID { get; set; }

        public List<Batch> Batches { get; set; }
    }
}
