﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkloadProject.Models;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WorkloadProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        [HttpGet]
        public ServerRFD ServerResponse([FromBody] ClientRFW clientRFWRequestBody)
        {
            CheckForBelowOneParameters(clientRFWRequestBody);
            ServerRFD serverRFD = new ServerRFD();
            serverRFD.Batches = new List<Batch>();
            List<Workload> workloadList = new List<Workload>(GetBenchmarkType(clientRFWRequestBody.BenchmarkType));
            List<double> allColumnValuesList = new List<double>(GetAllWorkloadColumnValues(clientRFWRequestBody.WorkloadMetric, workloadList));
            List<Batch> allBatchesList = new List<Batch>(GetAllBatches(clientRFWRequestBody.BatchUnit, allColumnValuesList));

            int startIndex = clientRFWRequestBody.BatchID - 1;
            int endIndex = 0;
            if((startIndex + clientRFWRequestBody.BatchSize) <= allBatchesList.Count())
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
        
        public void CheckForBelowOneParameters(ClientRFW client)
        {
            if(client.RFWID < 1)
            {
                client.RFWID = 1;
            }
            if(client.BatchID < 1)
            {
                client.BatchID = 1;
            }
            if(client.BatchUnit < 1)
            {
                client.BatchUnit = 1;
            }
            if(client.BatchSize < 1)
            {
                client.BatchSize = 1;
            }
        }

        // Tests to see if it returns all the values in every column in DVDTesting (Not necessary for project requirements)
        [HttpGet("all")]
        public List<Workload> GetResponse()
        {
            return ListOfWorkload.DVDTesting;
        }

        public List<Batch> GetAllBatches(int batchUnit, List<double> allColValuesList)
        {
            List<Batch> allBatchesList = new List<Batch>();
            
            for(int i = 0; i < allColValuesList.Count(); i += batchUnit)
            {
                int counter = 0;
                Batch batch = new Batch();
                batch.RequestedSamples = new List<double>();

                while (counter < batchUnit)
                {
                    batch.BatchID = ((i + counter) + batchUnit) / batchUnit;
                    if(i + counter < allColValuesList.Count())
                    {
                        batch.RequestedSamples.Add(allColValuesList[i + counter]);
                    }
                    counter++;
                }
                allBatchesList.Add(batch);
            }

            return allBatchesList;
        }

        public List<double> GetAllWorkloadColumnValues(WorkloadMetric workloadMetric, List<Workload> wrkloadList)
        {
            List<double> allColValuesList = new List<double>();

            for(int i = 0; i < wrkloadList.Count(); i++)
            {
                if (WorkloadMetric.CPU == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].CPUUtilization_Average);
                }
                else if (WorkloadMetric.NetworkIn == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].NetworkIn_Average);
                }
                else if (WorkloadMetric.NetworkOut == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].NetworkOut_Average);
                }
                else if (WorkloadMetric.Memory == workloadMetric)
                {
                    allColValuesList.Add(wrkloadList[i].MemoryUtilization_Average);
                }
            }
            return allColValuesList;
        }

        public List<Workload> GetBenchmarkType(BenchmarkType benchmrk)
        {
            List<Workload> wrkloadList = new List<Workload>();

            if (BenchmarkType.DVDTesting == benchmrk)
            {
                wrkloadList = ListOfWorkload.DVDTesting;
            }
            else if (BenchmarkType.DVDTraining == benchmrk)
            {
                wrkloadList = ListOfWorkload.DVDTraining;
            }
            else if (BenchmarkType.NDBenchTesting == benchmrk)
            {
                wrkloadList = ListOfWorkload.NDBTesting;
            }
            else if (BenchmarkType.NDBenchTraining == benchmrk)
            {
                wrkloadList = ListOfWorkload.NDBTraining;
            }

            return wrkloadList;
        }
    }
}
