using System.Windows;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("C# loader is LOADED");
            //on start stuff here
            base.OnStartup(e);
            new Loader().Start();
            //or here, where you find it more appropriate
        }
    }
}