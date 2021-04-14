using FlightExaminator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightExaminator.Views
{
    /// <summary>
    /// Interaction logic for PlaybackView.xaml
    /// </summary>
    public partial class PlaybackView : UserControl
    {
        private PlaybackViewModel vm;
        public PlaybackViewModel VM
        {
            get { return vm; }
            set
            {
                vm = value;
                DataContext = vm;
            }
        }
        public PlaybackView()
        {
            InitializeComponent();
        }

        private void OnPlayClick(object sender, RoutedEventArgs e)
        {
            if (vm != null) vm.VM_Play = true;
        }

        private void OnPauseClick(object sender, RoutedEventArgs e)
        {
            if (vm != null) vm.VM_Play = false;
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            if (vm != null) vm.VM_NextDataLocation = 0;
        }

        private void OnSlowerClick(object sender, RoutedEventArgs e)
        {
            if (vm != null) vm.VM_PlaybackSpeed -= 0.1;
        }

        private void OnFasterClick(object sender, RoutedEventArgs e)
        {
            if (vm != null) vm.VM_PlaybackSpeed += 0.1;
        }
    }
}
