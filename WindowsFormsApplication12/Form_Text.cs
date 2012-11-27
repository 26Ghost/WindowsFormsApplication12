using System;
using System.Windows.Forms;

namespace WindowsFormsApplication12
{
    [Serializable]
    public partial class Form_Text : Form
    {
        public Form_Text(string lyrics, string name)
        {
            InitializeComponent();
            this.Text = "Текст: \""+name+"\"";
            linkLabel1.Text = name;
            richTextBox1.Text = lyrics;
        }
    }
}
