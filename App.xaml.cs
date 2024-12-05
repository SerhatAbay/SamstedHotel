using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SamstedHotel.View;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SamstedHotel
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Simpel kode til at hente connection string og teste forbindelsen
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string? connectionString = config.GetConnectionString("DefaultConnection"); 

            // Åbn første view
            var mainWindow = new FørsteView();
            mainWindow.Show();
        }

    }
}
