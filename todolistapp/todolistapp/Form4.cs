using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace todolistapp
{
    public partial class Form4 : Form
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
        public Form4()
        {
            
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {
            tb.Columns.Add("No.", typeof(int));
            tb.Columns.Add("Todo", typeof(string));
            tb.Columns.Add("Date", typeof(string));
            tb.Columns.Add("Time", typeof(string));
            dataGridView1.DataSource = tb;

            DataGridViewColumn column =dataGridView1.Columns[0];
            column.Width = 30;
            column = dataGridView1.Columns[1];
            column.Width = 300;
            tableToDgv();
            int l = Program.works.Count;
            for (int i = 0; i < l; i++)
            {

                comboBox3.Items.Add(Convert.ToString(i + 1));
                

            }
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    comboBox1.Items.Add("0" + Convert.ToString(i));
                    comboBox2.Items.Add("0" + Convert.ToString(i));
                }
                else
                {
                    if (i < 24)
                    {
                        comboBox1.Items.Add(Convert.ToString(i));
                    }
                    comboBox2.Items.Add(Convert.ToString(i));
                }

            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var w = Program.works[Convert.ToInt32(comboBox3.Text) - 1];
            textBox1.Text = w.name;
            comboBox1.Text = w.hour;
            comboBox2.Text = w.minute;
            dateTimePicker1.Value =DateTime.ParseExact(w.date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")
            {
                int flag = 0;
                while (true)
                {
                    if (Form1.m.WaitOne())
                    {
                        for (int i = 0; i < Program.works.Count(); i++)
                        {
                            var w = Program.works[i];
                            if (w.hour == comboBox1.Text && w.minute == comboBox2.Text && w.date == dateTimePicker1.Value.ToString("dd-MM-yyyy"))
                            {
                                flag = 1;

                                break;
                            }
                        }
                        Form1.m.ReleaseMutex();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                if (flag == 1)
                {
                    MessageBox.Show("Updated datetime has a task assigned.");
                }
                else
                {
                    Program.work w = new Program.work();
                    w.name = textBox1.Text;
                    w.hour = comboBox1.Text;
                    w.minute = comboBox2.Text;
                    w.date = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                    w.flag = false;
                    int index = Convert.ToInt32(comboBox3.Text);
                    while (true)
                    {
                        if (Form1.m.WaitOne())
                        {
                            var v=Program.works[index-1];
                            w.flag = v.flag;
                            Program.works[index - 1] = w;
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
                    MessageBox.Show("Successfully Updated.");
                }


            }
            else
            {
                MessageBox.Show("Please Enter Task Namae");
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
