using FlightExaminator.Models.Interfaces;
using System;
using System.ComponentModel;

namespace FlightExaminator.ViewModels
{
    public class PlaybackViewModel : INotifyPropertyChanged
    {
        private IPlaybackModel model;
        public PlaybackViewModel(IPlaybackModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            VM_TotalLocations = model.TotalLocations;
        }

        public int VM_TotalLocations { get; }

        public int VM_NextDataLocation
        {
            get { return model.NextDataLocation; }
            set
            {
                if (value >= 0 && value <= model.TotalLocations)
                {
                    model.NextDataLocation = value;
                }
            }
        }

        public double VM_PlaybackSpeed
        {
            get { return model.PlaybackSpeed; }
            set
            {
                if (value >= 0 && value <= 2)
                {
                    model.PlaybackSpeed = value;
                }
            }
        }

        public bool VM_Play
        {
            get { return model.Play; }
            set { model.Play = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
