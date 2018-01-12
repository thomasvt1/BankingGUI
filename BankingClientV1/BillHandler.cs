using System;
using MonoBrick.NXT;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace BankingClientV1
{
    class BillHandler
    {           
        public static void BillHandle(int amount)
        {
            try
            {
                var brick = new Brick<Sensor, Sensor, Sensor, Sensor>("usb");

                JObject bills = JObject.Parse(new NewWebHandler().PingBill());
                Console.WriteLine(bills);

                int tenBills = (int)bills.GetValue("ten");
                int twentyBills = (int)bills.GetValue("twenty");
                int fiftyBills = (int)bills.GetValue("fifty");
                int maxTen = 0;
                int maxTwenty = 0;
                int maxFifty = 0;
                brick.Connection.Open();
                brick.MotorA.On(0);
                brick.MotorB.On(0);
                brick.MotorC.On(0);

                for (int i = 0; i < fiftyBills && amount > 49; i++)
                {
                    maxFifty++;
                    amount -= 50;
                }
                for (int i = 0; i < twentyBills && amount > 19; i++)
                {
                    maxTwenty++;
                    amount -= 20;
                }
                for (int i = 0; i < tenBills && amount > 9; i++)
                {
                    maxTen++;
                    amount -= 10;
                }
                if (amount == 0)
                {
                    for (int i = 0; i < maxFifty; i++)
                    {
                        brick.MotorA.On(-40);
                        Thread.Sleep(590);
                        brick.MotorA.On(0);
                    }
                    for (int i = 0; i < maxTwenty; i++)
                    {
                        brick.MotorB.On(-40);
                        Thread.Sleep(615);
                        brick.MotorB.On(0);
                    }
                    for (int i = 0; i < maxTen; i++)
                    {
                        brick.MotorC.On(-40);
                        Thread.Sleep(580);
                        brick.MotorC.On(0);
                        new NewWebHandler().RemoveBills(maxTen, maxTwenty, maxFifty);
                        maxFifty = 0;
                        maxTwenty = 0;
                        maxTen = 0;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadKey();
            }
        }
        public static Boolean isWithdrawable(int x)
        {
            JObject bills = JObject.Parse(new NewWebHandler().PingBill());
            
            int tenBills = (int)bills.GetValue("ten");
            int twentyBills = (int)bills.GetValue("twenty");
            int fiftyBills = (int)bills.GetValue("fifty");

            for (int i = 0; i < fiftyBills && x > 49; i++)
            {
//                maxFifty++;
                x -= 50;
            }
            for (int i = 0; i < twentyBills && x > 19; i++)
            {
 //               maxTwenty++;
                x -= 20;
            }
            for (int i = 0; i < tenBills && x > 9; i++)
            {
//                maxTen++;
                x -= 10;
            }
            if (x == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
