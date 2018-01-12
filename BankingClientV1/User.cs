using Newtonsoft.Json.Linq;
using System;

namespace BankingClientV1
{
    static class User
    {

        private static string TAG, PIN, BALANCEINPUT, ERROR;
        private static Boolean RECEIPT, WAITINGFORCARD;
        private static double BALANCE;

        public static Boolean inputBlocked, offline;


        public static void SetValues(String XTAG, String XPIN, double XBALANCE)
        {
            TAG = XTAG;
            PIN = XPIN;
        }

        public static void ClearUser()
        {
            TAG = "";
            PIN = "";
            BALANCEINPUT = "";
            BALANCE = 0;
            RECEIPT = false;
            WAITINGFORCARD = false;
            inputBlocked = false;
        }

        public static double GetBalance()
        {
            if (BALANCE != 0)
                return BALANCE;

            JObject json = new WebHandler().GetBalance();

            String response = (String)json.GetValue("balance");
            double money = double.Parse(response) / 100;
            BALANCE = money;
            return BALANCE;
        }

        public static Boolean GetReceipt()
        {
            return RECEIPT;
        }

        public static String GetError()
        {
            return ERROR;
        }

        public static String GetTag()
        {
            if (TAG == null)
                TAG = "";
            else if (TAG.StartsWith("AG"))
                TAG = TAG.Replace("AG ", "");
            return TAG;
        }

        public static String GetPin()
        {
            if (PIN == null)
                PIN = "";
            return PIN;
        }

        public static Boolean GetWaitingForCard()
        {
            return WAITINGFORCARD;
        }

        public static String GetBalanceInput()
        {
            if (BALANCEINPUT == null)
                BALANCEINPUT = "";
            return BALANCEINPUT;
        }

        public static void SetTag(String XTAG)
        {
            TAG = XTAG;
        }

        public static void SetReceipt(Boolean XTAG)
        {
            RECEIPT = XTAG;
        }

        public static void SetWaitingForCard(Boolean XWAITING)
        {
            WAITINGFORCARD = XWAITING;
        }

        public static void SetPin(String XPIN)
        {
            PIN = XPIN;
        }

        public static void SetError(String XERROR)
        {
            ERROR = XERROR;
        }

        public static void SetBalanceInput(String XBALANCEINPUT)
        {
            BALANCEINPUT = XBALANCEINPUT;
        }
    }
}