using FlightExaminator.Models.Interfaces;
using System.ComponentModel;
using System.Threading;

namespace FlightExaminator.Models
{
    public class FlightDataModel : IFlightDataModel
    {
        private SimulatorRunner sm;
        private double aileron;
        public double Aileron 
        { 
            get { return aileron; }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        private double elevator;
        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }
        private double rudder;
        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        private double throttle;
        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        public FlightDataModel(SimulatorRunner sm)
        {
            this.sm = sm;
            Thread thread = new Thread(GetValuesTask);
            thread.Start();
        }

        // Get all data relevant from the simulation runner
        private void GetValuesTask()
        {
            while (true)
            {
                if (sm.DataDictionary.ContainsKey("aileron"))
                {
                    Aileron = sm.DataDictionary["aileron"];
                }
                if (sm.DataDictionary.ContainsKey("elevator"))
                {
                    Elevator = sm.DataDictionary["elevator"];
                }
                if (sm.DataDictionary.ContainsKey("rudder"))
                {
                    Rudder = sm.DataDictionary["rudder"];
                }
                if (sm.DataDictionary.ContainsKey("throttle"))
                {
                    Throttle = sm.DataDictionary["throttle"];
                }
                Thread.Sleep(50);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
