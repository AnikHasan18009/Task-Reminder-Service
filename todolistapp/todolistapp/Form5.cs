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
    public partial class Form5 : Form
    {
        public DataTable tb = new DataTable();
        public void tableToDgv()
        {
            while (true)
            {
                if (Form1.m.WaitOne())
                {
                    tb.Rows.Clear();
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
                    System.Threading.Thread.Sleep(10);
                }
            }
            return;
        }
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            tb.Columns.Add("No.", typeof(int));
            tb.Columns.Add("Task", typeof(string));
            tb.Columns.Add("Date", typeof(string));
            tb.Columns.Add("Time", typeof(string));
            dataGridView1.DataSource = tb;

            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 30;
            column = dataGridView1.Columns[1];
            column.Width = 300;
            tableToDgv();
            int l = Program.works.Count;
            for (int i = 0; i < l; i++)
            {

                comboBox3.Items.Add(Convert.ToString(i + 1));


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r= MessageBox.Show("Sure?", "Delete", MessageBoxButtons.YesNo);
            if(r== DialogResult.Yes)
            {
                int index = Convert.ToInt32(comboBox3.Text);
                while (true)
                {
                    if (Form1.m.WaitOne())
                    {
                        Program.works.RemoveAt(index - 1);
                        Form1.m.ReleaseMutex();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                tableToDgv();
                while (true)
                {
                    try
                    {
                        StreamWriter s2 = new StreamWriter("C:\\Users\\TACHLAND\\Desktop\\todolist.txt");
                        for (int i = 0; i < Program.works.Count; i++)
                        {
                            var cw = Program.works[i];
                            s2.WriteLine(cw.name);
                            s2.WriteLine(cw.date);
                            s2.WriteLine(cw.hour + ":" + cw.minute);
                        }
                        s2.Close();
                        break;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                int l = Program.works.Count;
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                for (int i = 0; i < l; i++)
                {

                    comboBox3.Items.Add(Convert.ToString(i + 1));


                }
                MessageBox.Show("Successfully deleted.");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
