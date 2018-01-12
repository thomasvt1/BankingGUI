using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Windows;

namespace BankingClientV1
{
    class InputHandler
    {
        Window openWindow;
        String input;

        private void SetFocussedWindow()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.IsActive)
                    {
                        openWindow = w;
                        return;
                    }
                }
            }));
        }

        public InputHandler(String input)
        {
            this.input = input;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SetFocussedWindow();
                Handle();
            }));

        }

        void Handle()
        {

            Console.WriteLine("InputHandler called");

            if (openWindow == null)
                return;

            if (User.inputBlocked)
                return;

            if (input.Equals("D"))
            { // als er wordt geannuleerd
                new LetterHandler().LetterCancel();
                return;
            }

            if (input.Equals("C"))
            { // als er wordt geannuleerd
                new LetterHandler().LetterBack();
                return;
            }

            if (input.Equals("TAG CHECK OK"))
            {
                ConfirmHandler();
                User.SetWaitingForCard(false);
                BillHandler.BillHandle(int.Parse(User.GetBalanceInput()));
                return;
            }

            String windowName = openWindow.Title;
            if (windowName.Equals("PinWindow")) // check welke window open is, en vraag naar de bijbehorende handler
                new PinInputHandler(input);

            else if (windowName.Equals("Choice"))
                ChoiceHandler();

            else if (windowName.Equals("WithdrawWindow"))
                WithdrawHandler();

            else if (windowName.Equals("AdvWithdrawWindow"))
                AdvWithdrawHandler();

            else if (windowName.Equals("ReceiptWindow"))
                ReceiptHandler();
        }



        //TODO: (Damian) Zo veel mogelijk copy-pasta verwijderen (aka. de input.Equals in eigen methode)

        private void ChoiceHandler()
        {
            if (input.Equals("1"))
                WindowSwitcher.OpenBalanceWindow();

            else if (input.Equals("2"))
                WindowSwitcher.OpenWithdrawWindow();

            else if (input.Equals("3"))
            {
                if (!BillHandler.isWithdrawable(70))
                {
                    return;
                }
                else
                {
                    User.SetBalanceInput("70");
                    User.SetReceipt(false);
                    WindowSwitcher.OpenConfirm();
                }
            }
        }

        private void ReceiptHandler()
        {
            if (input.Equals("1"))
                User.SetReceipt(true);
            else if (input.Equals("2"))
                User.SetReceipt(false);

            if (input.Equals("1") || input.Equals("2"))
            {
                WindowSwitcher.OpenConfirm();
            }
        }

        private void WithdrawHandler()
        {
            int[] quickMoneySet = { 10, 20, 50, 100, 200, 500 };
            double balance = User.GetBalance();

            int n;
            if (int.TryParse(input, out n))
            {
                if (n == 7)
                {
                    User.SetBalanceInput("");
                    WindowSwitcher.OpenAdvWithdrawWindow();
                    return;
                }
                int amount = quickMoneySet[n - 1];
                if (balance < amount)
                    return;
                User.SetBalanceInput(quickMoneySet[n -1] + "");
            }

            if (User.GetBalanceInput().Length != 0)
                WindowSwitcher.OpenReceiptWindow();
        }

        private void AdvWithdrawHandler()
        {
            String userInput = User.GetBalanceInput();

            if (input.Equals("*"))
            {
                if (User.GetBalanceInput().Length != 0)
                    User.SetBalanceInput(User.GetBalanceInput().Substring(0, User.GetBalanceInput().Length - 1));
            }

            else if (input.Equals("#"))
            {
                double money = User.GetBalance();
                int inputMoney = int.Parse(userInput);

                if (!isWithdrawable(inputMoney))
                {
                    Console.WriteLine("eror komt voor");
                    WindowSwitcher.AdvWithdrawWindow.ERROR.Content = "Dit bedrag is niet in biljetten beschikbaar in deze bankautomaat.";
                    WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Visible;
                    User.inputBlocked = true;
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(5000);
                        User.inputBlocked = false;

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            WindowSwitcher.OpenWithdrawWindow();
                            User.SetBalanceInput("");
                            WindowSwitcher.AdvWithdrawWindow.InputLabel.Content = "";
                            WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Hidden;
                        }));
                    });
                    thread.Start();
                    return;
                }


                if (inputMoney > money)
                {
                    Console.WriteLine("Balance too low!");
                    WindowSwitcher.AdvWithdrawWindow.ERROR.Content = "Uw saldo is te laag om dit bedrag te pinnen";
                    WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Visible;
                    User.inputBlocked = true;

                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(5000);
                        User.inputBlocked = false;

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            User.SetBalanceInput("");
                            WindowSwitcher.AdvWithdrawWindow.InputLabel.Content = "";
                            WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Hidden;
                        }));
                    });
                    thread.Start();
                }
                else //There is enough money to proceed the transaction
                {   
                    if (IsMultipleOfTen(inputMoney))
                        WindowSwitcher.OpenReceiptWindow();

                    else
                    {
                        WindowSwitcher.AdvWithdrawWindow.ERROR.Content = "Dit is een ongeldig bedrag, voer een veelvoud van 10 in";
                        WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Visible;
                        User.inputBlocked = true;

                        Thread thread = new Thread(() =>
                        {
                            Thread.Sleep(5000);
                            User.inputBlocked = false;

                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                User.SetBalanceInput("");
                                WindowSwitcher.AdvWithdrawWindow.InputLabel.Content = "";
                                WindowSwitcher.AdvWithdrawWindow.ERROR.Visibility = Visibility.Hidden;
                            }));
                        });
                        thread.Start();
                    }
                }
            }

            int checkAlsGetal;
            if (int.TryParse(input, out checkAlsGetal))
                User.SetBalanceInput(User.GetBalanceInput() + input);

            WindowSwitcher.AdvWithdrawWindow.InputLabel.Content = User.GetBalanceInput();
        }

        private Boolean IsMultipleOfTen(int x)
        {
            return (x % 10) == 0;
        }

        private void ConfirmHandler()
        {
            if (!User.GetWaitingForCard())
                return;
            
            if (User.GetBalanceInput() == null)
                return;
            if (User.GetBalanceInput().Length == 0)
                return;

            WindowSwitcher.OpenPleaseWaitWindow();
            double amount = Double.Parse(User.GetBalanceInput());
            JObject response = new WebHandler().WithdrawMoney(amount);

            if (response.GetValue("response") == null)
                if (response.GetValue("error") != null)
                {
                    String error = response.GetValue("error").ToString();
                    User.SetError(error);
                    WindowSwitcher.OpenErrorOccurredWindow();
                    return;
                }
            JObject x = (JObject) response.GetValue("transaction");
            if ((Boolean)x.GetValue("success"))
            {
                WindowSwitcher.OpenThanksGoodbyeWindow();
                if (User.GetReceipt())
                    new PrintHandler().Print();
                //PlayThanks();
            }
            Console.WriteLine(response.ToString());
        }

        private void PlayThanks()
        {
            Thread thread = new Thread(() =>
            {
                var player = new System.Windows.Media.MediaPlayer();
                player.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\thanks.mp3", UriKind.Absolute));
                player.Play();
                Thread.Sleep(30000);
            });
            thread.Start();
        }

        private Boolean isWithdrawable(int x)
        {
            JObject bills = JObject.Parse(new NewWebHandler().PingBill());

            int tenBills = (int)bills.GetValue("ten");
            int twentyBills = (int)bills.GetValue("twenty");
            int fiftyBills = (int)bills.GetValue("fifty");

            for (int i = 0; i < fiftyBills && x > 49; i++)
            {
                x -= 50;
            }
            for (int i = 0; i < twentyBills && x > 19; i++)
            {
                x -= 20;
            }
            for (int i = 0; i < tenBills && x > 9; i++)
            {
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