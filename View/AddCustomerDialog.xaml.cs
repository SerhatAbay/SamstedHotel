using System;
using System.Windows;
using SamstedHotel.Model;
using SamstedHotel.Repos;

namespace SamstedHotel.View
{
    public partial class AddCustomerDialog : Window
    {
        // Public properties to access customer data
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string TLF { get; private set; }

        private readonly CustomerRepo _customerRepo;

        // Public constructor
        public AddCustomerDialog(string connectionString)
        {
            InitializeComponent();
            _customerRepo = new CustomerRepo(connectionString);
        }

        // Confirm button to collect customer data and close the dialog
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Indsamling af data fra tekstfelterne
            FirstName = FirstNameTextBox.Text;
            LastName = LastNameTextBox.Text;
            Email = EmailTextBox.Text;
            TLF = TLFTextBox.Text;

            // Validering af input
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(TLF))
            {
                MessageBox.Show("Alle felter skal være udfyldt.", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Opret ny kunde og gem den i databasen
                var newCustomer = new Customer
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Email = Email,
                    TLF = TLF
                };

                // Brug CustomerRepo til at tilføje den nye kunde til databasen
                _customerRepo.Add(newCustomer);  // Her kalder du din Add-metode

                // Bekræft om kunden blev oprettet korrekt
                MessageBox.Show("Kunde oprettet!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;  // Luk dialogen med succes
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Der opstod en fejl: {ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;  // Luk dialogen uden at gemme data
            }
        }

        // Cancel button to close the dialog without saving data
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;  // Close dialog without saving data
        }
    }
}
