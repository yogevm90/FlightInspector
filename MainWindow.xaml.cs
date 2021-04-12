using System;
using System.Collections.Generic;
using System.IO;
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

namespace FlightExaminator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SimulatorRunner sm;
        public MainWindow()
        {
            InitializeComponent();
            string currentPath = Directory.GetCurrentDirectory();
            string configFile = currentPath + @"\ConfigurationFiles\playback_small.xml";
            string simulatorPath = @"C:\Program Files\FlightGear 2018.3.5";
            string flightData = currentPath + @"\FlightDataExamples\reg_flight.csv";
            sm = new SimulatorRunner(configFile, simulatorPath, flightData, 5400);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sm.InsertDataTask();
        }
    }
}
