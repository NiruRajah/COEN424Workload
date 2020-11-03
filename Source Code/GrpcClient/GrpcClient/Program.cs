using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.PowerBI.Api.Models;
using System.Collections.Generic;
using System.Linq;

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

            while (true)
            {
                bool inputChecker = true;

                Console.WriteLine("Enter RFWID: ");
                int num = -1;
                string inputRFWID = Console.ReadLine();

                while (!int.TryParse(inputRFWID, out num))
                {
                    Console.WriteLine("The value must be an integer. Please re-enter RFWID:");
                    inputRFWID = Console.ReadLine();
                }
                Int32 rfwId = Convert.ToInt32(inputRFWID);

                string benchmark = "";

                while (inputChecker)
                {
                    Console.WriteLine("Enter BenchmarkType (DvdTesting = 0, DvdTraining = 1, NdBenchTesting = 2, NdBenchTraining = 3): ");
                    benchmark = Console.ReadLine();

                    if ((benchmark.Equals("0") || benchmark.Equals("1") || benchmark.Equals("2") || benchmark.Equals("3")))
                    {
                        inputChecker = false;
                    }
                }

                inputChecker = true;

                string workloadMetric = "";

                while (inputChecker)
                {
                    Console.WriteLine("Enter WorkloadMetric (Cpu = 0, NetworkIn = 1, NetworkOut = 2, Memory = 3): ");
                    workloadMetric = Console.ReadLine();

                    if ((workloadMetric.Equals("0") || workloadMetric.Equals("1") || workloadMetric.Equals("2") || workloadMetric.Equals("3")))
                    {
                        inputChecker = false;
                    }
                }

                inputChecker = true;

                Console.WriteLine("Enter BatchUnit: ");
                string inputBatchUnit = Console.ReadLine();
                while (!int.TryParse(inputBatchUnit, out num))
                {
                    Console.WriteLine("The value must be an integer. Please re-enter BatchUnit:");
                    inputBatchUnit = Console.ReadLine();
                }
                Int32 batchUnit = Convert.ToInt32(inputBatchUnit);

                Console.WriteLine("Enter BatchID: ");
                string inputBatchID = Console.ReadLine();
                while (!int.TryParse(inputBatchID, out num))
                {
                    Console.WriteLine("The value must be an integer. Please re-enter BatchID:");
                    inputBatchID = Console.ReadLine();
                }
                Int32 batchId = Convert.ToInt32(inputBatchID);

                Console.WriteLine("Enter BatchSize: ");
                string inputBatchSize = Console.ReadLine();

                while (!int.TryParse(inputBatchSize, out num))
                {

                    Console.WriteLine("The size of the batch must be an integer. Please re-enter the size of the batch:");
                    inputBatchSize = Console.ReadLine();
                }
                Int32 batchSize = Convert.ToInt32(inputBatchSize);

                WorkloadRequest workloadRequest = new WorkloadRequest();
                workloadRequest.RFWID = rfwId;
                workloadRequest.BenchmarkType = (WorkloadRequest.Types.BenchmarkType)Enum.Parse(typeof(WorkloadRequest.Types.BenchmarkType), benchmark);
                workloadRequest.WorkloadMetric = (WorkloadRequest.Types.WorkloadMetric)Enum.Parse(typeof(WorkloadRequest.Types.WorkloadMetric), workloadMetric);
                workloadRequest.BatchUnit = batchUnit;
                workloadRequest.BatchID = batchId;
                workloadRequest.BatchSize = batchSize;

                WorkloadResponse workloadResponse = new WorkloadResponse();
                workloadResponse = await client.GetWorkloadAsync(workloadRequest);

                Console.WriteLine("Server Response:\n"
                    + "\"rfdid\": " + workloadResponse.RFDID.ToString() + ",\n"
                    + "\"lastbatchID\": " + workloadResponse.LastBatchID.ToString() + ",\n"
                    + "\"batches\": [");

                for (int i = 0; i < workloadResponse.Batches.Count(); i++)
                {
                    Console.WriteLine("\t{\t\"batchID\": " + workloadResponse.Batches[i].BatchID + ",");
                    Console.WriteLine("\t\t\"requestedSamples\": [\n");
                    for(int j = 0; j < workloadResponse.Batches[i].RequestedSamples.Count(); j++)
                    {
                        if(j == workloadResponse.Batches[i].RequestedSamples.Count() - 1)
                        {
                            Console.WriteLine("\t\t\t" + workloadResponse.Batches[i].RequestedSamples[j] + "\n\t\t]\n\t},");
                        }
                        else
                        {
                            Console.WriteLine("\t\t\t" + workloadResponse.Batches[i].RequestedSamples[j] + ",");
                        }
                    }
                    if(i == workloadResponse.Batches.Count - 1)
                    {
                        Console.WriteLine("]");
                    }
                }

                Console.WriteLine("Press any key to continue OR press '9' to exit");
                string continueORQuit = Console.ReadLine();

                if(continueORQuit.Equals("9"))
                {
                    break;
                }
            }

        }
    }
}
