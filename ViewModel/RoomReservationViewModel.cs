using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamstedHotel.Model;
using SamstedHotel.Repos;

namespace SamstedHotel.ViewModel
{
    public class RoomReservationViewModel : INotifyPropertyChanged
    {
        private readonly ReservationRepo _reservationRepo;

        public ObservableCollection<Reservation> Reservations { get; set; }
        public Reservation SelectedReservation { get; set; }

        public RelayCommand AddReservationCommand { get; }
        public RelayCommand ClearReservationCommand { get; }

        public RoomReservationViewModel(ReservationRepo reservationRepo)
        {
            _reservationRepo = reservationRepo;
            Reservations = new ObservableCollection<Reservation>(_reservationRepo.GetAllReservations());

            AddReservationCommand = new RelayCommand(AddReservation);
            ClearReservationCommand = new RelayCommand(ClearReservation);
        }

        private void AddReservation(object parameter)
        {
            var newReservation = new Reservation
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(2),
                Room = new Room { RoomID = 1, RoomType = new RoomType { PricePerNight = 1200 } }
            };
            Reservations.Add(newReservation);
            _reservationRepo.AddReservation(newReservation);
        }

        private void ClearReservation(object parameter)
        {
            SelectedReservation = null; // Clear the selected reservation
            OnPropertyChanged(nameof(SelectedReservation));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

