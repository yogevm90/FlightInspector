using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightExaminator.Models.Interfaces
{
    public interface IPlaybackModel : INotifyPropertyChanged
    {
        int NextDataLocation { get; set; }
        double PlaybackSpeed { get; set; }
        bool Play { get; set; }
        int TotalLocations { get; }
    }
}
