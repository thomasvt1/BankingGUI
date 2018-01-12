using System;
using System.Text.RegularExpressions;

namespace BankingClientV1

{
    class Loader
    {
        NoArduino na;
        NoArduino getNoArduino()
        {
            return na;
        }

        public void Start()
        {
            String ATMID = Regex.Replace(Environment.MachineName, "[^0-9.]", "");
            Console.WriteLine("Starting ATM: " + ATMID);
            na = new NoArduino();
            new ArduinoInterface(na).Invoke();
            
            new ZombieKiller().Start();
            new NetworkMonitor().Start();

            System.Diagnostics.Debug.WriteLine("C# is gwn een bitch");
        }
    }
}