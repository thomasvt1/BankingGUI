using System;
using System.Windows;

namespace BankingClientV1
{
    static class WindowSwitcher
    {
        public static Confirm Confirm;
        public static Choice Choice;
        public static PinWindow PinWindow;
        public static Closing Closing;
        public static BalanceWindow BalanceWindow;
        public static AdvWithdrawWindow AdvWithdrawWindow;
        public static ReceiptWindow ReceiptWindow;
        public static WithdrawWindow WithdrawWindow;
        public static PleaseWaitWindow PleaseWaitWindow;
        public static ThanksGoodbyeWindow ThanksGoodbyeWindow;
        public static ErrorOccurredWindow ErrorOccurredWindow;
        public static Return Return;

        static private Window GetWindow(String s)
        {
            foreach (Window w in Application.Current.Windows)
                if (w.Title.Equals(s))
                    return w;

            return null;
        }

        public static void UserToChoice()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window w in Application.Current.Windows)
                    if (!w.Title.Equals("MainWindow") && !w.Title.Equals("Choice") && w.Title.Length != 0)
                    {
                        Console.WriteLine("Closing: " + w.Title);
                        w.Close();
                    }
            }));
        }

        private static void ClearMemory()
        {
            Confirm = null;
            Choice = null;
            PinWindow = null;
            Closing = null;
            BalanceWindow = null;
            AdvWithdrawWindow = null;
            ReceiptWindow = null;
            ReceiptWindow = null;
            ReceiptWindow = null;
            PleaseWaitWindow = null;
            ThanksGoodbyeWindow = null;
            ErrorOccurredWindow = null;
            Return = null;
        }


        public static void ResetUserState()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window w in Application.Current.Windows)
                    if (!w.Title.Equals("MainWindow") && w.Title.Length != 0)
                    {
                        Console.WriteLine("Closing: " + w.Title);
                        w.Close();
                    }
            }));
            ClearMemory();
        }

        /*
         * MAKE SURE RUNNING IN A STA THREAD
         */
        [STAThread]
        public static Window GetActiveWindow()
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.IsActive)
                {
                    Console.WriteLine("Open window: " + w.Title);
                    return w;
                }
            }
                return null;
        }

        public static void OpenReceiptWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window w = GetWindow("ReceiptWindow");

                if (w != null)
                    w.Activate();
                else
                {
                    ReceiptWindow = new ReceiptWindow();
                    ReceiptWindow.Show();
                }
            }));
        }


        public static void OpenPinWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window w = GetWindow("PinWindow");

                if (w != null)
                    w.Activate();
                else
                {
                    PinWindow = new PinWindow();
                    PinWindow.Show();
                }
            }));
        }

        public static void OpenPleaseWaitWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window x = GetWindow("PleaseWaitWindow");

                if (x != null)
                    x.Activate();
                else
                {
                    PleaseWaitWindow = new PleaseWaitWindow();
                    PleaseWaitWindow.Show();
                }
            }));
        }

        public static void OpenErrorOccurredWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window x = GetWindow("ErrorOccurredWindow");

                if (x != null)
                    x.Activate();
                else
                {
                    ErrorOccurredWindow = new ErrorOccurredWindow();
                    ErrorOccurredWindow.Show();
                }
            }));
        }

        public static void OpenChoice()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window x = GetWindow("Choice");

                if (x != null)
                    x.Activate();
                else
                {
                    Choice = new Choice();
                    Choice.Show();
                }
            }));
        }

        public static void OpenClosing()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window y = GetWindow("Closing");

                if (y != null)
                    y.Activate();
                else
                {
                    Closing = new Closing();
                    Closing.Show();
                }
            }));
        }

        public static void OpenWithdrawWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window a = GetWindow("WithdrawWindow");

                if (a != null)
                    a.Activate();
                else
                {
                    WithdrawWindow = new WithdrawWindow();
                    WithdrawWindow.Show();
                }
            }));
        }

        public static void OpenThanksGoodbyeWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window b = GetWindow("ThanksGoodbyeWindow");

                if (b != null)
                    b.Activate();
                else
                {
                    ThanksGoodbyeWindow = new ThanksGoodbyeWindow();
                    ThanksGoodbyeWindow.Show();
                }
            }));
        }

        public static void OpenAdvWithdrawWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window b = GetWindow("AdvWithdrawWindow");

                if (b != null)
                    b.Activate();
                else
                {
                    AdvWithdrawWindow = new AdvWithdrawWindow();
                    AdvWithdrawWindow.Show();
                }
            }));
        }

        public static void OpenBalanceWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window c = GetWindow("BalanceWindow");

                if (c != null)
                    c.Activate();
                else
                {
                    BalanceWindow = new BalanceWindow();
                    BalanceWindow.Show();
                }
            }));
        }

        public static void OpenReturn()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window d = GetWindow("Return");

                if (d != null)
                    d.Activate();
                else
                {
                    Return = new Return();
                    Return.Show();
                }
            }));
        }

        public static void OpenConfirm()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window e = GetWindow("Confirm");

                if (e != null)
                    e.Activate();
                else
                {
                    Confirm = new Confirm();
                    Confirm.Show();
                }
            }));
        }

    }
}
