using System;
using System.Windows.Input;

namespace SamstedHotel.ViewModel
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        // Constructor
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            // Hvis canExecute er null, så returner altid true (kommandoen kan udføres)
            _canExecute = canExecute ?? (() => true);
        }

        // CanExecute metoden bestemmer, om kommandoen kan udføres
        public bool CanExecute(object parameter) => _canExecute();

        // Execute metoden udfører kommandoen
        public void Execute(object parameter) => _execute();

        // Event der bruges til at opdatere UI'en om at kommandoens status kan have ændret sig
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;   // Når systemet foreslår en ændring
            remove => CommandManager.RequerySuggested -= value;  // Fjern ændringshændelsen
        }

        // Når CanExecute ændrer sig, bør vi fortælle systemet at det skal tjekke igen
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
