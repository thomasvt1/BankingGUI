using System;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class ZombieKiller
    {

        public void Start()
        {

            



            Thread thread = new Thread(() =>
            {
                Boolean running = true;
                while (running)
                {
                    Thread.Sleep(20000);

                    if (Application.Current == null)
                        KillApplication();

                    Application.Current.Dispatcher.Invoke(new Action(() => {
                        int count = Application.Current.Windows.Count;
                        System.Diagnostics.Debug.WriteLine("KILLCHECK: " + count);
                        if (count == 0)
                            running = KillApplication();
                    }));
                }

            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        Boolean KillApplication()
        {
            Environment.Exit(1);
            Application.Current.Shutdown();
            return false;
        }
    }
}