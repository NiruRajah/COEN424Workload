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
                Int32 rfwId = Convert.ToInt32(Console.ReadLine());

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
                Int32 batchUnit = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter BatchID: ");
                Int32 batchId = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter BatchSize: ");
                Int32 batchSize = Convert.ToInt32(Console.ReadLine());

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

                /*char[] response = workloadResponse.Batches.ToString().ToCharArray();

                List<string> finalReponse = new List<string>();

                for(int i = 0; i < response.Length; i++)
                {
                    if(response[i].Equals('{') || response[i].Equals('}') || response[i].Equals(','))
                    {

                        finalReponse.Add("\n" + response[i].ToString());
                    }
                    else
                    {
                        finalReponse.Add(response[i].ToString());
                    }
                }
                
                for(int i = 0; i < finalReponse.Count(); i++)
                {
                    Console.Write(finalReponse[i]);
                }*/

                for (int i = 0; i < workloadResponse.Batches.Count(); i++)
                {
                    Console.WriteLine("\t{\t\"batchID\": " + workloadResponse.Batches[i].BatchID + ",");
                    Console.WriteLine("\t\t\"requestedSamples\" [\n");
                    for(int j = 0; j < workloadResponse.Batches.Count(); j++)
                    {
                        if(j == workloadResponse.Batches.Count() - 1)
                        {
                            Console.WriteLine("\t\t\t" + workloadResponse.Batches[i].RequestedSamples[j] + "\n\t\t]\n\t},");
                        }
                        else
                        {
                            Console.WriteLine("\t\t\t" + workloadResponse.Batches[i].RequestedSamples[j] + ",");
                        }
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
