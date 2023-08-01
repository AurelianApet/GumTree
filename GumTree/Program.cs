using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GumTree
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
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("SoftWare").CreateSubKey("Gumtree");
            string id = reg.GetValue("license", "").ToString();
            string datetime = reg.GetValue("date", "").ToString();
            if(datetime == "")
            {
                Application.Run(new log());
                return;
            }
            getday(id, datetime);
            if (id != "")
            {
                Application.Run(new Form1());
            }
            Application.Run(new log());
        }

        static void getday(string license, string datetime)
        {
            DateTime oldDate = DateTime.Parse(datetime);
            DateTime newDate = DateTime.Now;
            TimeSpan ts = newDate - oldDate;
            int differenceInDays = ts.Days;
            int day = Convert.ToInt32(license.Substring(license.Length - 3, 3),16);
            if (differenceInDays >= day)
            {
                Registry.CurrentUser.DeleteSubKey("Software\\Gumtree");
                return ;
            }
        }
    }
}
