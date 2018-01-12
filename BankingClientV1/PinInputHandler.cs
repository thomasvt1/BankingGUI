using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace BankingClientV1
{
    class PinInputHandler
    {

        String input;

        public PinInputHandler(String Xinput)
        {
            input = Xinput;
            PinWindowHandler();
        }

        private void UpdatePinProgress()
        {

            int pinlength = User.GetPin().Length;

            if (pinlength == 0)
                WindowSwitcher.PinWindow.PinProgress.Content = "- - - -";
            else if (pinlength == 1)
                WindowSwitcher.PinWindow.PinProgress.Content = "* - - -";
            else if (pinlength == 2)
                WindowSwitcher.PinWindow.PinProgress.Content = "* * - -";
            else if (pinlength == 3)
                WindowSwitcher.PinWindow.PinProgress.Content = "* * * -";
            else
            {
                WindowSwitcher.PinWindow.PinProgress.Content = "* * * *";
            }
        }


        private void UpdatePinProgress(Color color)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                WindowSwitcher.PinWindow.PinProgress.Foreground = new SolidColorBrush(color);
            }));

            User.inputBlocked = true;

            Thread thread = new Thread(() =>
            {
                Thread.Sleep(5000);
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WindowSwitcher.PinWindow.PinProgress.Content = "- - - -";
                    WindowSwitcher.PinWindow.PinProgress.Foreground = new SolidColorBrush(Colors.Black);
                    User.inputBlocked = false;
                }));
            });
            //thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        void PinWindowHandler()
        {
            int checkAlsGetal;

            if (int.TryParse(input, out checkAlsGetal))
                if (User.GetPin().Length != 4)
                    User.SetPin(User.GetPin() + input);

            if (input.Equals("*"))
            {
                if (User.GetPin().Length != 0)
                    User.SetPin(User.GetPin().Substring(0, User.GetPin().Length - 1));
            }

            UpdatePinProgress();

            if (input.Equals("#"))
            {
                if (User.GetPin().Length != 4)
                    return;

                JObject json = new WebHandler().GetBalance();


                if (json.GetValue("error") != null)
                {
                    UpdatePinProgress(Colors.Red);
                   
                    User.SetPin("");

                    String error = ((String ) json.GetValue("error")).ToLower();


                    if (error.Equals("pin incorrect"))
                    {
                        int triesremaining = 3 - ((int) json.GetValue("tries"));

                        WindowSwitcher.PinWindow.ERROR.Content = ("Nog X poging(en) over").Replace("X", triesremaining +"");  
                    }

                    if (error.Equals("card blocked"))
                    {
                        WindowSwitcher.PinWindow.ERROR.Content = "Uw pas is geblokeerd, neem aub contact op";

                        Thread thread = new Thread(() =>
                        {
                            Thread.Sleep(5000);
                            WindowSwitcher.OpenThanksGoodbyeWindow();
                        });
                        //thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();

                    }


                    WindowSwitcher.PinWindow.ERROR.Visibility = Visibility.Visible;
                }
                else
                {
                    Thread thread = new Thread(() =>
                    {
                        UpdatePinProgress(Colors.Green);
                        Thread.Sleep(5000);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            WindowSwitcher.PinWindow.ERROR.Visibility = Visibility.Hidden;
                            WindowSwitcher.OpenChoice();
                            WindowSwitcher.Choice.Show();
                        }));
                    });
                    //thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                }
            }
        }
    }
}