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
    public partial class PoloHistoryForm : Form
    {
        string[] coins = new string[] { "XRP", "ETC", "STR", "ETH" };
        DateTime Minutes5 = DateTime.Now.ToUniversalTime().AddMinutes(-5);
        DateTime Minutes15 = DateTime.Now.ToUniversalTime().AddMinutes(-15);
        DateTime Minutes30 = DateTime.Now.ToUniversalTime().AddMinutes(-30);
        DateTime Hour = DateTime.Now.ToUniversalTime().AddHours(-1);        
        DateTime Hour4 = DateTime.Now.ToUniversalTime().AddHours(-4);
        DateTime Day = DateTime.Now.ToUniversalTime().AddDays(-1);
        DateTime Week = DateTime.Now.ToUniversalTime().AddDays(-7);
        DateTime Month = DateTime.Now.ToUniversalTime().AddMonths(-1);

        public PoloHistoryForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private DataTable GetJsonData(string url)
        {
            WebRequest req = WebRequest.Create(url);
            req.ContentType = "application/json";
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader re = new StreamReader(stream);
            string json = re.ReadToEnd();            

            return JsonConvert.DeserializeObject<DataTable>(json.ToString());
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
                table.Rows.Add(objectResult.CoinName, objectResult.TotalBuy.ToString("N2"), objectResult.TotalSell.ToString("N2"), objectResult.AverageAmount.ToString("N8"));
            }

            return table;
        }
        

        private ObjectResult GetObjectResult(string coinName, DataTable table)
        {
            decimal totalBuy = table.AsEnumerable().Where(x => x.Field<string>("type") == "buy").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalSell = table.AsEnumerable().Where(x => x.Field<string>("type") == "sell").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalPrice = table.AsEnumerable().Sum(x => Convert.ToDecimal(x.Field<string>("rate")));
            decimal averageAmount = totalPrice / table.Rows.Count;

            ObjectResult objectResult = new ObjectResult();
            objectResult.CoinName = coinName;
            objectResult.TotalBuy = totalBuy;
            objectResult.TotalSell = totalSell;
            objectResult.AverageAmount = averageAmount;

            return objectResult;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                double toTimeStamp = Utils.ConvertToTimestamp(DateTime.Now);
                double fromTimeStamp = Utils.ConvertToTimestamp(DateTime.Now.AddDays(-1));

                List<ObjectResult> objectResults = new List<ObjectResult>();

                foreach (var coin in coins)
                {
                    string url = string.Format("https://poloniex.com/public?command=returnTradeHistory&currencyPair=BTC_{0}&start={1}&end={2}", coin, fromTimeStamp, toTimeStamp);
                    DataTable dt = GetJsonData(url);

                    DataTable dtMinute15 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Minutes15).CopyToDataTable();
                    objectResults.Add(GetObjectResult(coin, dtMinute15));
                }









                ////Load 15 minutes
                //double minute15 = Utils.ConvertToTimestamp(DateTime.Now.AddMinutes(-15));
                //LoadDataDetail(gvMinute15, minute15);

                ////Load 30 minutes
                //double minute30 = Utils.ConvertToTimestamp(DateTime.Now.AddMinutes(-30));
                //LoadDataDetail(gvMinute30, minute30);

                ////Load 1 hour
                //double hour = Utils.ConvertToTimestamp(DateTime.Now.AddHours(-1));
                //LoadDataDetail(gvHour, hour);

                ////Load 2 hours
                //double hour2 = Utils.ConvertToTimestamp(DateTime.Now.AddHours(-2));
                //LoadDataDetail(gvHour2, hour2);

                ////Load 4 hours
                //double hour4 = Utils.ConvertToTimestamp(DateTime.Now.AddHours(-4));
                //LoadDataDetail(gvHour4, hour4);

                ////Load 8 hours
                //double hour8 = Utils.ConvertToTimestamp(DateTime.Now.AddHours(-8));
                //LoadDataDetail(gvHour8, hour8);

                ////Load 1 Day
                //double day = Utils.ConvertToTimestamp(DateTime.Now.AddDays(-1));
                //LoadDataDetail(gvDay, day);

                ////Load 1 Week
                //double week = Utils.ConvertToTimestamp(DateTime.Now.AddDays(-7));
                //LoadDataDetail(gvWeek, week);

                ////Load 1 Month
                //double month = Utils.ConvertToTimestamp(DateTime.Now.AddMonths(-1));
                //LoadDataDetail(gvMonth, month);

            }
            catch (WebException webex)
            {
                MessageBox.Show("Es gab so ein Schlamassel! ({0})", webex.Message);
            }
        }

        private void LoadDataDetail(DataGridView gv, double fromTimeStamp)
        {
            DateTime ToDate = DateTime.Now;
            double toTimeStamp = Utils.ConvertToTimestamp(ToDate);

            List<ObjectResult> objectResults = new List<ObjectResult>();

            foreach (string coin in coins)
            {
                string url = string.Format("https://poloniex.com/public?command=returnTradeHistory&currencyPair=BTC_{0}&start={1}&end={2}", coin, fromTimeStamp, toTimeStamp);
                DataTable dt = GetJsonData(url);
                objectResults.Add(GetObjectResult(coin, dt));
            }

            gv.DataSource = ConvertListToDataTable(objectResults);
        }
    }
}
