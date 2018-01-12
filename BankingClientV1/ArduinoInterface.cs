using System;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class ArduinoInterface  
    {

        int portnumber = 8;

        ArduinoConsole ac;
        SerialPort port;
        NoArduino na;

        public ArduinoInterface(NoArduino na)
        {
            ac = new ArduinoConsole();
            this.na = na;
        }

        public void Invoke()
        {
            Thread thread = new Thread(() =>
            {
                SetupPort();
                RunArduinoListener();
            });
            //thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        /*
         * This is the main method of ArduinoInterface
         * It will invoke certain methods to setup port and print etc.
         */
        private void RunArduinoListener()
        {
            Thread.Sleep(1000);
            while (true)
            {
                if (Application.Current == null)
                {
                    Console.WriteLine("Application.Current == null");
                    return;
                }

                try
                {
                    HandleSerialConsole();
                }
                catch (Exception e)
                {
                    Console.WriteLine("PORT EXCEPTION");

                    //CheckIfApplicationClosed();

                    SetupPort();

                    if (tries > 0)
                        TryOpenPortElseShowError();

                    Thread.Sleep(250);
                }
            }
        }

        private void OpenArduinoWarning()
        {
            //Console.WriteLine("VISIBLE CHECK");
            if (!na.IsActive)
            {
                na = new NoArduino();
                na.Show();
            }

            else if (!na.IsVisible)
            {
                na.Visibility = Visibility.Visible;
                Console.WriteLine("NOT VISIBLE");
            }
        }

        void CloseAllWarningScreens()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                foreach (Window w in Application.Current.Windows)
                {
                    if (w.Title.Equals("NoArduino"))
                    {
                        w.Close();
                    }
                }
            }));
        }

        private void TryOpenPortElseShowError()
        {
            try
            {
                port.Open();

                if (port.IsOpen)
                {
                    Console.WriteLine("CLOSING NA");
                    tries = -5;
                    CloseAllWarningScreens();
                }
            }
            catch (Exception e)
            {
                CheckIfApplicationClosed();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    OpenArduinoWarning();
                }));

            }
        }

        /*
         * This method will setup the port to connect it to Arduino.
         */
        private void SetupPort()
        {
            int COMNUMBER = GetPortNumberAndIncrease();
            Console.WriteLine("TRY: " + tries + " COMPORT: " + portnumber);
            String portnum = "COM" + COMNUMBER;

            port = new SerialPort()
            {
                BaudRate = 9600,
                PortName = portnum
            };


        }

        int tries = 0;

        int GetPortNumberAndIncrease()
        {
            if (tries != 1)
            {
                tries++;
                return portnumber;
            }
            else
            {
                tries = 0;
                if (portnumber > 10)
                {
                    portnumber = 0;
                    return portnumber;
                }
                else
                {
                    portnumber++;
                    return portnumber;
                }
            }
        }

        /*
         * This will push all serial input to ArduinoConsole to be handled
         */
        private void HandleSerialConsole()
        {
            if (port.IsOpen)
                CloseAllWarningScreens();

            String data_rx = port.ReadLine();

            //TODO: readtimeout
            String data_up = Regex.Replace(data_rx, @"\t|\n|\r", "");

            try
            {
                ac.HandleArduinoPrint(data_up);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        /*
         * This will check if the NO ARDUINO message is visible, and otherwise
         * It will reopen the window to make sure it's in front.
         */
        private void CheckIfApplicationClosed()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (na.IsActive)
                    return;

                if (na == null)
                {
                    //Console.WriteLine("na == null)");
                }
                else if (!na.IsActive)
                {
                    na.Activate();
                    //Console.WriteLine("!na.IsActive");
                }
            }));
        }
    }
}