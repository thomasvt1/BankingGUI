using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace BankingClientV1
{
    class WebHandler
    {

        private string GetTimeHash()
        {
            DateTime date = DateTime.Now;
            int prefix = date.Minute * 9 + date.Day + 5 + date.DayOfYear * 3 * date.Month * 9 * date.Year;
            string suffix = "" + date.Minute + (date.Year * date.Day) + (date.Minute * date.Minute) + date.Day + (date.DayOfYear * date.Day);
            String final = prefix + suffix;
            return final.Substring(0, 16);
        }

        //gXbB9%kXrg6cxh#y
        String GetEncryptedPin()
        {
            String pin = User.GetPin();
            String encryptedpin = new Encryption("gXbB9%kXrg6cxh#y").Encrypt(pin);

            return encryptedpin;
        }

        private String GetKey()
        {
            //String timeHash = GetTimeHash();

            //String encryptKey = new Encryption(timeHash).Encrypt("LOL");

            //Console.WriteLine(encryptKey);

            //return encryptKey;
            return "debug";
        }

        public JObject GetCardValid()
        {
            String url = "action=cardCheck&user={user}&key={key}";

            String response = WebHandle(url, false);

            JObject json = JObject.Parse(response);

            return json;
        }

        public JObject GetBalance()
        {
            String url = "action=getBalance&user={user}&pin={pin}&key={key}";

            String response = WebHandle(url, false);

            JObject json = JObject.Parse(response);

            return json;
        }

        public JObject WithdrawMoney(double amount)
        {
            String url = "action=withdrawMoney&user={user}&pin={pin}&amount={amount}&key={key}";

            //url = url.Replace("{user}", User.GetTag()).Replace("{pin}", GetEncryptedPin());
            url = url.Replace("{amount}", amount + "");

            Console.WriteLine(url);

            String response = WebHandle(url, false);

            JObject json = JObject.Parse(response);

            return json;
        }

        public String PingTest()
        {
            return WebHandle("key={key}", true);
        }
        
        /*
         * Private method to handle the api request. Will replace the following tags
         * {user} {pin} {key} to the current session
         * This will return a json string. And if connection fails generates an json with error.
         */
        int tries = 0;
        private String WebHandle(String syntax, Boolean silent)
        {
            string html = string.Empty;
            String url_prefix = "https://api.bank.thomasvt.xyz/oldapi?";
            //String url_prefix = "http://localhost:9010/oldapi?";

            String url = @url_prefix + syntax;

            if (!silent)
                Console.WriteLine(User.GetTag());

            //GetKey()
            url = url.Replace("{user}", User.GetTag()).Replace("{pin}", GetEncryptedPin()).Replace("{key}", GetKey());

            Console.WriteLine("Request to: " + url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
            } catch(Exception e)
            {
                html = ErrorJson(e);
            }
            if (!silent)
                Console.WriteLine("Response: " + html);

            if (html.Contains("BAD KEY"))
            {
                if (tries > 10)
                {
                    User.SetError("Veilige verbinding met SOFA niet gelukt");
                    WindowSwitcher.OpenErrorOccurredWindow();
                    return html;
                }
                tries++;
                Thread.Sleep(100);
                Console.WriteLine("ERROR WITH KEY - TRY: " + tries);
                return WebHandle(syntax, silent);
            }
            tries = 0;
            return html;
        }

        private String ErrorJson(Exception e)
        {
            JObject json = new JObject
            {
                { "error", "connection error" },
                { "details", e.Message }
            };
            return json.ToString();
        }
    }
}