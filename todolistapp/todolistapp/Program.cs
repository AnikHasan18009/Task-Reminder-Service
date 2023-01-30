using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using Microsoft.Win32;

namespace todolistapp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public struct work
    {
        public string name { get; set; }
        public string hour { get; set; }
        public string minute { get; set; }
        public string date { get; set; }
        public bool flag { get; set; }

        

   
    }
       public static List<work> works = new List<work>();
        [STAThread]
        static void Main()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

           
            rkApp.SetValue("TodoApp", Application.ExecutablePath.ToString());

            while (true)
            {
                try
                {
                    StreamReader st = new StreamReader("C:\\Users\\TACHLAND\\Desktop\\todolist.txt");
                    string line = st.ReadLine();
                    if (line != "")
                    {
                        while (line != null)
                        {
                            string line2 = st.ReadLine();
                            string line3 = st.ReadLine();
                            work w1 = new work();
                            w1.name = line;
                            char[] sep2 = { ':' };
                            string[] hm = line3.Split(sep2);
                            w1.hour = hm[0];
                            w1.minute = hm[1];
                            w1.flag = false;
                            w1.date = line2;
                            works.Add(w1);

                            line = st.ReadLine();
                        }
                    }
                        st.Close();
                        break;
                    
                   
                }
                catch (Exception e)
                {
                    Thread.Sleep(10);

                }
            }
                Application.EnableVisualStyles();
           
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        
    }
}
