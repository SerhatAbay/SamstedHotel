using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SamstedHotel.Model;
using SamstedHotel.Repos;
using SamstedHotel.View;

namespace SamstedHotel.ViewModel
{
    public class ReservationViewModel : BaseViewModel
    {
        private readonly string _connectionString;
        private readonly ReservationRepo _reservationRepo;
        private readonly CourseRoomRepo _courseRoomRepo;
        private readonly RoomRepo _roomRepo;

        private ObservableCollection<Reservation> _reservations; 
      
        public ObservableCollection<Reservation> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;  // Set the value of _reservations
                OnPropertyChanged();  // Notify the UI to refresh
            }
        }


        private ObservableCollection<SelectableItem<Room>> _availableRooms;
        public ObservableCollection<SelectableItem<Room>> AvailableRooms
        {
            get => _availableRooms;
            set { _availableRooms = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SelectableItem<CourseRoom>> _availableCourseRooms;
        public ObservableCollection<SelectableItem<CourseRoom>> AvailableCourseRooms
        {
            get => _availableCourseRooms;
            set { _availableCourseRooms = value; OnPropertyChanged(); }
        }

        private DateTime _startDate;
        private DateTime _endDate;

        // Constructor
        public ReservationViewModel(string connectionString)
        {
            _connectionString = connectionString;
            _courseRoomRepo = new CourseRoomRepo(_connectionString);
            _roomRepo = new RoomRepo(_connectionString);
            _reservationRepo = new ReservationRepo(_connectionString);

            _startDate = DateTime.Now;
            _endDate = DateTime.Now.AddDays(1);

            _reservations = new ObservableCollection<Reservation>();

            LoadReservations();
            LoadRoomsAndCourseRooms();

            // Initialize commands
            BookReservationCommand = new RelayCommand(BookReservation);
            CancelReservationCommand = new RelayCommand(CancelReservation);
            EditReservationCommand = new RelayCommand(EditReservation);
            SaveToCsvCommand = new RelayCommand(SaveReservationsToCsv);
        }  

        // Properties for Start and End Date
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        // Commands for different reservation actions
        public ICommand BookReservationCommand { get; }
        public ICommand CancelReservationCommand { get; }
        public ICommand EditReservationCommand { get; }
        public ICommand SaveToCsvCommand { get; }

        // Method to load reservations from the repository
        private void LoadReservations()
        {
            // Hent alle reservationer fra databasen
            var reservations = _reservationRepo.GetAllReservations(); // Sørg for, at denne metode returnerer en liste af Reservation-objekter

            // Opdater ObservableCollection med de hentede data
            Reservations = new ObservableCollection<Reservation>(reservations);
        }


        private void LoadRoomsAndCourseRooms()
        {
            // Filtrer værelserne, så kun de tilgængelige i den valgte periode vises
            var rooms = _roomRepo.GetAll()
                .Where(room => _roomRepo.IsRoomAvailable(room.RoomID, StartDate, EndDate))  // Tjek om værelset er ledigt i den valgte periode
                .Select(room => new SelectableItem<Room>(room))  // Opret SelectableItem for binding
                .ToList();

            // Filtrer kursuslokalerne, så kun de tilgængelige i den valgte periode vises
            var courseRooms = _courseRoomRepo.GetAll()
                .Where(courseRoom => _courseRoomRepo.IsCourseRoomAvailable(courseRoom.CourseRoomID, StartDate, EndDate))  // Tjek om kursuslokalet er ledigt
                .Select(courseRoom => new SelectableItem<CourseRoom>(courseRoom))  // Opret SelectableItem for binding
                .ToList();

            // Opdater ObservableCollection for DataGrid binding
            AvailableRooms = new ObservableCollection<SelectableItem<Room>>(rooms);
            AvailableCourseRooms = new ObservableCollection<SelectableItem<CourseRoom>>(courseRooms);
        }

        // Method to handle booking a reservation
        private void BookReservation()
        {
            try
            {
                // Åbn dialog for at tilføje en ny kunde
                var addCustomerDialog = new AddCustomerDialog();

                if (addCustomerDialog.ShowDialog() == true)
                {
                    // Opret ny kunde
                    var newCustomer = new Customer
                    {
                        FirstName = addCustomerDialog.FirstName,
                        LastName = addCustomerDialog.LastName,
                        Email = addCustomerDialog.Email,
                        TLF = addCustomerDialog.TLF
                    };

                    // Tilføj kunde til databasen via CustomerRepo
                    var customerRepo = new CustomerRepo(_connectionString);
                    customerRepo.Add(newCustomer);

                    // Vælg de værelser og kursuslokaler, der er valgt af brugeren
                    var selectedRooms = AvailableRooms.Where(r => r.IsSelected).Select(r => r.Item).ToList();
                    var selectedCourseRooms = AvailableCourseRooms.Where(cr => cr.IsSelected).Select(cr => cr.Item).ToList();

                    // Tjek om der er valgte værelser eller kursuslokaler
                    if (!selectedRooms.Any() && !selectedCourseRooms.Any())
                        throw new InvalidOperationException("Du skal vælge mindst ét værelse eller kursuslokale.");

                    // Saml de valgte værelser og kursuslokaler i én kommasepareret liste
                    var selectedRoomNames = selectedRooms.Select(room => room.RoomType.Name).ToList();
                    var selectedCourseRoomNames = selectedCourseRooms.Select(cr => cr.CourseRoomName).ToList();
                    var allSelectedItems = selectedRoomNames.Concat(selectedCourseRoomNames).ToList();
                    string bookingType = string.Join(", ", allSelectedItems);  // Komma-separeret streng af valgte værelser og kursuslokaler

                    // Beregn det samlede beløb
                    decimal totalAmount = 0;

                    // Beregn beløbet for værelser
                    foreach (var room in selectedRooms)
                    {
                        if (room?.RoomType != null)  // Tjek om RoomType ikke er null
                        {
                            totalAmount += room.RoomType.PricePerNight * (EndDate - StartDate).Days;
                        }
                    }

                    // Beregn beløbet for kursuslokaler
                    foreach (var courseRoom in selectedCourseRooms)
                    {
                        if (courseRoom != null)
                        {
                            totalAmount += courseRoom.TotalPrice * (EndDate - StartDate).Days;
                        }
                    }

                    if (totalAmount <= 0)
                        throw new InvalidOperationException("Der er opstået en fejl i beregningen af beløbet.");

                    // Opret reservationen
                    var newReservation = new Reservation
                    {
                        CustomerID = newCustomer.CustomerID,  // Bruger CustomerID af den nye kunde
                        StartDate = StartDate,
                        EndDate = EndDate,
                        TotalAmount = totalAmount, // Brug den beregnede totalAmount
                        Status = "Booked",
                        BookingType = bookingType // Gem de valgte værelser og kursuslokaler som BookingType
                    };

                    // Valider datoer
                    if (StartDate >= EndDate)
                        throw new InvalidOperationException("Startdato skal være før slutdato.");

                    // Tilføj reservationen til databasen
                    _reservationRepo.AddReservation(newReservation);

                    // Opdater reservationslisten
                    LoadReservations(); // Metode der henter reservationerne fra databasen

                    // Bekræft booking
                    Console.WriteLine("Reservation er gennemført.");
                }
                else
                {
                    // Hvis dialogen blev afbrudt (dvs. brugeren annullerede)
                    Console.WriteLine("Booking blev afbrudt af brugeren.");
                }
            }
            catch (Exception ex)
            {
                // Vis fejlbesked
                Console.WriteLine($"Fejl ved booking: {ex.Message}");
            }
        }

        // Method to handle canceling a reservation
        private void CancelReservation()
        {
            try
            {
                var selectedReservation = Reservations.FirstOrDefault(r => r.Status == "Booked");
                if (selectedReservation != null)
                {
                    _reservationRepo.DeleteReservation(selectedReservation.ReservationID);
                    LoadReservations(); // Refresh the list after deletion
                }
            }
            catch (Exception ex)
            {
                // Handle exception (show error to user or log)
                Console.WriteLine($"Error canceling reservation: {ex.Message}");
            }
        }

        // Method to handle editing a reservation
        private void EditReservation()
        {
            try
            {
                var selectedReservation = Reservations.FirstOrDefault(r => r.Status == "Booked");
                if (selectedReservation != null)
                {
                    selectedReservation.StartDate = StartDate;
                    selectedReservation.EndDate = EndDate;

                    if (selectedReservation.StartDate >= selectedReservation.EndDate)
                    {
                        throw new InvalidOperationException("Start date must be before the end date.");
                    }

                    _reservationRepo.UpdateReservation(selectedReservation);
                    LoadReservations(); // Refresh the list after update
                }
            }
            catch (Exception ex)
            {
                // Handle exception (show error to user or log)
                Console.WriteLine($"Error editing reservation: {ex.Message}");
            }
        }

        // Method to save reservations to a CSV file
        private void SaveReservationsToCsv()
        {
            try
            {
                var reservations = Reservations.Select(r => new
                {
                    r.ReservationID,
                    r.CustomerID,
                    r.StartDate,
                    r.EndDate,
                    r.TotalAmount,
                    r.Status
                });

                string filePath = "reservations.csv";
                var sb = new StringBuilder();
                sb.AppendLine("ReservationID,CustomerID,StartDate,EndDate,TotalAmount,Status");

                foreach (var reservation in reservations)
                {
                    sb.AppendLine($"{reservation.ReservationID},{reservation.CustomerID},{reservation.StartDate:yyyy-MM-dd},{reservation.EndDate:yyyy-MM-dd},{reservation.TotalAmount},{reservation.Status}");
                }

                System.IO.File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine("Reservations successfully exported to CSV.");
            }
            catch (Exception ex)
            {
                // Handle exception (show error to user or log)
                Console.WriteLine($"Error saving reservations to CSV: {ex.Message}");
            }
        }

        // Method to refresh available course rooms based on the selected dates
        private void RefreshAvailableCourseRooms()
        {
            if (StartDate == default || EndDate == default) return;

            var courseRooms = _courseRoomRepo.GetAll()
                .Where(courseRoom => _courseRoomRepo.IsCourseRoomAvailable(courseRoom.CourseRoomID, StartDate, EndDate))
                .Select(courseRoom => new SelectableItem<CourseRoom>(courseRoom))
                .ToList();

            AvailableCourseRooms = new ObservableCollection<SelectableItem<CourseRoom>>(courseRooms);
        }
    }
}
