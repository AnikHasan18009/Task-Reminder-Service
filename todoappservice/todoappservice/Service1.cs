using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Media;

namespace todoappservice
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        public struct Work
        {
            public string name { get; set; }
            public string hour { get; set; }
            public string minute { get; set; }
            public string date { get; set; }
            public bool flag { get; set; }            
        }
        public static List<Work> works = new List<Work>();
        public static List<Work> newworks = new List<Work>();

        int st = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadTime"]);
        public Thread t1 = null;
        public Thread t2 = null;
        Mutex m = new Mutex();
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            t1 = new Thread(updateList);
            t2 = new Thread(alarm);

            t1.Start();
            t2.Start();
        }
        public void updateList()
        {
            while (true)
            {
                if (m.WaitOne())
                {
                    int f = 0;
                    string n = "";
                        string systemTime = DateTime.Now.ToLongTimeString();
                        char[] s = { ' ' };
                        string[] time = systemTime.Split(s);
                        s = new char[] { ':' };
                        string[] hm = time[0].Split(s);
                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    for (int i = 0; i < works.Count; i++)
                    {
                        var v = works[i];
                        if (hm[0] == v.hour && hm[1] == v.minute && v.flag == true && date == v.date)
                        {
                            n = v.name;
                            f = 1;
                            break;
                        }
                    }       
                    while (true)
                    {
                        try
                        {
                            StreamReader st = new StreamReader("C:\\Users\\TACHLAND\\Desktop\\todolist.txt");
                            string line = st.ReadLine();
                            if (line != "")
                            {
                                works.Clear();
                                while (line != null)
                                {
                                    string line2 = st.ReadLine();
                                    string line3 = st.ReadLine();
                                    Work w1 = new Work();
                                    w1.name = line;
                                    char[] sep2 = { ':' };
                                    string[] hm2 = line3.Split(sep2);
                                    w1.hour = hm2[0];
                                    w1.minute = hm2[1];
                                    w1.date = line2;
                                    w1.flag = false;

                                    if (f == 1 && line == n && line2==date&& hm[0] == hm2[0] && hm[1] == hm2[1])
                                    {
                                        w1.flag = true;
                                    }
                                    works.Add(w1);
                                    line = st.ReadLine();
                                }
                            }
                            st.Close();
                            break;
                        }
                        catch(Exception)
                        {
                            Thread.Sleep(10);
                        }
                    }
                m.ReleaseMutex();
               Thread.Sleep(4000);
                }
                else
                {
                    Thread.Sleep(10);
                }

            }
        }
        public void alarm()
        {
            while (true)
            {
                if (m.WaitOne())
                {
                    
                    for (int i = 0; i < works.Count; i++)
                    {
                        string systemTime = DateTime.Now.ToLongTimeString();
                        string date = DateTime.Now.ToString("dd-MM-yyyy");
                        char[] s = { ' ' };
                        string[] time = systemTime.Split(s);
                        s = new char[] { ':' };
                        string[] hm = time[0].Split(s);
                        var v = works[i];
                        if (hm[0] == v.hour && hm[1] == v.minute && date==v.date)
                        {
                            if (v.flag == false)
                            {
                                v.flag = true;
                                SoundPlayer myPlayer = new SoundPlayer();
                                myPlayer.SoundLocation = @"C:\Users\TACHLAND\Desktop\ac.wav";
                                myPlayer.PlaySync();
                                works[i] = v;
                              
                                break;
                            }

                        }
                        else
                        {
                            v.flag = false;
                            works[i] = v;
                        }

                    }
                    
                        m.ReleaseMutex();
                   
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

        }
        protected override void OnStop()
        {
            try
            {
                if((t1!=null) && (t2!=null) && t1.IsAlive && t2.IsAlive)
                {
                    t1.Abort();
                    t2.Abort();
                }
            }
            catch(Exception)
                {
                throw;
            }
        }
    }
}
