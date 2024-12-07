
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using global::SamstedHotel.Model;
using global::SamstedHotel.Repos;
using SamstedHotel.Model;
using SamstedHotel.Repos;

namespace SamstedHotel.ViewModel
{
    public class ViewDeleteReservationViewModel : INotifyPropertyChanged
    {
        private readonly ReservationRepo _reservationRepo;

        public ObservableCollection<Reservation> Reservations { get; set; }
        public Reservation SelectedReservation { get; set; }

        public ICommand DeleteReservationCommand { get; }

        public ViewDeleteReservationViewModel(ReservationRepo reservationRepo)
        {
            _reservationRepo = reservationRepo;

            // Hent alle reservationer fra repo
            Reservations = new ObservableCollection<Reservation>(_reservationRepo.GetAllReservations());

            // Initialiser kommandoen
            DeleteReservationCommand = new RelayCommand(DeleteReservation, CanDeleteReservation);
        }

        private void DeleteReservation(object parameter)
        {
            if (SelectedReservation != null)
            {
                // Slet fra repo
                _reservationRepo.DeleteReservation(SelectedReservation.ReservationID);

                // Fjern fra ObservableCollection
                Reservations.Remove(SelectedReservation);

                // Ryd den valgte reservation
                SelectedReservation = null;
                OnPropertyChanged(nameof(SelectedReservation));
            }
        }

        private bool CanDeleteReservation(object parameter)
        {
            return SelectedReservation != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
