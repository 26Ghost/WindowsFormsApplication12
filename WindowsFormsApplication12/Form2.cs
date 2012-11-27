using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace WindowsFormsApplication12
{
    [Serializable]
    public partial class Form2 : Form
    {
        public int app_id = 2738973;

        public Form2()
        {
            InitializeComponent();

            webBrowser1.ScriptErrorsSuppressed = true;

            if (Registry.CurrentUser.OpenSubKey("Software\\Classes\\MIME\\Database\\Content type\\application/json") == null)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\MIME\\Database\\Content Type\\application/json");
                key.SetValue("CLSID", "{25336920-03F9-11cf-8FD0-00AA00686F13}");
                key.SetValue("Encoding", 0x00080000);
            }
            webBrowser1.Navigate("http://api.vk.com/oauth/logout");

            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();


            webBrowser1.Navigate(String.Format("http://api.vk.com/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", parametrs.app_id, 10));
            while (webBrowser1.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            if (e.Url.ToString().IndexOf("access_token") != -1)
            {
                
                string adr = e.Url.ToString();
                parametrs.access = adr.Substring(adr.IndexOf("access_token=") + "access_token=".Length, adr.IndexOf("&expires_in") - adr.IndexOf("access_token=") - "access_token=".Length);
                parametrs.user = adr.Substring(adr.IndexOf("user_id=") + "user_id=".Length);
                this.Dispose();
            }
            else if (e.Url.ToString().IndexOf("User denied your request") != -1)
                Application.Exit();

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (parametrs.user == null)
            {
                if (MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Авторизация не была пройдена", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    webBrowser1.Navigate(String.Format("http://api.vk.com/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", parametrs.app_id, 10));
                }
                else { Environment.Exit(1); }

            }
        }
    }
}
