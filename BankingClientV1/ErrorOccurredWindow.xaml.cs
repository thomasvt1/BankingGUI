using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for ErrorOccurredWindow.xaml
    /// </summary>
    public partial class ErrorOccurredWindow : Window
    {
        public ErrorOccurredWindow()
        {
            InitializeComponent();

            PLACEHOLDER.Content = User.GetError();
            User.SetBalanceInput("");

            StartCountdown(10);
        }

        private void StartCountdown(int seconds)
        {
            Thread thread = new Thread(() =>
            {
                for (int i = 0; i < seconds; i++)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        TIME.Content = "T e r u g  i n: " + (seconds - i);
                    }));
                    Thread.Sleep(1000);
                }
                FinalAction();
            });
            thread.Start();
        }

        private void FinalAction()
        {
            if (User.GetTag().Length == 0)
            {
                WindowSwitcher.ResetUserState();
                User.ClearUser();
            }
            else
                WindowSwitcher.UserToChoice();
        }
    }
}
