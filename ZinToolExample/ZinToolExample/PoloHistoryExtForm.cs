using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ZinToolExample
{
    public partial class PoloHistoryExtForm : Form
    {             
        public PoloHistoryExtForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DateTime.Now < Constant.EffectDate)
            {
                LoadData();
            }
        }

        private string[] GetCoins()
        {
            string strSearch = txtSearch.Text.Trim().ToUpper();
            if (strSearch.Contains(";"))
            {
                return strSearch.Split(';');
            }
            return new string[] { strSearch };
        }        


        private ObjectResult GetObjectResult(string coinName, DataTable table)
        {
            decimal totalBuy = table.AsEnumerable().Where(x => x.Field<string>("type") == "buy").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalSell = table.AsEnumerable().Where(x => x.Field<string>("type") == "sell").Sum(x => Convert.ToDecimal(x.Field<string>("total")));
            decimal totalPrice = table.AsEnumerable().Sum(x => Convert.ToDecimal(x.Field<string>("rate")));
            decimal averageAmount = totalPrice / table.Rows.Count;

            decimal volume24h = ServiceHelper.GetVolum24h(coinName);

            ObjectResult objectResult = new ObjectResult();
            objectResult.CoinName = coinName;
            objectResult.TotalBuy = totalBuy;
            objectResult.TotalSell = totalSell;
            objectResult.AverageAmount = averageAmount;
            objectResult.Total = totalBuy + totalSell;
            objectResult.Volume24H = volume24h;
            objectResult.TotalPerVolume = objectResult.Total / volume24h;

            return objectResult;
        }        

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                LoadData();
            }
        }

        private bool ValidateData()
        {
            if (DateTime.Now < Constant.EffectDate)
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    MessageBox.Show("Please enter the coin name.");
                    return false;
                }

                return true;
            }
            else
            {
                MessageBox.Show("Get data error!!!");
                return false;
            }
        }
        private void LoadData()
        {
            double fromTimeStamp = Utils.ConvertToTimestamp(DateTime.Now.AddDays(-7));
            double toTimeStamp = Utils.ConvertToTimestamp(DateTime.Now);

            lblServerTime.Text = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
            lblProcessing.Visible = true;

            List<ObjectResult> objectResultsMinute5 = null;
            List<ObjectResult> objectResultsMinute15 = null;
            List<ObjectResult> objectResultsMinute30 = null;
            List<ObjectResult> objectResultsHour = null;
            List<ObjectResult> objectResultsHour2 = null;
            List<ObjectResult> objectResultsHour4 = null;
            List<ObjectResult> objectResultsDay = null;
            List<ObjectResult> objectResultsWeek = null;
            List<ObjectResult> objectResultsMonth = null;

            try
            {                
                foreach (var coin in GetCoins())
                {
                    if (!string.IsNullOrEmpty(coin))
                    {
                        string url = string.Format("https://poloniex.com/public?command=returnTradeHistory&currencyPair={0}_{1}&start={2}&end={3}", cbCoinType.Text, coin, fromTimeStamp, toTimeStamp);
                        DataTable dt = ServiceHelper.GetJsonData(url);

                        objectResultsMinute5 = new List<ObjectResult>();
                        objectResultsMinute15 = new List<ObjectResult>();
                        objectResultsMinute30 = new List<ObjectResult>();
                        objectResultsHour = new List<ObjectResult>();
                        objectResultsHour2 = new List<ObjectResult>();
                        objectResultsHour4 = new List<ObjectResult>();
                        objectResultsDay = new List<ObjectResult>();
                        objectResultsWeek = new List<ObjectResult>();
                        objectResultsMonth = new List<ObjectResult>();

                        if (dt != null)
                        {
                            //Load 5 minutes
                            var listMinutes5 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Minutes5);
                            if (listMinutes5 != null && listMinutes5.Count() > 0)
                            {
                                DataTable dtMinute5 = listMinutes5.CopyToDataTable();
                                objectResultsMinute5.Add(GetObjectResult(coin, dtMinute5));
                            }

                            //Load 15 minutes
                            var listMinutes15 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Minutes15);
                            if (listMinutes15 != null && listMinutes15.Count() > 0)
                            {
                                DataTable dtMinute15 = listMinutes15.CopyToDataTable();
                                objectResultsMinute15.Add(GetObjectResult(coin, dtMinute15));
                            }

                                //Load 30 minutes
                            var listMinutes30 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Minutes30);
                            if (listMinutes30 != null && listMinutes30.Count() > 0)
                            {
                                DataTable dtMinute30 = listMinutes30.CopyToDataTable();
                                objectResultsMinute30.Add(GetObjectResult(coin, dtMinute30));
                            }

                            //Load 1 hour
                            var listHour = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Hour);
                            if (listHour != null && listHour.Count() > 0)
                            {
                                DataTable dtHour = listHour.CopyToDataTable();
                                objectResultsHour.Add(GetObjectResult(coin, dtHour));
                            }

                            //Load 2 hour
                            var listHour2 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Hour2);
                            if (listHour2 != null && listHour2.Count() > 0)
                            {
                                DataTable dtHour2 = listHour2.CopyToDataTable();
                                objectResultsHour2.Add(GetObjectResult(coin, dtHour2));
                            }

                            //Load 4 hour
                            var listHour4 = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Hour4);
                            if (listHour4 != null && listHour4.Count() > 0)
                            {
                                DataTable dtHour4 = listHour4.CopyToDataTable();
                                objectResultsHour4.Add(GetObjectResult(coin, dtHour4));
                            }

                            //Load Day
                            var listDay = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Day);
                            if (listDay != null && listDay.Count() > 0)
                            {
                                DataTable dtDay = listDay.CopyToDataTable();
                                objectResultsDay.Add(GetObjectResult(coin, dtDay));
                            }

                            //Load Week
                            var listWeek = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Constant.Week);
                            if (listWeek != null && listWeek.Count() > 0)
                            {
                                DataTable dtWeek = listWeek.CopyToDataTable();
                                objectResultsWeek.Add(GetObjectResult(coin, dtWeek));
                            }

                            ////Load Month
                            //var listMonth = dt.AsEnumerable().Where(x => Convert.ToDateTime(x.Field<string>("date")) >= Month);
                            //if (listMonth != null && listMonth.Count() > 0)
                            //{
                            //    DataTable dtMonth = listMonth.CopyToDataTable();
                            //    objectResultsMonth.Add(GetObjectResult(coin, dtMonth));
                            //}
                        }
                    }
                }

                gvMinute5.DataSource = GridHelper.ConvertListToDataTable(objectResultsMinute5);
                gvMinute15.DataSource = GridHelper.ConvertListToDataTable(objectResultsMinute15);
                gvMinute30.DataSource = GridHelper.ConvertListToDataTable(objectResultsMinute30);
                gvHour.DataSource = GridHelper.ConvertListToDataTable(objectResultsHour);
                gvHour2.DataSource = GridHelper.ConvertListToDataTable(objectResultsHour2);
                gvHour4.DataSource = GridHelper.ConvertListToDataTable(objectResultsHour4);
                gvDay.DataSource = GridHelper.ConvertListToDataTable(objectResultsDay);
                gvWeek.DataSource = GridHelper.ConvertListToDataTable(objectResultsWeek);
                //gvMonth.DataSource = ConvertListToDataTable(objectResultsMonth);

                lblProcessing.Visible = false;
            }
            catch (WebException webex)
            {
                MessageBox.Show("Get data error!!!");
            }
        }

        private void mnDataDetails_Click(object sender, EventArgs e)
        {
            DataDetailsForm form = new DataDetailsForm();
            form.Show();
        }
    }
}
