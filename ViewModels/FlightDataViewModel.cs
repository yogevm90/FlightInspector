using FlightExaminator.Models.Interfaces;
using System;
using System.ComponentModel;

namespace FlightExaminator.ViewModels
{
    public class FlightDataViewModel : INotifyPropertyChanged
    {
        private IFlightDataModel model;
        private int joystickSize;
        public FlightDataViewModel(IFlightDataModel model, int joystickSize)
        {
            this.joystickSize = joystickSize;
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public double VM_Aileron
        {
            get 
            {
                return joystickSize + (joystickSize * model.Aileron); 
            }
        }
        public double VM_Elevator
        {
            get 
            {
                return joystickSize + (joystickSize * model.Elevator); 
            }
        }
        public double VM_Rudder
        {
            get 
            { 
                return model.Rudder; 
            }
        }
        public double VM_Throttle
        {
            get
            { 
                return model.Throttle; 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
