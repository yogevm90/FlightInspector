using FlightExaminator.Models;
using FlightExaminator.Models.Interfaces;
using System;
using System.ComponentModel;

namespace FlightExaminator.ViewModels
{
    public class SimulationConfigurationViewModel : INotifyPropertyChanged
    {
        private ISimulationConfigurationModel model;
        public SimulationConfigurationViewModel(SimulationConfigurationModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public string VM_FlightFilePath
        {
            get { return model.FlightFilePath; }
            set { model.FlightFilePath = value; }
        }

        public string VM_SimulatorPath
        {
            get { return model.SimulatorPath; }
            set { model.SimulatorPath = value; }
        }

        public string VM_ConfigFilePath
        {
            get { return model.ConfigFilePath; }
            set { model.ConfigFilePath = value; }
        }

        public string VM_Message
        {
            get { return model.Message; }
        }

        public void StartSimulation()
        {
            if (String.IsNullOrEmpty(VM_ConfigFilePath)
                || String.IsNullOrEmpty(VM_SimulatorPath)
                || String.IsNullOrEmpty(VM_FlightFilePath))
            {
                model.Message = "Please configure files first";
            }
            model.StartSimulation();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
