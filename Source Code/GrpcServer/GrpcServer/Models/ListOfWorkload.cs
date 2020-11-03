using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace GrpcServer.Models
{
    public class ListOfWorkload
    {
        //public static string FilePath = Path.Combine(Environment.CurrentDirectory) + "../../../../Data";
        public static string FilePath = Path.Combine(Environment.CurrentDirectory) + "/Data";

        public static string FilePathDVDTesting = FilePath + "/DVD-testing.csv";

        public static string FilePathDVDTraining = FilePath + "/DVD-training.csv";

        public static string FilePathNDBenchTesting = FilePath + "/NDBench-testing.csv";

        public static string FilePathNDBenchTraining = FilePath + "/NDBench-training.csv";

        public static List<Workload> DVDTesting = new List<Workload>();
        public static List<Workload> DVDTraining = new List<Workload>();
        public static List<Workload> NDBTesting = new List<Workload>();
        public static List<Workload> NDBTraining = new List<Workload>();

        public static void GetWorkloads()
        {
            DVDTesting = GetListOfData(FilePathDVDTesting);
            DVDTraining = GetListOfData(FilePathDVDTraining);
            NDBTesting = GetListOfData(FilePathNDBenchTesting);
            NDBTraining = GetListOfData(FilePathNDBenchTraining);
        }

        public static List<Workload> GetListOfData(string filePath)
        {
            List<Workload> list = new List<Workload>();

            string[] lines = File.ReadAllLines(filePath);

            for (int i = 1; i < lines.Length; i++)
            {
                string[] valuesPerLine = new string[5];
                valuesPerLine = lines[i].Split(",");

                Workload workload = new Workload();

                workload.CPUUtilization_Average = Convert.ToDouble(valuesPerLine[0]);
                workload.NetworkIn_Average = Convert.ToDouble(valuesPerLine[1]);
                workload.NetworkOut_Average = Convert.ToDouble(valuesPerLine[2]);
                workload.MemoryUtilization_Average = Convert.ToDouble(valuesPerLine[3]);
                workload.FinalTarget = Convert.ToDouble(valuesPerLine[4]);

                list.Add(workload);
            }

            return list;
        }
    }
}
