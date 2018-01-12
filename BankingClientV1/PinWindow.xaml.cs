
using System.Windows;
using System.Windows.Controls;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for PinWindow.xaml
    /// </summary>
    public partial class PinWindow : Window
    {
        public PinWindow()
        {
            InitializeComponent();
        }

        public Label GetPinProgress()
        {
            return PinProgress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Reset session button

        }


    }
}
