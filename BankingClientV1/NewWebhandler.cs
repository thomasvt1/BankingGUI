using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace BankingClientV1
{
    class NewWebHandler
    {

        public String PingBill()
        {
            var map = new Dictionary<string, string>
            {
                { "Client-Id", "bdf5d823-5022-11e7-9f16-de2b444c2004" },
                { "Client-Secret", "bdf5e27d-5022-11e7-9f16-de2b444c2004" }
            };
            return WebHandle("ping/{ATM}", map, true);
        }

        public String RemoveBills(int ten, int twenty, int fifty)
        {
            var map = new Dictionary<string, string>
            {
                { "Client-Id", "bdf5d823-5022-11e7-9f16-de2b444c2004" },
                { "Client-Secret", "bdf5e27d-5022-11e7-9f16-de2b444c2004" },
                { "Ten", ten.ToString() },
                { "Twenty", twenty.ToString() },
                { "Fifty", fifty.ToString() },
            };
            return WebHandle("bills/{ATM}", map, true);
        }

        // ping + clientid + client secret + atm
        /*
         * Private method to handle the api request. Will replace the following tags
         * {user} {pin} {key} to the current session
         * This will return a json string. And if connection fails generates an json with error.
         */

        String ATMID = Regex.Replace(Environment.MachineName, "[^0-9.]", "");

        private String WebHandle(String syntax, Dictionary<String, String> map, Boolean silent)
        {
            string html = string.Empty;
            String url_prefix = "https://api.sofabank.ml/";

            String url = @url_prefix + syntax;

            if (!silent)
                Console.WriteLine(User.GetTag());

            // 
            url = url.Replace("{ATM}", ATMID);

            Console.WriteLine("Request to: " + url);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            foreach (var pair in map)
                request.Headers.Add(pair.Key, pair.Value);

            request.AutomaticDecompression = DecompressionMethods.GZip;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                html = ErrorJson(e);
            }
            if (!silent)
                Console.WriteLine("Response: " + html);
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