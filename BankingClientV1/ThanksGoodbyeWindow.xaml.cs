using System;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    /// <summary>
    /// Interaction logic for ThanksGoodbyeWindow.xaml
    /// </summary>
    public partial class ThanksGoodbyeWindow : Window
    {
        public ThanksGoodbyeWindow()
        {
            InitializeComponent();

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
            WindowSwitcher.ResetUserState();
            User.ClearUser();
        }
    }
}