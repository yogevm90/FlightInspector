using FlightExaminator.Models;
using FlightExaminator.ViewModels;
using System.IO;
using System.Windows;

namespace FlightExaminator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static SimulatorRunner Runner;
        public MainWindow()
        {
            SimulatorRunner sm = new SimulatorRunner(5400);
            FlightDataViewModel flightDataVm = new FlightDataViewModel(new FlightDataModel(sm), 75);
            PlaybackViewModel playbackVm = new PlaybackViewModel(new PlaybackModel(sm));
            SimulationConfigurationViewModel simulationConfigurationVm = new SimulationConfigurationViewModel(new SimulationConfigurationModel(sm));
            InitializeComponent();
            FlightDataView.VM = flightDataVm;
            PlaybackView.VM = playbackVm;
            SimulationConfigurationView.VM = simulationConfigurationVm;
        }
    }
}
