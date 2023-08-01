using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace GumTree
{
    public partial class log : Form
    {
        public log()
        {
            InitializeComponent();
        }

        Point mousePoint;
        public void WriteRegistry(string license)
        {
            RegistryKey reg = Registry.CurrentUser.CreateSubKey("SoftWare").CreateSubKey("Gumtree");
            reg.SetValue("license", license);
            reg.SetValue("date", System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));
        }

        private void log_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }
        private void log_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                this.Location = new Point(this.Left - (mousePoint.X - e.X), this.Top - (mousePoint.Y - e.Y));
            }
        }
        public string GenerateKey(string str)
        {
            string secMac = str;
            byte[] EmailByte = Encoding.UTF8.GetBytes(secMac);
            for (int i = 0; i < EmailByte.Length; i++)
            {
                EmailByte[i] ^= 0x45;
            }

            secMac = Encoding.Default.GetString(EmailByte);
            string total = secMac;
            total = generateKey(total);
            return total;
        }
        private string generateKey(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        public List<string> GetMACAddress()
        {
            


            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            List<string> MacAddress = new List<string>();
            foreach (NetworkInterface adapter in adapters)
            {
                System.Net.NetworkInformation.PhysicalAddress pa = adapter.GetPhysicalAddress();
                if (pa != null && !pa.ToString().Equals(""))
                {
                    MacAddress.Add(pa.ToString());
//                    break;
                }
                
            }

            return MacAddress;
        }

        private void bt_cancel_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
            return;
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            bool sameflag = false;
            List<string> address = GetMACAddress();
            for (int i = 0; i < address.Count; i++)
            {
                if (address[i].Equals("00000000000000E0")|address[i] == null)
                {
                    continue;
                }
                string license = GenerateKey(address[i]);
                
                if (tb_license.Text.Substring(0,tb_license.Text.Length-3).Equals(license))
                {
                    //this.Dispose();
                    WriteRegistry(tb_license.Text);
                    this.Visible = false;
                    Form1 mainform = new Form1();
                    mainform.logForm = this;
                    mainform.Show();
                    sameflag = true;
                }
                else
                {
                    //MessageBox.Show("LicenseKey is incorrect.\n Please input license again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    //tb_license.Text = "";
                    continue;
                }
            }
            if (!sameflag)
            {
                MessageBox.Show("LicenseKey is incorrect.\n Please input license again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                tb_license.Text = "";
            }
        }
    }
}
