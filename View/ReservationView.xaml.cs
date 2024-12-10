using System.Windows;
using SamstedHotel.ViewModel;

namespace SamstedHotel.View
{
    public partial class ReservationView : Window
    {
        public ReservationView(string connectionString)
        {
            InitializeComponent();

            // Initialize the ViewModel and set it as the DataContext of the View
            var viewModel = new ReservationViewModel(connectionString);
            this.DataContext = viewModel;
        }
    }
}
