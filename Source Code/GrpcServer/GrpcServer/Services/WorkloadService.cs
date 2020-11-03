using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServer.Models;
using Microsoft.Extensions.Logging;

namespace GrpcServer
{
    public class WorkloadService : Workload.WorkloadBase
    {
        private readonly ILogger<WorkloadService> _logger;
        public WorkloadService(ILogger<WorkloadService> logger)
        {
            _logger = logger;
        }

        public override Task<WorkloadResponse> GetWorkload(WorkloadRequest request, ServerCallContext context)
        {
            return Task.FromResult(GetWorkloadResponse(request));
        }

        public WorkloadResponse GetWorkloadResponse(WorkloadRequest clientRFWRequestBody)
        {
            CheckForBelowOneParameters(clientRFWRequestBody);
            WorkloadResponse serverRFD = new WorkloadResponse();
            List<Models.Workload> workloadList = new List<Models.Workload>(GetBenchmarkType(clientRFWRequestBody.BenchmarkType));
            List<double> allColumnValuesList = new List<double>(GetAllWorkloadColumnValues(clientRFWRequestBody.WorkloadMetric, workloadList));
            List<Batch> allBatchesList = new List<Batch>(GetAllBatches(clientRFWRequestBody.BatchUnit, allColumnValuesList));

            int startIndex = clientRFWRequestBody.BatchID - 1;
            int endIndex = 0;
            if ((startIndex + clientRFWRequestBody.BatchSize) <= allBatchesList.Count())
            {
                endIndex = startIndex + clientRFWRequestBody.BatchSize;
            }
            else
            {
                endIndex = allBatchesList.Count();
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                serverRFD.Batches.Add(allBatchesList[i]);
            }

            serverRFD.RFDID = clientRFWRequestBody.RFWID;
            serverRFD.LastBatchID = endIndex;

            return serverRFD;
        }

        public void CheckForBelowOneParameters(WorkloadRequest client)
        {
            if (client.RFWID < 1)
            {
                client.RFWID = 1;
            }
            if (client.BatchID < 1)
            {
                client.BatchID = 1;
            }
            if (client.BatchUnit < 1)
            {
                client.BatchUnit = 1;
            }
            if (client.BatchSize < 1)
            {
                client.BatchSize = 1;
            }
        }

        public List<Batch> GetAllBatches(int batchUnit, List<double> allColValuesList)
        {
            List<Batch> allBatchesList = new List<Batch>();

            for (int i = 0; i < allColValuesList.Count(); i += batchUnit)
            {
                int counter = 0;
                Batch batch = new Batch();

                while (counter < batchUnit)
                {
                    batch.BatchID = ((i + counter) + batchUnit) / batchUnit;
                    if (i + counter < allColValuesList.Count())
                    {
                        batch.RequestedSamples.Add(allColValuesList[i + counter]);
                    }
                    counter++;
                }
                allBatchesList.Add(batch);
            }

            return allBatchesList;
        }

        public List<double> GetAllWorkloadColumnValues(WorkloadRequest.Types.WorkloadMetric workloadMetric, List<Models.Workload> wrkloadList)
        {
            List<double> allColValuesList = new List<double>();

            for (int i = 0; i < wrkloadList.Count(); i++)
            {
                if (WorkloadRequest.Types.WorkloadMetric.Cpu == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].CPUUtilization_Average);
                }
                else if (WorkloadRequest.Types.WorkloadMetric.NetworkIn == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].NetworkIn_Average);
                }
                else if (WorkloadRequest.Types.WorkloadMetric.NetworkOut == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].NetworkOut_Average);
                }
                else if (WorkloadRequest.Types.WorkloadMetric.Memory == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].MemoryUtilization_Average);
                }
            }
            return allColValuesList;
        }

        public List<Models.Workload> GetBenchmarkType(WorkloadRequest.Types.BenchmarkType benchmrk)
        {
            List<Models.Workload> wrkloadList = new List<Models.Workload>();

            if (WorkloadRequest.Types.BenchmarkType.DvdTesting == benchmrk)
            {
                wrkloadList = ListOfWorkload.DVDTesting;
            }
            else if (WorkloadRequest.Types.BenchmarkType.DvdTraining == benchmrk)
            {
                wrkloadList = ListOfWorkload.DVDTraining;
            }
            else if (WorkloadRequest.Types.BenchmarkType.NdBenchTesting == benchmrk)
            {
                wrkloadList = ListOfWorkload.NDBTesting;
            }
            else if (WorkloadRequest.Types.BenchmarkType.NdBenchTraining == benchmrk)
            {
                wrkloadList = ListOfWorkload.NDBTraining;
            }

            return wrkloadList;
        }

    }

}

