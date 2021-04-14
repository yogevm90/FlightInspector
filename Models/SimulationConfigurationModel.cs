using FlightExaminator.Models.Interfaces;
using System;
using System.ComponentModel;
using System.IO;

namespace FlightExaminator.Models
{
    public class SimulationConfigurationModel : ISimulationConfigurationModel
    {
        public SimulatorRunner Runner { get; set; }
        private string configFilePath;
        public string ConfigFilePath
        {
            get { return configFilePath; }
            set
            {
                if (!File.Exists(value))
                {
                    Message = $"Config file {value} not found";
                }
                else
                {
                    Message = $"Configured file {value}";
                    configFilePath = value;
                    NotifyPropertyChanged("ConfigFilePath");
                }
            }
        }
        private string flightFilePath;
        public string FlightFilePath
        {
            get { return flightFilePath; }
            set
            {
                if (!File.Exists(value))
                {
                    Message = $"Flight data file {value} not found";
                }
                else
                {
                    Message = $"Flight data file {value} added";
                    flightFilePath = value;
                    NotifyPropertyChanged("FlightFilePath");
                }
            }
        }
        private string simulatorPath;
        public string SimulatorPath
        {
            get { return simulatorPath; }
            set
            {
                if (!Directory.Exists(value))
                {
                    Message = $"Simulator directory {value} not found";
                }
                else
                {
                    Message = $"Simulator directory: {value}";
                    simulatorPath = value;
                    NotifyPropertyChanged("SimulatorPath");
                }
            }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public void StartSimulation()
        {
            try
            {
                Runner.FlightDataPath = flightFilePath;
                Runner.SimulatorCongifFilePath = configFilePath;
                Runner.SimulatorPath = simulatorPath;
                Message = "Uploading configuration files";
                Runner.UploadConfigFile();
                Runner.UploadDataFile();
                Message = "Starting Simulator";
                Runner.StartSimulator();
                Message = "Simulator is up, checking server availability";
                Runner.CheckServerAvailability();
                Message = "Server is up!";
                Runner.Ready = true;
            }
            catch(Exception e)
            {
                Message = "Simulation start failed!\n" + e.Message;
                return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public SimulationConfigurationModel(SimulatorRunner runner)
        {
            Runner = runner;
        }
    }
}
