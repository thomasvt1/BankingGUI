using System;
using System.Text.RegularExpressions;

namespace BankingClientV1
{
    class PrintHandler
    {
        public void Print()
        {
            String amount = User.GetBalanceInput();
            Console.WriteLine("Invoking the printer service | amount: " + amount);
            String ATMID = Regex.Replace(Environment.MachineName, "[^0-9.]", "");
            
            var label = DYMO.Label.Framework.Label.Open("sofa.label");


            label.SetObjectText("AMOUNT", "Geldopname: $" + amount);

            String lastNumers = User.GetTag().Substring(User.GetTag().Length - 2);

            label.SetObjectText("PASNUMMER", "Pasnummer: XXXXXX" + lastNumers);

            label.SetObjectText("AUTOMAAT", "Automaat: " + ATMID);

            label.Print("DYMO LabelWriter 400");
            
        }
    }
}
