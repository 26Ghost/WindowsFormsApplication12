using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing;

namespace WindowsFormsApplication12
{
    static class parametrs
    {
        public static int app_id = 2738973;
        public static string access { get; set; }
        public static string user { get; set; }
        //public static ListView LV { get; set; }
        public static ScrollableListView LV { get; set; }
        public static Form_Download fd { get; set; }
        public static List<WebClient> wcl { get; set; }
        public static List<ProgressBar> prgb { get; set; }
        public static SortOrder sr { get; set; }
        public static Point lyrics_point { get; set; }
        public static string[, ,] str { get; set; }
    }
    
    
}
