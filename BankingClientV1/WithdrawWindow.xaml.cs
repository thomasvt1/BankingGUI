using System.Windows;
using Newtonsoft.Json.Linq;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for WithdrawWindow.xaml
    /// </summary>
    public partial class WithdrawWindow : Window
    {
        public WithdrawWindow()
        {
            InitializeComponent();

            double money = User.GetBalance();

            if (money < 10)
                _1.IsEnabled = false;
            if (money < 10)
                _7.IsEnabled = false;
            if (money < 20)
                _2.IsEnabled = false;
            if (money < 50)
                _3.IsEnabled = false;
            if (money < 100)
                _4.IsEnabled = false;
            if (money < 200)
                _5.IsEnabled = false;
            if (money < 500)
                _6.IsEnabled = false;

            if (!BillHandler.isWithdrawable(10))
                _1.IsEnabled = false;

            if (!BillHandler.isWithdrawable(20))
                _2.IsEnabled = false;

            if (!BillHandler.isWithdrawable(50))
                _3.IsEnabled = false;

            if (!BillHandler.isWithdrawable(100))
                _4.IsEnabled = false;

            if (!BillHandler.isWithdrawable(200))
                _5.IsEnabled = false;

            if (!BillHandler.isWithdrawable(500))
                _6.IsEnabled = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}

