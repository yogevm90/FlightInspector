using FlightExaminator.Models;
using FlightExaminator.ViewModels;
using System.Windows.Controls;

namespace FlightExaminator.Views
{
    /// <summary>
    /// Interaction logic for FlightDataView.xaml
    /// </summary>
    public partial class FlightDataView : UserControl
    {
        private FlightDataViewModel vm;
        public FlightDataViewModel VM 
        {
            get { return vm; }
            set
            {
                vm = value;
                DataContext = vm;
            }
        }
        public FlightDataView()
        {
            InitializeComponent();
        }
    }
}
