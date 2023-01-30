using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace todolistapp
{

   
    public partial class Form1 : Form
    {
        public static Mutex m = new Mutex();
        public static string cw = null;
        public static Thread notification = null;
        public static Thread updateList = null;
        public Form1()
        {
            InitializeComponent();
            label3.Text = DateTime.Now.ToLongDateString();
            timer1.Start();
            notification = new Thread(notify);
            updateList = new Thread(listUpdate);
            updateList.Start();
            notification.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Form4().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }
        public void listUpdate()
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
                    for (int i = 0; i < Program.works.Count; i++)
                    {
                        var v = Program.works[i];
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
                                Program.works.Clear();
                                while (line != null)
                                {
                                    string line2 = st.ReadLine();
                                    string line3 = st.ReadLine();
                                    Program.work w1 = new Program.work();
                                    w1.name = line;
                                    char[] sep2 = { ':' };
                                    string[] hm2 = line3.Split(sep2);
                                    w1.hour = hm2[0];
                                    w1.minute = hm2[1];
                                    w1.date = line2;
                                    w1.flag = false;

                                    if (f == 1 && line == n && line2 == date && hm[0] == hm2[0] && hm[1] == hm2[1])
                                    {
                                        w1.flag = true;
                                    }
                                    Program.works.Add(w1);
                                    line = st.ReadLine();
                                }
                            }
                            st.Close();
                            break;
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(5);
                        }
                    }
                    m.ReleaseMutex();
                    Thread.Sleep(4000);
                }
                else
                {
                    Thread.Sleep(5);
                }

            }
        }

        public void notify()
        {
            while(true)
            {
                if (m.WaitOne()) { 
                    for(int i = 0; i <Program.works.Count; i++)
                    {
                    string systemTime = DateTime.Now.ToLongTimeString();
                    char[] s = { ' ' };
                    string[] time = systemTime.Split(s);
                    s = new char[] { ':' };
                    string[] hm = time[0].Split(s);
                        string date = DateTime.Now.ToString("dd-MM-yyyy");
                    var v = Program.works[i];
                    if (hm[0] == v.hour && hm[1] == v.minute && date==v.date)
                    {
                        if (v.flag == false)
                        {
                            v.flag = true;
                                notifyIcon1.Visible = true;
                                notifyIcon1.BalloonTipTitle = "Task Reminder";
                                notifyIcon1.BalloonTipText = v.name;
                                notifyIcon1.Icon = SystemIcons.Information;
                                notifyIcon1.Text = "Task Reminder";
                                notifyIcon1.ShowBalloonTip(30000);
                                cw = v.name;
                            Program.works[i] = v;

                            break;
                        }

                    }
                    else
                    {
                        v.flag = false;
                        Program.works[i] = v;
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

    

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(cw);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible=false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            notification.Abort();
            updateList.Abort();
            Close();
            notifyIcon1.Dispose();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

             notification.Abort();
            updateList.Abort();
            notifyIcon1.Dispose();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form5().Show();
        }
    }

}

