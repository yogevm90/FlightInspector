using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace FlightExaminator
{
    /*
     * Operates infrot of the simulator installed on computer.
     * Gets and sets configuration and data to simulator.
     */
    public class SimulatorRunner
    {
        public string SimulatorPath { get; set; }
        public string SimulatorCongifFilePath { get; set; }
        public string FlightDataPath { get; set; }
        private List<string> FlightDataList { get; set; }
        private int Port { get; set; }
        public bool Ready { get; set; }
        public Dictionary<string, double> DataDictionary;
        private Dictionary<string, int> configurationDictionary;
        private char separationChar;

        public SimulatorRunner(int port)
        {
            Port = port;
            Ready = false;
            configurationDictionary = new Dictionary<string, int>();
            DataDictionary = new Dictionary<string, double>();
            separationChar = ',';
        }

        /*
         * Sets configuration file for next flight.
         * Copies file to installation directory of simulator
         * Sets the data according to its location in the xml config file
         */
        public void UploadConfigFile()
        {
            // Check installation path is known
             if (String.IsNullOrEmpty(SimulatorPath))
            {
                throw new Exception("Please configure simulator directory first");
            }
            string configFileName = Path.GetFileName(SimulatorCongifFilePath);
            string pathToUploadConfig = SimulatorPath + $@"\data\Protocol\{configFileName}";
            // Copy file to data dir
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

            // Set configuration data as it is located in the file
            try
            {
                XmlDocument configDocument = new XmlDocument();
                configDocument.LoadXml(pathToUploadConfig);
                XmlNode inputNode = configDocument.DocumentElement.SelectSingleNode("input");
                int listLocation = 0;
                foreach (XmlNode childNode in inputNode.ChildNodes)
                {
                    if (childNode.Name == "var_separator")
                    {
                        string separator = childNode.Value;
                        separationChar = separator[0];
                    }
                    if (childNode.Name == "chunk")
                    {
                        XmlNode nameNode = childNode.SelectSingleNode("name");
                        string name = nameNode.Value;
                        configurationDictionary.Add(name, listLocation);
                        listLocation++;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Config file not in correct format: {e.Message}");
            }
        }

        /*
         * Sets the data to be sent to the simulator
         */
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

        /*
         * Run simulator exe
         */
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

        /*
         * Open Tcp connection and send to simulator next data according to the location given
         */
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

            // Update data dictionary 
            string line = FlightDataList.ElementAt(nextLocation);
            string[] elementsData = line.Split(separationChar);
            foreach (var couple in configurationDictionary)
            {
                string name = couple.Key;
                double value = Double.Parse(elementsData[couple.Value]);
                DataDictionary[name] = value;
            }
        }

        /*
         * Wait for simulator server to turn up
         */
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
