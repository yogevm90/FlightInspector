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
        string SimulatorPath { get; set; }
        string SimulatorCongifFilePath { get; set; }
        string FlightDataPath { get; set; }
        List<string> FlightDataList { get; }
        int MiliSecondsBetweenIterations { get; set; }
        int NextDataLocation { get; set; }
        int Port { get; set; }
        bool InsertData { get; set; }
        bool Running { get; set; }
        

        public SimulatorRunner(string configFilePath, string simulatorPath, string flightDataFilePath, int port)
        {
            SimulatorPath = simulatorPath;
            SimulatorCongifFilePath = configFilePath;
            FlightDataPath = flightDataFilePath;
            Port = port;
            MiliSecondsBetweenIterations = 100;
            NextDataLocation = 0;
            InsertData = false;
            Running = false;

            if (!File.Exists(SimulatorCongifFilePath))
            {
                throw new FileNotFoundException($"Config file {SimulatorCongifFilePath} not found");
            }
            if (!File.Exists(FlightDataPath))
            {
                throw new FileNotFoundException($"Flight data {FlightDataPath} file not found");
            }
            if (!Directory.Exists(SimulatorPath))
            {
                throw new DirectoryNotFoundException($"Simulator directory {SimulatorPath} not found");
            }

            string line;
            FlightDataList = new List<string>();
            StreamReader dataStreamReader = new StreamReader(FlightDataPath);
            while((line = dataStreamReader.ReadLine()) != null)
            {
                FlightDataList.Add(line);
            }
            dataStreamReader.Close();

            UploadConfigFile();
            StartSimulator();
        }

        private void UploadConfigFile()
        {
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

        private void StartSimulator()
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
            if (Running) return;
            InsertData = true;
            Running = true;
            Thread thread = new Thread(InsertDataToSimulator);
            thread.Start();
        }

        private void InsertDataToSimulator()
        {
            while (!CheckServerAvailability())
            {
                Thread.Sleep(1000);
            }
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
            Running = false;
        }

        private bool CheckServerAvailability()
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", Port);
                client.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
