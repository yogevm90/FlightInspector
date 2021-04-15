using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightExaminator.Models.Interfaces
{
    public interface IFlightDataModel : INotifyPropertyChanged
    {
        double Aileron { get; set; }
        double Elevator { get; set; }
        double Rudder { get; set; }
        double Throttle { get; set; }
    }
}
