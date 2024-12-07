using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SamstedHotel.View;


namespace SamstedHotel.ViewModel
{
    using System.Windows;
    using System.Windows.Input;
  

    namespace SamstedHotel.ViewModel
    {
        public class MenuViewModel
        {

            public ICommand NavigateToRoomReservationCommand { get; }
            public ICommand NavigateToCourseRoomReservationCommand { get; }
            public ICommand NavigateToViewDeleteReservationsCommand { get; }

            public MenuViewModel()
            {
                NavigateToRoomReservationCommand = new RelayCommand(OpenRoomReservation);
                NavigateToCourseRoomReservationCommand = new RelayCommand(OpenCourseRoomReservation);
                NavigateToViewDeleteReservationsCommand = new RelayCommand(OpenViewDeleteReservations);
            }

            private void OpenRoomReservation(object parameter)
            {
                var roomReservationWindow = new RoomReservationWindow();
                roomReservationWindow.ShowDialog();
            }

            private void OpenCourseRoomReservation(object parameter)
            {
                var courseRoomReservationWindow = new CourseRoomReservationWindow();
                courseRoomReservationWindow.ShowDialog();
            }

            private void OpenViewDeleteReservations(object parameter)
            {
                var viewDeleteReservationsWindow = new ViewDeleteReservationWindow();
                viewDeleteReservationsWindow.ShowDialog();
            }
        }
    }
}

