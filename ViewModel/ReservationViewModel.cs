using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using SamstedHotel.ViewModel;

namespace SamstedHotel.ViewModel
{
    public class ReservationViewModel : BaseViewModel
    {
        private readonly string _connectionString;
        private ReservationRepo _reservationRepo;
        private ObservableCollection<Reservation> _reservations;
        private DateTime _startDate;
        private DateTime _endDate;

        // Constructor
        public ReservationViewModel(string connectionString)
        {
            _connectionString = App.ConnectionString;
            _reservationRepo = new ReservationRepo(_connectionString);

            LoadReservations();

            // Initialize commands
            BookReservationCommand = new RelayCommand(BookReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation);
            EditReservationCommand = new RelayCommand(EditReservation);
            SaveToCsvCommand = new RelayCommand(SaveReservationsToCsv);
        }

        // ObservableCollection to bind to DataGrid in XAML
        public ObservableCollection<Reservation> Reservations
        {
            get { return _reservations; }
            set { _reservations = value; OnPropertyChanged(); }
        }

        // Properties for Start and End Date
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged(); }
        }

        // Commands for different reservation actions
        public ICommand BookReservationCommand { get; set; }
        public ICommand CancelReservationCommand { get; set; }
        public ICommand EditReservationCommand { get; set; }
        public ICommand SaveToCsvCommand { get; set; }

        // Method to load reservations from the repository
        private void LoadReservations()
        {
            var reservations = _reservationRepo.GetAllReservations();
            Reservations = new ObservableCollection<Reservation>(reservations);
        }

        // Method to handle booking a reservation
        private void BookReservation()
        {
            // Example logic for booking a new reservation
            var newReservation = new Reservation
            {
                CustomerID = 1, // Assume customer ID is 1 for now, or retrieve dynamically
                StartDate = StartDate,
                EndDate = EndDate,
                TotalAmount = 1000, // Example amount, could be calculated based on room type
                Status = "Booked"
            };

            _reservationRepo.AddReservation(newReservation);
            LoadReservations(); // Refresh the list after adding the reservation
        }

        // Method to handle canceling a reservation
        private void CancelReservation()
        {
            // Logic to cancel the selected reservation
            var selectedReservation = Reservations.FirstOrDefault(r => r.Status == "Booked");
            if (selectedReservation != null)
            {
                _reservationRepo.DeleteReservation(selectedReservation.ReservationID);
                LoadReservations(); // Refresh the list after deletion
            }
        }

        // Method to handle editing a reservation
        private void EditReservation()
        {
            // Logic to edit the selected reservation (for simplicity, just an example)
            var selectedReservation = Reservations.FirstOrDefault(r => r.Status == "Booked");
            if (selectedReservation != null)
            {
                selectedReservation.StartDate = StartDate; // Example update
                selectedReservation.EndDate = EndDate; // Example update
                _reservationRepo.UpdateReservation(selectedReservation);
                LoadReservations(); // Refresh the list after update
            }
        }

        // Method to save reservations to a CSV file (simplified)
        private void SaveReservationsToCsv()
        {
            // Logic to export reservations to CSV
            var reservations = Reservations.Select(r => new
            {
                r.ReservationID,
                r.CustomerID,
                r.StartDate,
                r.EndDate,
                r.TotalAmount,
                r.Status
            });

            // Code to save reservations to CSV (you could use CSVHelper or implement manually)
            // For now, this is just a placeholder
            string filePath = "reservations.csv";
            var sb = new StringBuilder();
            sb.AppendLine("ReservationID,CustomerID,StartDate,EndDate,TotalAmount,Status");
            foreach (var reservation in reservations)
            {
                sb.AppendLine($"{reservation.ReservationID},{reservation.CustomerID},{reservation.StartDate},{reservation.EndDate},{reservation.TotalAmount},{reservation.Status}");
            }

            System.IO.File.WriteAllText(filePath, sb.ToString());
        }
    }
}
