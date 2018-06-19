using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZinToolExample
{
    public class ServiceHelper
    {
        public static DataTable GetJsonData(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.ContentType = "application/json";
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader re = new StreamReader(stream);
                string json = re.ReadToEnd();

                return JsonConvert.DeserializeObject<DataTable>(json.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static JObject GetVolume24hJsonData(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.ContentType = "application/json";
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader re = new StreamReader(stream);
                string json = re.ReadToEnd();

                return JObject.Parse(json);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static DataTable Tabulate(string json)
        {
            var jsonLinq = JObject.Parse(json);

            // Find the first array using Linq
            var srcArray = jsonLinq.Descendants().Where(d => d is JArray).First();
            var trgArray = new JArray();
            foreach (JObject row in srcArray.Children<JObject>())
            {
                var cleanRow = new JObject();
                foreach (JProperty column in row.Properties())
                {
                    // Only include JValue types
                    if (column.Value is JValue)
                    {
                        cleanRow.Add(column.Name, column.Value);
                    }
                }

                trgArray.Add(cleanRow);
            }

            return JsonConvert.DeserializeObject<DataTable>(trgArray.ToString());
        }

        public static decimal GetVolum24h(string coinName)
        {
             string url_return24hVolume = "https://poloniex.com/public?command=return24hVolume";

            JObject jsonVolume24h = ServiceHelper.GetVolume24hJsonData(url_return24hVolume);
            string btcCoin = "BTC_" + coinName;
            return Convert.ToDecimal(jsonVolume24h[btcCoin]["BTC"]);
        }
    }
}
