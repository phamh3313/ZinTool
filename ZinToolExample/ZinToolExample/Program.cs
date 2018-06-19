using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZinToolExample
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new PoloHistoryExtForm());

            //Application.Run(new PoloHistoryForm());

            //Application.Run(new HistoryForm());

            //Application.Run(new TicksFormV2());
        }
    }
}
