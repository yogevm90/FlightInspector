using FlightExaminator.Models.Interfaces;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace FlightExaminator.Models
{
    public class PlaybackModel : IPlaybackModel
    {
        private SimulatorRunner runner;
        private int totalLocations;
        private int sleepTime;

        public int TotalLocations 
        { 
            get { return totalLocations; } 
            set
            {
                totalLocations = value;
                NotifyPropertyChanged("TotalLocations");
            }
        }

        private int nextDataLocation;
        public int NextDataLocation
        {
            get { return nextDataLocation; }
            set 
            { 
                nextDataLocation = value;
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
                    sleepTime = 100;
                }
                if (value == 2)
                {
                    sleepTime = 50;
                }
                else
                {
                    sleepTime = (int)(100 * (1 / playbackSpeed));
                }
                NotifyPropertyChanged("PlaybackSpeed");
            }
        }

        private bool play;
        public bool Play
        {
            get
            {
                return play;
            }
            set
            {
                if (runner.Ready)
                {
                    play = true;
                    TotalLocations = runner.GetTotalLocations();
                    InsertDataTask();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public PlaybackModel(SimulatorRunner runner)
        {
            this.runner = runner;
            Play = false;
            sleepTime = 100;
            PlaybackSpeed = 1;
            NextDataLocation = 0;
        }

        public void InsertDataTask()
        {
            if (!runner.Ready) return;
            Thread thread = new Thread(InsertDataToSimulator);
            thread.Start();
        }

        private void InsertDataToSimulator()
        {
            while (Play)
            {
                runner.SendData(NextDataLocation);
                NextDataLocation++;
                if (NextDataLocation >= TotalLocations)
                {
                    Play = false;
                    NextDataLocation = 0;
                }
                Thread.Sleep(sleepTime);
            }
        }
    }
}
