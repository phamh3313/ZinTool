using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZinToolExample
{
    public partial class TicksFormV2 : Form
    {
        string uri = "https://bittrex.com/api/v1.1/public/getmarkets";
        public TicksFormV2()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                WebRequest req = WebRequest.Create("https://bittrex.com/Api/v2.0/pub/market/GetTicks?marketName=BTC-ETC&tickInterval=oneMin&_=1527292800");
                req.ContentType = "application/json";
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader re = new StreamReader(stream);
                String json = re.ReadToEnd();

                var jsonLinq = JObject.Parse(json);

                DataTable dt = Tabulate(json);

                dataGridView1.DataSource = dt;

            }
            catch (WebException webex)
            {
                MessageBox.Show("Es gab so ein Schlamassel! ({0})", webex.Message);
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

        private string GetRESTData(string uri)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            var webResponse = (HttpWebResponse)webRequest.GetResponse();
            var reader = new StreamReader(webResponse.GetResponseStream());
            return reader.ReadToEnd();
        }
        
    }
}
