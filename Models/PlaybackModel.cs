using FlightExaminator.Models.Interfaces;
using System.ComponentModel;

namespace FlightExaminator.Models
{
    public class PlaybackModel : IPlaybackModel
    {
        private SimulatorRunner runner;
        private int totalLocations;
        public int TotalLocations 
        { 
            get { return totalLocations; } 
            set
            {
                totalLocations = value;
                NotifyPropertyChanged("TotalLocations");
            }
        }

        public int NextDataLocation
        {
            get { return runner.NextDataLocation; }
            set 
            { 
                runner.NextDataLocation = value;
                NotifyPropertyChanged("NextDataLocation");
            }
        }

        private double playbackSpeed;
        public double PlaybackSpeed
        {
            get { return playbackSpeed; }
            set
            {
                playbackSpeed = value;
                if (value == 0)
                {
                    Play = false;
                }
                if (value == 2)
                {
                    runner.MiliSecondsBetweenIterations = 50;
                }
                else
                {
                    runner.MiliSecondsBetweenIterations = (int)(100 * (1 / playbackSpeed));
                }
                NotifyPropertyChanged("PlaybackSpeed");
            }
        }

        public bool Play
        {
            get
            {
                return runner.InsertData;
            }
            set
            {
                if (runner.Ready)
                {
                    TotalLocations = runner.GetTotalLocations();
                    runner.InsertData = value;
                    runner.InsertDataTask();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public PlaybackModel(int totalLocations, SimulatorRunner runner)
        {
            this.runner = runner;
            TotalLocations = totalLocations;
            Play = false;
            PlaybackSpeed = 1;
            NextDataLocation = 0;
        }
    }
}
