using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkloadProject.Models
{
    public class ServerRFD
    {
        public int RFD { get; set; }

        public int LastBatchID { get; set; }

        public List<Batch> Batches { get; set; }
    }
}
