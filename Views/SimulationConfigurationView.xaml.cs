using FlightExaminator.ViewModels;
using System;
using System.Windows;
using System.Windows.Forms;

namespace FlightExaminator.Views
{
    /// <summary>
    /// Interaction logic for SimulationConfigurationView.xaml
    /// </summary>
    public partial class SimulationConfigurationView : System.Windows.Controls.UserControl
    {
        private SimulationConfigurationViewModel vm;
        public SimulationConfigurationViewModel VM
        {
            get { return vm; }
            set
            {
                vm = value;
                DataContext = vm;
            }
        }

        public SimulationConfigurationView()
        {
            InitializeComponent();
        }

        private void SimulatorPathClick(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowser.ShowDialog();
                if (result == DialogResult.OK && !String.IsNullOrEmpty(folderBrowser.SelectedPath))
                {
                    vm.VM_SimulatorPath = folderBrowser.SelectedPath;
                }
            }
        }

        private void ConfigFileClick(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Config files (*.xml) (*.*)|*.*";
                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    vm.VM_ConfigFilePath = fileDialog.FileName;
                }
            }
        }

        private void FlightDataFileClick(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = "Data files (*.csv) (*.*)|*.*";
                DialogResult result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    vm.VM_FlightFilePath = fileDialog.FileName;
                }
            }
        }

        private void StartSimultorClick(object sender, RoutedEventArgs e)
        {
            vm.StartSimulation();
        }
    }
}
