using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZinToolExample
{
    public class Constant
    {
        public static DateTime EffectDate = DateTime.Parse("2018/08/15");

        public static DateTime Minutes5 = DateTime.Now.ToUniversalTime().AddMinutes(-5);
        public static DateTime Minutes15 = DateTime.Now.ToUniversalTime().AddMinutes(-15);
        public static DateTime Minutes30 = DateTime.Now.ToUniversalTime().AddMinutes(-30);
        public static DateTime Hour = DateTime.Now.ToUniversalTime().AddHours(-1);
        public static DateTime Hour2 = DateTime.Now.ToUniversalTime().AddHours(-2);
        public static DateTime Hour4 = DateTime.Now.ToUniversalTime().AddHours(-4);
        public static DateTime Day = DateTime.Now.ToUniversalTime().AddDays(-1);
        public static DateTime Week = DateTime.Now.ToUniversalTime().AddDays(-7);
        public static DateTime Month = DateTime.Now.ToUniversalTime().AddMonths(-1);

        public const int INTERVAL_MINUTE = 15;
    }
}
