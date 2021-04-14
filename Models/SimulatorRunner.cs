using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FlightExaminator
{
    public class SimulatorRunner
    {
        public string SimulatorPath { get; set; }
        public string SimulatorCongifFilePath { get; set; }
        public string FlightDataPath { get; set; }
        private List<string> FlightDataList { get; set; }
        public int MiliSecondsBetweenIterations { get; set; }
        public int NextDataLocation { get; set; }
        private int Port { get; set; }
        public bool InsertData { get; set; }
        public bool Ready { get; set; }
        

        public SimulatorRunner(int port)
        {
            Port = port;
            MiliSecondsBetweenIterations = 100;
            NextDataLocation = 0;
            InsertData = false;
            Ready = false;
        }

        public void UploadConfigFile()
        {
             if (String.IsNullOrEmpty(SimulatorPath))
            {
                throw new Exception("Please configure simulator directory first");
            }
            string configFileName = Path.GetFileName(SimulatorCongifFilePath);
            string pathToUploadConfig = SimulatorPath + $@"\data\Protocol\{configFileName}";
            try
            {
                if (File.Exists(pathToUploadConfig))
                {
                    return;
                }
                File.Copy(SimulatorCongifFilePath, pathToUploadConfig);
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException("Application does not have permissions to copy configuration file\n" +
                    $@"Run application with elevated permissions or copy config file manually to {pathToUploadConfig}");
            }
        }

        public void UploadDataFile()
        {
            string line;
            FlightDataList = new List<string>();
            StreamReader dataStreamReader = new StreamReader(FlightDataPath);
            while ((line = dataStreamReader.ReadLine()) != null)
            {
                FlightDataList.Add(line);
            }
            dataStreamReader.Close();
        }

        public void StartSimulator()
        {
            string configFileName = Path.GetFileNameWithoutExtension(SimulatorCongifFilePath);
            Process process = new Process();
            process.StartInfo.FileName = $@"cmd.exe";
            process.StartInfo.WorkingDirectory = $@"{SimulatorPath}";
            process.StartInfo.Arguments = $@"/c bin\fgfs.exe --generic=socket,in,10,127.0.0.1,{Port},tcp,{configFileName} --fdm=null";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Verb = "runas";
            process.Start();
        }

        public void InsertDataTask()
        {
            if (!Ready) return;
            InsertData = true;
            Thread thread = new Thread(InsertDataToSimulator);
            thread.Start();
        }

        private void InsertDataToSimulator()
        {
            TcpClient client = new TcpClient("127.0.0.1", Port);
            NetworkStream stream = client.GetStream();
            while (InsertData)
            {
                Byte[] data = Encoding.ASCII.GetBytes(FlightDataList.ElementAt(NextDataLocation) + "\n");
                stream.Write(data, 0, data.Length);
                stream.Flush();

                NextDataLocation++;
                if (NextDataLocation >= FlightDataList.Count())
                {
                    InsertData = false;
                    NextDataLocation = 0;
                }
                Thread.Sleep(MiliSecondsBetweenIterations);
            }
            stream.Close();
            client.Close();
        }

        public void CheckServerAvailability()
        {
            for (int i = 0; i < 600; i++)
            {
                try
                {
                    TcpClient client = new TcpClient("127.0.0.1", Port);
                    client.Close();
                    return;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                    continue;
                }
            }
            throw new Exception("Unable to connect to server after 10 minutes");
        }

        public int GetTotalLocations()
        {
            return FlightDataList.Count();
        }
    }
}
