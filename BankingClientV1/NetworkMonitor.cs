using System;
using System.Net;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class NetworkMonitor
    {

        NoNetwork nn;

        public void Start()
        {
            Thread thread = new Thread(() =>
            {
                Boolean running = true;
                while (running)
                {
                    System.Diagnostics.Debug.WriteLine("Performing network test...");

                    String response = new NewWebHandler().PingBill();

                    if (response.Contains("connection error"))
                    {
                        SetOutOfOrder();
                        Console.WriteLine(response);
                    }
                    else
                        User.offline = false;

                    if (!User.offline)
                        SetWorkingState();

                    if (!User.offline)
                        Thread.Sleep(20000);
                    else
                        Thread.Sleep(5000);
                }

            });
            //thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        void SetOutOfOrder()
        {
            User.offline = true;
            Console.WriteLine("WARNING: No connection to server");

            Application.Current.Dispatcher.Invoke(new Action(() => {
                if (nn == null)
                    nn = new NoNetwork();
                else if (!nn.IsFocused)
                    nn.Focus();
                try
                {
                    nn.Show();
                }
                catch (Exception e) { Console.WriteLine(e); }
            }));
        }

        void SetWorkingState()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.Title.Equals("NoNetwork"))
                        w.Close();
                }
            }));
        }
    }
}