using Newtonsoft.Json.Linq;
using System;
using System.Windows;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for BalanceWindow.xaml
    /// </summary>
    public partial class BalanceWindow : Window
    {
        public BalanceWindow()
        {
            InitializeComponent();

            BALANCE.Content = "$" + User.GetBalance();

        }


    }
}
