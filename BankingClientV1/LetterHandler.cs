using System;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class LetterHandler
    {

        String[] back_blacklist =
        {
            "MainWindow",
            "NoArduino",
            "NoNetwork",
            "Choice",
            "PinWindow",
            "PleaseWaitWindow",
            "ThanksGoodbyeWindow",
            "ErrorOccurredWindow",
            "Closing"
        };

        String[] abort_blacklist =
        {
            "MainWindow",
            "PleaseWaitWindow",
            "ThanksGoodbyeWindow",
            "ErrorOccurredWindow",
            "NoArduino",
            "NoNetwork",
            "Closing"
        };


        public void LetterCancel()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window w = WindowSwitcher.GetActiveWindow();

                if (WindowInBlacklist(w, abort_blacklist))
                    return;

                WindowSwitcher.OpenClosing();
                WindowSwitcher.Closing.Show();
                User.inputBlocked = true;
                User.ClearUser();

                Thread thread = new Thread(() =>
                {
                    Thread.Sleep(2000);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        WindowSwitcher.ResetUserState();
                        User.inputBlocked = false;
                    }));
                });
                thread.Start();

            }));
        }

        

        Boolean WindowInBlacklist(Window w, String[] blacklist)
        {
            String title = w.Title;

            foreach (String s in blacklist)
            {
                if (title.Equals(s))
                    return true;
            }
            return false;
        }

        public void LetterBack()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Window w = WindowSwitcher.GetActiveWindow();

                if (WindowInBlacklist(w, back_blacklist))
                    return;

                if (w != null)
                    w.Close();
            }));
        }
    }
}