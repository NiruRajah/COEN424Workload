using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.PowerBI.Api.Models;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter Server in format URL:Port");
            string serverURL = Console.ReadLine();
            var channel = GrpcChannel.ForAddress(serverURL);
            var client = new Workload.WorkloadClient(channel);

            Console.WriteLine("Enter RFWID: ");
            Int32 rfwId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter BenchmarkType (DvdTesting = 0, DvdTraining = 1, NdBenchTesting = 2, NdBenchTraining = 3): ");
            string benchmark = Console.ReadLine();

            Console.WriteLine("Enter WorkloadMetric (Cpu = 0, NetworkIn = 1, NetworkOut = 2, Memory = 3): ");
            string workloadMetric = Console.ReadLine();

            Console.WriteLine("Enter BatchUnit: ");
            Int32 batchUnit = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter BatchID: ");
            Int32 batchId = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter BatchSize: ");
            Int32 batchSize = Convert.ToInt32(Console.ReadLine());

            WorkloadRequest workloadRequest = new WorkloadRequest();
            workloadRequest.RFWID = rfwId;
            workloadRequest.BenchmarkType = (WorkloadRequest.Types.BenchmarkType) Enum.Parse(typeof(WorkloadRequest.Types.BenchmarkType),benchmark);
            workloadRequest.WorkloadMetric = (WorkloadRequest.Types.WorkloadMetric)Enum.Parse(typeof(WorkloadRequest.Types.WorkloadMetric), workloadMetric);
            workloadRequest.BatchUnit = batchUnit;
            workloadRequest.BatchID = batchId;
            workloadRequest.BatchSize = batchSize;

            WorkloadResponse workloadResponse = new WorkloadResponse();
            workloadResponse = await client.GetWorkloadAsync(workloadRequest);

            Console.WriteLine("Server Response: " + workloadResponse.ToString());

            
        }
    }
}
