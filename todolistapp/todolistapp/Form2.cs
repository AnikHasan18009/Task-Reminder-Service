using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Media;

namespace todolistapp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }
        DataTable tb = new DataTable();
       
        private void Form2_Load(object sender, EventArgs e)
        {

            tb.Columns.Add("No.", typeof(int));
            tb.Columns.Add("Todo", typeof(string));
            tb.Columns.Add("Date", typeof(string));
            tb.Columns.Add("Time", typeof(string));
           
            dataGridView1.DataSource = tb;
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 30;
            column = dataGridView1.Columns[1];
            column.Width = 300;
            while (true)
            {
                if (Form1.m.WaitOne())
                {
                    for (int i = 0; i < Program.works.Count(); i++)
                    {
                        var w = Program.works[i];
                        string[] row = new string[4];
                        row[0] = Convert.ToString(i + 1);
                        row[1] = w.name;
                        row[2] = w.date;
                        row[3] = w.hour + ":" + w.minute;
                        tb.Rows.Add(row);

                    }
                    Form1.m.ReleaseMutex();
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
           

        }

        
    }
}
