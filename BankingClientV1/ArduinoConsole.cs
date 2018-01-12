using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class ArduinoConsole
    {

        public void HandleArduinoPrint(string s)
        {
            if (User.inputBlocked || User.offline)
            {
                Console.WriteLine("Ignoring input because blocked or offline");
                return;
            }

            Console.WriteLine("DEBUG: '" + s + "'");
            //TODO: Handle the arduino event.
            if (s.StartsWith("TAG"))
                TagHandler(s.Substring(4));
            else if (s.StartsWith("INP"))
                InputHandler(s.Substring(4));
        }

        private void InputHandler(String s)
        {
            new InputHandler(s);
        }

        private void TagHandler(string tag)
        {
            

            if (User.GetTag().Length != 0)
            {
                if (User.GetTag().Equals(tag))
                    InputHandler("TAG CHECK OK");
                Console.WriteLine("SAME TAG PRESENTED");

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    if (WindowSwitcher.GetActiveWindow().Title.Equals("MainWindow"))
                        User.SetTag("");
                }));
                return;
            }

            User.SetTag(tag);
            Console.WriteLine("TAG: " + tag);

            JObject json = new WebHandler().GetCardValid();

            if (json.GetValue("error") != null)
            {
                Console.WriteLine("Connection error!");
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    new NoNetwork().Show();
                }));
                
                return;
            }


            Boolean cardExists = (Boolean) json.GetValue("exists");

            if (!cardExists)
            {
                User.SetError("Deze kaart komt niet voor in onze database");
                WindowSwitcher.OpenErrorOccurredWindow();
                User.SetTag("");
                return;
            }
            
            WindowSwitcher.OpenPinWindow();

            int tries = (int) json.GetValue("tries");

            if (tries == 3)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    WindowSwitcher.PinWindow.ERROR.Content = "Uw pas is geblokeerd, neem zo snel mogelijk contact op";
                    WindowSwitcher.PinWindow.ERROR.Visibility = Visibility.Visible;
                    User.inputBlocked = true;
                }));


                Thread thread = new Thread(() =>
                {
                    Thread.Sleep(5000);
                    WindowSwitcher.OpenThanksGoodbyeWindow();
                });
                //thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }

            else if (tries != 0)
                UpdateTries(3 - tries);
        }

        private void UpdateTries(int triesremaining)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                WindowSwitcher.PinWindow.ERROR.Content = ("Nog X poging(en) over").Replace("X", triesremaining + "");
                WindowSwitcher.PinWindow.ERROR.Visibility = Visibility.Visible;
            }));
        }
    }
}