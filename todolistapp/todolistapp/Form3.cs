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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
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
                    Thread.Sleep(10);
                }
            }
            return;
        }
        private void Form3_Load(object sender, EventArgs e)
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
            tableToDgv();
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") {
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
                    MessageBox.Show("This datetime has a task assigned.");
                }
                else
                {
                    Program.work w = new Program.work();
                    w.name = textBox1.Text;
                    w.hour = comboBox1.Text;
                    w.minute = comboBox2.Text;
                    w.date = dateTimePicker1.Value.ToString("dd-MM-yyyy");
                    w.flag = false;
                    while(true)
                    {
                        if(Form1.m.WaitOne())
                        {
                            Program.works.Add(w);

                            Form1.m.ReleaseMutex();
                            break;
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    tableToDgv();
                    while(true)
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
                        catch(Exception)
                        {
                            throw;
                        }
                    }
                    MessageBox.Show("Successfully Added.");
                }
            }
            else
            {
                MessageBox.Show("Please Enter the task.");
            }

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
