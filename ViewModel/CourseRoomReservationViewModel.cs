
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using global::SamstedHotel.Model;
using SamstedHotel.Model;

namespace SamstedHotel.ViewModel
{
    public class CourseRoomViewModel : INotifyPropertyChanged
    {
        public DateTime StartDate { get; set; } = DateTime.Now;
        public string SelectedEventPackage { get; set; }
        public decimal TotalPrice { get; set; }
        public Customer CurrentCustomer { get; set; } = new Customer();
        public ObservableCollection<string> EventPackages { get; set; } = new ObservableCollection<string> { "Package 1", "Package 2", "Package 3" };

        public ICommand AddReservationCommand { get; }
        public ICommand ClearAllCommand { get; }
        public ICommand BackCommand { get; }

        public CourseRoomViewModel()
        {
            AddReservationCommand = new RelayCommand(AddReservation, CanAddReservation);
            ClearAllCommand = new RelayCommand(ClearAll);
            BackCommand = new RelayCommand(BackToMenu);
        }

        private void AddReservation(object parameter)
        {
            // Logik til at oprette reservation, fx gemme i databasen eller sende videre til repository
            TotalPrice = CalculateTotalPrice(SelectedEventPackage);
        }

        private bool CanAddReservation(object parameter)
        {
            return !string.IsNullOrWhiteSpace(CurrentCustomer.FirstName) &&
                   !string.IsNullOrWhiteSpace(CurrentCustomer.LastName) &&
                   !string.IsNullOrWhiteSpace(SelectedEventPackage);
        }

        private void ClearAll(object parameter)
        {
            StartDate = DateTime.Now;
            SelectedEventPackage = null;
            TotalPrice = 0;
            CurrentCustomer = new Customer();
            OnPropertyChanged(nameof(StartDate));
            OnPropertyChanged(nameof(SelectedEventPackage));
            OnPropertyChanged(nameof(TotalPrice));
            OnPropertyChanged(nameof(CurrentCustomer));
        }

        private void BackToMenu(object parameter)
        {
            // Naviger tilbage til menuen
        }

        private decimal CalculateTotalPrice(string eventPackage)
        {
            // Simpel logik til beregning af pris baseret på eventPackage
            return eventPackage switch
            {
                "Package 1" => 1000,
                "Package 2" => 2000,
                "Package 3" => 3000,
                _ => 0
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


