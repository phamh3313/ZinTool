using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZinToolExample
{
    public class Utils
    {
        public static DateTime Epoch { get; private set; }

        public static double ConvertToTimestamp(DateTime value)
        {
            //TimeSpan elapsedTime = value - Epoch;
            //return (long)elapsedTime.TotalSeconds;
            return (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }
    }
}
