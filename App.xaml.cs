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

          
            var MenuWindow = new View.MenuWindow();
            MenuWindow.Show();
        }

    }

}
