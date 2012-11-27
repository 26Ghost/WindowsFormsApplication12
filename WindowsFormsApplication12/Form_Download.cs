using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication12
{
    [Serializable]
    public partial class Form_Download : Form
    {
        public Form_Download()
        {
            InitializeComponent();
            this.Controls.Add(parametrs.LV);
        }

        private void Form_Download_Load(object sender, EventArgs e)
        {
            this.Controls[0].GotFocus += new EventHandler(Form_Download_GotFocus);
            this.Controls[0].LostFocus += new EventHandler(Form_Download_LostFocus);
        }

        void Form_Download_LostFocus(object sender, EventArgs e)
        {
            //this.Opacity = 0.8;
            //throw new NotImplementedException();
        }

        void Form_Download_GotFocus(object sender, EventArgs e)
        {
            //this.Opacity = 1;
            //throw new NotImplementedException();
        }


        private void Form_Download_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true; ;
            this.Visible = false;
            this.ShowInTaskbar = false;
        }
    }
}
