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
        private int Port { get; set; }
        public bool Ready { get; set; }
        

        public SimulatorRunner(int port)
        {
            Port = port;
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

        public void SendData(int nextLocation)
        {
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                client = new TcpClient("127.0.0.1", Port);
                stream = client.GetStream();
                Byte[] data = Encoding.ASCII.GetBytes(FlightDataList.ElementAt(nextLocation) + "\n");
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (stream != null) { stream.Close(); }
                if (client != null) { client.Close(); }
            }
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
                    Thread.Sleep(5000);
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
