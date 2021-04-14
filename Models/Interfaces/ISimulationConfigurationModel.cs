using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightExaminator.Models.Interfaces
{
    public interface ISimulationConfigurationModel : INotifyPropertyChanged
    {
        SimulatorRunner Runner { get; set; }
        string ConfigFilePath { get; set; }
        string FlightFilePath { get; set; }
        string SimulatorPath { get; set; }
        string Message { get; set; }
        void StartSimulation();
    }
}
