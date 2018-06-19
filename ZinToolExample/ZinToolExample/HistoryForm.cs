using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZinToolExample
{
    public partial class HistoryForm : Form
    {
        string url = "https://bittrex.com/api/v1.1/public/getmarkethistory?market=BTC-ETC";

        public HistoryForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                WebRequest req = WebRequest.Create("https://bittrex.com/api/v1.1/public/getmarkethistory?market=BTC-ETC");
                req.ContentType = "application/json";
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader re = new StreamReader(stream);
                String json = re.ReadToEnd();

                var jsonLinq = JObject.Parse(json);

                string[] coins = new string[] { "ETC", "XRP", "LTC", "TRX", "ETH" };

                List<ObjectResult> objectResults = new List<ObjectResult>();

                foreach (string coin in coins)
                {
                    string url = string.Format("https://bittrex.com/api/v1.1/public/getmarkethistory?market=BTC-{0}", coin);
                    DataTable dt = GetJsonData(url);
                    objectResults.Add(GetObjectResult(coin, dt));
                }

                dataGridView1.DataSource = ConvertListToDataTable(objectResults);
            }
            catch (WebException webex)
            {
                MessageBox.Show("Es gab so ein Schlamassel! ({0})", webex.Message);
            }
        }

        private DataTable GetJsonData(string url)
        {
            WebRequest req = WebRequest.Create(url);
            req.ContentType = "application/json";
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader re = new StreamReader(stream);
            String json = re.ReadToEnd();

            var jsonLinq = JObject.Parse(json);

            return Tabulate(json);
        }

        static DataTable ConvertListToDataTable(List<ObjectResult> list)
        {
            DataTable table = new DataTable();
            table.Columns.Add("CoinName", typeof(string));
            table.Columns.Add("TotalBuy", typeof(decimal));
            table.Columns.Add("TotalSell", typeof(decimal));
            table.Columns.Add("AverageAmount", typeof(decimal));

            foreach(var objectResult in list)
            {
                table.Rows.Add(objectResult.CoinName, objectResult.TotalBuy, objectResult.TotalSell, objectResult.AverageAmount);
            }

            return table;
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

        public IEnumerable<HistoryObject> GetHistoryObjects(DataTable dt)
        {
            List<HistoryObject> historyObjects = new List<HistoryObject>();
            historyObjects = (from DataRow dr in dt.Rows
                           select new HistoryObject()
                           {
                               Id = Convert.ToInt32(dr[0]),                               
                               Quantity = Convert.ToDecimal(dr[2]),
                               Price = Convert.ToDecimal(dr[3]),
                               Total = Convert.ToDecimal(dr[4]),
                               FillType = dr[5].ToString(),
                               OrderType = dr[6].ToString(),
                           }).ToList();

            return historyObjects;
        }

        private ObjectResult GetObjectResult(string coinName, DataTable table)
        {
            decimal totalBuy = 0;
            decimal totalSell = 0;
            decimal averageAmount = 0;
            decimal totalPrice = 0;

            foreach (DataRow item in table.Rows)
            {
                totalPrice += Convert.ToDecimal(item["Price"]);

                if (item["OrderType"].ToString() == "BUY")
                {
                    totalBuy += Convert.ToDecimal(item["Total"]);
                }

                if (item["OrderType"].ToString() == "SELL")
                {
                    totalSell += Convert.ToDecimal(item["Total"]);
                }
            }

            averageAmount = totalPrice / table.Rows.Count;

            ObjectResult objectResult = new ObjectResult();
            objectResult.CoinName = coinName;
            objectResult.TotalBuy = totalBuy;
            objectResult.TotalSell = totalSell;
            objectResult.AverageAmount = averageAmount;

            return objectResult;
        }

        private string GetRESTData(string uri)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            var reader = new StreamReader(webResponse.GetResponseStream());
            return reader.ReadToEnd();
        }
        
    }
}
