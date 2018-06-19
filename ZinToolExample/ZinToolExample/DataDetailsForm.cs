using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZinToolExample
{
    public partial class DataDetailsForm : Form
    {
        DateTime serverCurrentTime;
        public DataDetailsForm()
        {
            InitializeComponent();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            int numberOfDays = GetNumberOfDays();
            double fromTimeStamp = Utils.ConvertToTimestamp(DateTime.Now.AddDays(-numberOfDays));
            double toTimeStamp = Utils.ConvertToTimestamp(DateTime.Now);
            
            int currentMinute = (DateTime.Now.Minute / Constant.INTERVAL_MINUTE) * Constant.INTERVAL_MINUTE;
            serverCurrentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, currentMinute, 0).ToUniversalTime();

            string coin = txtSearch.Text.Trim();
            lblProcessing.Visible = true;

            try
            {
                List<ObjectResult> objectResults = new List<ObjectResult>();

                if (!string.IsNullOrEmpty(coin))
                {
                    string url = string.Format("https://poloniex.com/public?command=returnTradeHistory&currencyPair={0}_{1}&start={2}&end={3}", cbCoinType.Text, coin, fromTimeStamp, toTimeStamp);
                    DataTable dt = ServiceHelper.GetJsonData(url);

                    if (dt != null)
                    {
                        //Load 5 minutes
                        int fiveMinute = (DateTime.Now.Minute / 5) * 5;
                        DateTime serverCurrentFiveMinute = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, fiveMinute, 0).ToUniversalTime();

                        var listMinutes5 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= serverCurrentFiveMinute.AddMinutes(-5));
                        if (listMinutes5 != null && listMinutes5.Count() > 0)
                        {
                            DataTable dtMinute5 = listMinutes5.CopyToDataTable();
                            objectResults.Add(GetObjectDetailsResult(coin, dtMinute5, 5));
                        }                        

                        int totalMinutes = numberOfDays * 24 * 60;
                        var minute = Constant.INTERVAL_MINUTE;

                        while (minute < totalMinutes)
                        {
                            DateTime timeMinute = serverCurrentTime.AddMinutes(-minute);
                            var listResult = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= timeMinute);

                            if (listResult != null && listResult.Count() > 0)
                            {
                                DataTable dtResult = listResult.CopyToDataTable();
                                objectResults.Add(GetObjectDetailsResult(coin, dtResult, minute));

                            }

                            dt = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) < timeMinute).CopyToDataTable();
                            minute += Constant.INTERVAL_MINUTE;
                        }
                    }
                }

                gvDataDetails.DataSource = GridHelper.ConvertListToDetailsDataTable(objectResults);

                lblProcessing.Visible = false;
            }
            catch (WebException webex)
            {
                MessageBox.Show("Get data error!!!");
            }
        }

        private int GetNumberOfDays()
        {
            int numberOfDays = 1;
            try
            {
                numberOfDays = Convert.ToInt32(txtNumberOfDays.Text);
            }
            catch
            {
                numberOfDays = 1;
            }

            return numberOfDays;
        }

        private ObjectResult GetObjectDetailsResult(string coinName, DataTable table, int minutes)
        {
            decimal totalBuy = table.AsEnumerable().Where(x => x.Field<string>("type") == "buy").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalSell = table.AsEnumerable().Where(x => x.Field<string>("type") == "sell").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalPrice = table.AsEnumerable().Sum(x => Convert.ToDecimal(x.Field<string>("rate")));
            decimal averageAmount = totalPrice / table.Rows.Count;

            decimal volume24h = ServiceHelper.GetVolum24h(coinName);

            ObjectResult objectResult = new ObjectResult();
            //objectResult.TimeName = string.Format("{0}:{1}", (minutes / 60).ToString("00"), (minutes % 60).ToString("00"));
            objectResult.TimeName = serverCurrentTime.AddMinutes(-minutes).ToString("HH:mm");
            objectResult.CoinName = coinName;
            objectResult.TotalBuy = totalBuy;
            objectResult.TotalSell = totalSell;
            objectResult.AverageAmount = averageAmount;
            objectResult.Total = totalBuy + totalSell;
            objectResult.Volume24H = volume24h;
            objectResult.TotalPerVolume = objectResult.Total / volume24h;

            return objectResult;
        }

        private void gvDataDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                decimal totalBuy = (decimal)gvDataDetails.Rows[e.RowIndex].Cells["TotalBuy"].Value;
                decimal totalSell = (decimal)gvDataDetails.Rows[e.RowIndex].Cells["TotalSell"].Value;

                if (totalBuy > totalSell)
                    gvDataDetails.Rows[e.RowIndex].Cells["TotalBuy"].Style.BackColor = Color.Green;
                    //e.CellStyle.BackColor = Color.Green;
                else
                    gvDataDetails.Rows[e.RowIndex].Cells["TotalSell"].Style.BackColor = Color.Red;
                    //e.CellStyle.BackColor = Color.Red;
            }
            catch
            {
            }
            
        }
    }
}
