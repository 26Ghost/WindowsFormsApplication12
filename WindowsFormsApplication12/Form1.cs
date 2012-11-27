using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;
using Microsoft.Win32;
using Un4seen;

namespace WindowsFormsApplication12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
       private void Form1_Load(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
            
            lv_add("_мои_записи", (TabPage)tabControl1.TabPages[0].Controls[0].Controls[0]);
            lv_add("_поиск", (TabPage)tabControl1.TabPages[0].Controls[0].Controls[1]);
            lv_add("_друзья", (TabPage)tabControl1.TabPages[0].Controls[0].Controls[2]);

            lv_add("_мои_записи", (TabPage)tabControl1.TabPages[1].Controls[0].Controls[0]);
            lv_add("_поиск", (TabPage)tabControl1.TabPages[1].Controls[0].Controls[1]);
            lv_add("_друзья", (TabPage)tabControl1.TabPages[1].Controls[0].Controls[2]);

            lv_add("_мои_записи", (TabPage)tabControl1.TabPages[2].Controls[0].Controls[0]);
            lv_add("_поиск", (TabPage)tabControl1.TabPages[2].Controls[0].Controls[1]);
            lv_add("_друзья", (TabPage)tabControl1.TabPages[2].Controls[0].Controls[2]);

            download_LV();
            parametrs.fd = new Form_Download();
            parametrs.wcl = new List<WebClient>();
            parametrs.prgb = new List<ProgressBar>();
            parametrs.sr = SortOrder.None;
            parametrs.LV.Scroll += LV_Scroll;
            parametrs.LV.SizeChanged += new EventHandler(LV_SizeChanged);
            audio((ListView)tabControl1.TabPages[0].Controls[0].Controls[0].Controls[0]);
            this.tabPage4.Enter +=new EventHandler(tabPage4_Enter);
            this.tabPage5.Enter += new EventHandler(tabPage5_Enter);
           //Un4seen.Bass.Bass.
        }

       void tabPage5_Enter(object sender, EventArgs e)
       {
           //throw new NotImplementedException();
           XmlDocument res = new XmlDocument();
           res.Load(string.Format("https://api.vkontakte.ru/method/audio.getAlbums.xml?access_token={0}&uid={1}", parametrs.access, "6337587"));
           var xml = res.GetElementsByTagName("response");
           
           Console.WriteLine("ggg");       
       }

       void tabPage4_Enter(object sender,EventArgs e)
       {
           //throw new NotImplementedException();
           if ( ((ListView)(((TabPage)sender).Controls[0])).Items.Count == 0)
           {
               if (MessageBox.Show("Загрузить фотографии? \nЭто займет некоторое время", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
               {
                   XmlDocument res = new XmlDocument();
                   res.Load(string.Format("https://api.vkontakte.ru/method/friends.get.xml?access_token={0}&fields={1}", parametrs.access, "uid,first_name,last_name,nickname,photo"));
                   XmlNodeList xnl = res.GetElementsByTagName("user");
                   ////////////
                   ImageList photo = new ImageList();
                   photo.ImageSize = new Size(45, 45);
                   photo.ColorDepth = ColorDepth.Depth8Bit;

                   ((ListView)(((TabPage)sender).Controls[0])).SmallImageList = photo;
                   //////////
                   foreach (XmlNode node in xnl)
                   {
                       
                       photo.Images.Add(Image.FromStream(WebRequest.Create(node.ChildNodes[3].InnerText).GetResponse().GetResponseStream()));
                       ((ListView)(((TabPage)sender).Controls[0])).Items.Add(new ListViewItem(new string[] { node.ChildNodes[1].InnerText, node.ChildNodes[2].InnerText },photo.Images.Count-1 ));
                       
                   }
               }
           }
       }

       void LV_ColumnClick(object sender, ColumnClickEventArgs e)
       {
           //throw new NotImplementedException();
           ((ListView)sender).BeginUpdate();
           if (parametrs.sr == SortOrder.None)
           {
               //((ListView)sender).Columns[e.Column].Text += "  ↑";
               parametrs.sr = SortOrder.Ascending;
               ((ListView)sender).ListViewItemSorter = new ListViewItemComparer(e.Column,1 ); 
           }
           else if (parametrs.sr == SortOrder.Ascending)
           {
               //((ListView)sender).Columns[e.Column].Text += "  ↓";
               parametrs.sr = SortOrder.Descending;
               ((ListView)sender).ListViewItemSorter = new ListViewItemComparer(e.Column,-1 ); 
           }
           else if (parametrs.sr == SortOrder.Descending)
           {
               //((ListView)sender).Columns[e.Column].Text += "  ↑";
               parametrs.sr = SortOrder.Ascending;
               ((ListView)sender).ListViewItemSorter = new ListViewItemComparer(e.Column,1 ); 
           }
           redraw((ListView)sender);
           ((ListView)sender).EndUpdate();
           
       }
       private void redraw(ListView lv)
       {
           foreach (ListViewItem  i in lv.Items)
           {
               if (i.Index % 2 == 0)
                   i.BackColor = Color.Silver;
               else
                   i.BackColor = SystemColors.InactiveBorder;
           } 
       }

        void LV_SizeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            prgbr_resize();
        }

        void LV_Scroll(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            prgbr_resize();
        }
        private void prgbr_resize()
        {
            for (int i = 0; i < parametrs.prgb.Count; i++)
                parametrs.prgb[i].SetBounds(parametrs.LV.Items[i].SubItems[1].Bounds.X, parametrs.LV.Items[i].SubItems[1].Bounds.Y, parametrs.LV.Columns[1].Width, parametrs.LV.Items[0].Bounds.Height);
        }

        private void download_LV()
        {
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "Название";
            ch1.Width = 300;
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "Скачано";
            ch2.Width = 200;
            ColumnHeader ch3 = new ColumnHeader();
            ch3.Text = "Размер";
            ch3.Width = 200;
            //ColumnHeader ch4 = new ColumnHeader();
            //ch4.Text = "%";
            //ch4.Width = 30;
            //parametrs.LV = new ListView();
            parametrs.LV = new ScrollableListView();
            parametrs.LV.ColumnWidthChanged += LV_ColumnWidthChanged;
            parametrs.LV.BackColor = SystemColors.InactiveBorder;
            parametrs.LV.Dock = DockStyle.Fill;
            parametrs.LV.Location = new Point(3, 3);
            //lv.Size = new System.Drawing.Size(564, 307);
            parametrs.LV.TabIndex = 0;
            parametrs.LV.UseCompatibleStateImageBehavior = false;
            parametrs.LV.View = View.Details;
            parametrs.LV.HideSelection = false;
            //lv.MouseClick += new MouseEventHandler(listView_MouseClick);
            //lv.ColumnClick += new ColumnClickEventHandler(listView1_ColumnClick);
            //tabControl1.TabPages["TabPage" + listView3.GetItemAt(e.X, e.Y).Text].Controls.Add(lv);
            parametrs.LV.FullRowSelect = true;
            //tbp.Controls.Add(lv);


            parametrs.LV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ch1, ch2, ch3 });
        }

        void LV_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            //throw new NotImplementedException();
            prgbr_resize();
        }
        private void lv_add(string name, TabPage tbp)
        {
            ListView lv = new ListView();
            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "№";
            ColumnHeader ch3 = new ColumnHeader();
            ch3.Text = "Название";
            ch3.Width = 170;
            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "Исполнитель";
            ch2.Width = 170;
            ColumnHeader ch4 = new ColumnHeader();
            ch4.Text = "Длительность";
            ColumnHeader ch5 = new ColumnHeader();
            ch5.Text = "Размер";
            ch5.Width = 60;

            lv.BackColor = System.Drawing.SystemColors.InactiveBorder;
            lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ch1, ch2, ch3, ch4 });
            lv.Dock = System.Windows.Forms.DockStyle.Fill;
            lv.Location = new System.Drawing.Point(3, 3);
            lv.Name = "ListView" + name; // listView3.GetItemAt(e.X, e.Y).Text;
            lv.Size = new System.Drawing.Size(564, 307);
            lv.TabIndex = 0;
            lv.UseCompatibleStateImageBehavior = false;
            lv.View = System.Windows.Forms.View.Details;
            lv.HideSelection = false;
            lv.MouseClick += new MouseEventHandler(listView_MouseClick);
            //tabControl1.TabPages["TabPage" + listView3.GetItemAt(e.X, e.Y).Text].Controls.Add(lv);
            lv.FullRowSelect = true;
            lv.ColumnClick += new ColumnClickEventHandler(LV_ColumnClick);
            lv.MouseClick += new MouseEventHandler(lv_MouseClick);
            tbp.Controls.Add(lv);

        }

        void lv_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (((ListView)sender).GetItemAt(e.X, e.Y).SubItems[6].Text.Length == 0)
                    contextMenuStrip1.Items[0].Enabled = false;
                else
                {
                    contextMenuStrip1.Items[0].Enabled = true;
                    parametrs.lyrics_point = e.Location;
                }
                contextMenuStrip1.Show(MousePosition.X,MousePosition.Y);
               
            }
            //throw new NotImplementedException();
        }
        private void listView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //((ListView)tabControl2.SelectedTab.Controls[0]).GetItemAt(e.X, e.Y)
                
            }
        }

        private void audio(ListView lv)
        {
            XmlDocument res = new XmlDocument();
            res.Load(String.Format("https://api.vkontakte.ru/method/audio.get.xml?access_token={0}&uid={1}", parametrs.access, parametrs.user));
            string wtf = res.InnerXml.ToString();

            if (wtf.IndexOf("<error_msg>Access denied: user deactivated</error_msg>", System.StringComparison.Ordinal) == -1)
            {
                //WebRequest req;// = (HttpWebRequest)WebRequest.Create(parametrs.LV.Items[i].SubItems[3].Text);
               // HttpWebResponse resp;// = (HttpWebResponse)req.GetResponse();
                //parametrs.LV.Items[i].SubItems[2].Text = (((resp.ContentLength) / 1024f) / 1024f).ToString().Substring(0, 5) + " Mb";
                //req.Method = "HEAD";
                //req.Proxy = null;
                //WebClient wc = new WebClient();
                //wc.
                XmlNodeList xnl = res.GetElementsByTagName("audio");
                for (int i = 0; i < xnl.Count; i++)
                {
                    TimeSpan time = new TimeSpan(0, 0, int.Parse(xnl[i].ChildNodes[4].InnerText));
                    /*WebRequest*/ //req= (HttpWebRequest)WebRequest.Create(xnl[i].ChildNodes[5].InnerText); 
                    //req.Proxy = null;
                    //req.Method = "HEAD";
                    /*HttpWebResponse*/ //resp = (HttpWebResponse)req.GetResponse();
                   
                    if (xnl[i].ChildNodes.Count == 6)
                        lv.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), xnl[i].ChildNodes[2].InnerText.ToString(), xnl[i].ChildNodes[3].InnerText.ToString(), time.ToString()/*,(((resp.ContentLength) / 1024f) / 1024f).ToString().Substring(0, 5) + " Mb"*/, xnl[i].ChildNodes[4].InnerText.ToString(), xnl[i].ChildNodes[5].InnerText, null, }));
                    else if (xnl[i].ChildNodes.Count==7)
                        lv.Items.Add(new ListViewItem(new string[] { (i + 1).ToString(), xnl[i].ChildNodes[2].InnerText.ToString(), xnl[i].ChildNodes[3].InnerText.ToString(), time.ToString()/*,(((resp.ContentLength) / 1024f) / 1024f).ToString().Substring(0, 5) + " Mb"*/ , xnl[i].ChildNodes[4].InnerText.ToString(), xnl[i].ChildNodes[5].InnerText.ToString(), xnl[i].ChildNodes[6].InnerText.ToString() }));
                    else
                    {
                        MessageBox.Show("WTF!!");
                    }
                    //req.Abort();
                    //resp.Close();
                    if (lv.Items.Count % 2 == 0)
                        lv.Items[lv.Items.Count - 1].BackColor = Color.Silver;//SystemColors.ControlLight;
                }
            }
            else MessageBox.Show("Пользователь удален или забанен.");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ListView lv1 = ((ListView)((TabControl)tabControl1.SelectedTab.Controls[0]).SelectedTab.Controls[0]);

            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {

               
                for (int i = 0; i < lv1.SelectedIndices.Count; i++)
                {
                    parametrs.LV.Items.Add(new ListViewItem(new string[] { lv1.Items[lv1.SelectedIndices[i]].SubItems[1].Text + " - " + lv1.Items[lv1.SelectedIndices[i]].SubItems[2].Text, "0%", lv1.Items[lv1.SelectedIndices[i]].SubItems[4].Text, lv1.Items[lv1.SelectedIndices[i]].SubItems[5].Text}));
                    if (parametrs.LV.Items.Count % 2 == 0)
                        parametrs.LV.Items[parametrs.LV.Items.Count - 1].BackColor = SystemColors.ControlLight;

                    ProgressBar pb = new ProgressBar();
                    pb.Parent = parametrs.LV;
                    pb.Visible = true;
                    pb.Bounds = new Rectangle(parametrs.LV.Items[parametrs.LV.Items.Count - 1].SubItems[1].Bounds.X, parametrs.LV.Items[parametrs.LV.Items.Count - 1].SubItems[1].Bounds.Y, parametrs.LV.Columns[1].Width, parametrs.LV.Items[0].Bounds.Height);
                    pb.SetBounds(parametrs.LV.Items[parametrs.LV.Items.Count - 1].SubItems[1].Bounds.X, parametrs.LV.Items[parametrs.LV.Items.Count - 1].SubItems[1].Bounds.Y, parametrs.LV.Columns[1].Width, parametrs.LV.Items[0].Bounds.Height);
                    pb.BackColor = Color.Black;
                    pb.ForeColor = Color.Lime;
                    pb.Style = ProgressBarStyle.Blocks;
                   
                    parametrs.prgb.Add(pb);
                    download(parametrs.LV.Items.Count-1, fd.SelectedPath);
                    
                }
                //MessageBox.Show("BioHazard ☣☣");
                parametrs.fd.Show();
                parametrs.fd.ShowInTaskbar = true;
            }
            else MessageBox.Show("Ничего не выбрано.");
        }
        private void download(int i, string savepath)
        {
            //сделать download после всех + и скачивать, если не 0%
            WebClient wc = new WebClient();
            parametrs.wcl.Add(wc);
            wc.DownloadProgressChanged += wc_DownloadProgressChanged;
            wc.DownloadFileCompleted += wc_DownloadFileCompleted;
            wc.DownloadFileAsync(new Uri(parametrs.LV.Items[i].SubItems[3].Text), savepath + @"\" + parametrs.LV.Items[i].Text + ".mp3");
            //var req = WebRequest.Create(parametrs.LV.Items[i].SubItems[3].Text);
            //var resp = (HttpWebResponse)req.GetResponse();
            //System.Threading.Thread.Sleep(100);
            //parametrs.LV.Items[i].SubItems[2].Text = (((resp.ContentLength)/1024f)/1024f).ToString().Substring(0,5)+" Mb";
        }
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
           
            ((ProgressBar)parametrs.prgb[parametrs.wcl.IndexOf((WebClient)sender)]).Value = e.ProgressPercentage;              
            //parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].SubItems[3].Text = e.ProgressPercentage.ToString();
            //parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].SubItems[2].Text = e.BytesReceived.ToString() +"/"+e.TotalBytesToReceive.ToString() ;
           
        }
        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
           
            //if (parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].SubItems[3].Text.IndexOf("http") != -1)
            if(e.Error!=null)
            {
                parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].BackColor = Color.Red;
                parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].SubItems[2].Text = "Ошибка загрузки";
                //parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].SubItems[3].Text = "0%";
            }
            else
                parametrs.LV.Items[parametrs.wcl.IndexOf((WebClient)sender)].BackColor = Color.LightGreen;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Написал Частов Антон just4fun :)");
        }

        private void текстToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string adress = ((ListView)((TabControl)tabControl1.SelectedTab.Controls[0]).SelectedTab.Controls[0]).GetItemAt(parametrs.lyrics_point.X, parametrs.lyrics_point.Y).SubItems[6].Text;
            string name = ((ListView)((TabControl)tabControl1.SelectedTab.Controls[0]).SelectedTab.Controls[0]).GetItemAt(parametrs.lyrics_point.X, parametrs.lyrics_point.Y).SubItems[1].Text;
            name +=" - " + ((ListView)((TabControl)tabControl1.SelectedTab.Controls[0]).SelectedTab.Controls[0]).GetItemAt(parametrs.lyrics_point.X, parametrs.lyrics_point.Y).SubItems[2].Text;
            XmlDocument res = new XmlDocument();
            res.Load(String.Format("https://api.vkontakte.ru/method/audio.getLyrics.xml?access_token={0}&lyrics_id={1}", parametrs.access,adress));
            XmlNodeList xnl = res.GetElementsByTagName("text");
            Form_Text ft = new Form_Text(xnl[0].InnerText,name);
            ft.Show();
            
        }
    }
}
