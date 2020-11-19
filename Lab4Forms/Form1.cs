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
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace Lab4Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            radioButton1.Checked= true;
            
        }
        Dictionary<char, int> openWith;
        public Dictionary<string, int> bigram;
        Dictionary<string, int> trigram;
        string signs = " 1234567890:;'*/?,.!{}[]—-…\"\n\r";

        int total = 0;
        int total_Bigram = 0;
        int total_Trigram = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            const string directory = @"D:\zemlya1.txt";
            chart1.Series["S1"].Points.Clear();
            if (radioButton1.Checked)
            {
                MethodOneSymbol(directory);
            }
            else if (radioButton2.Checked)
            {
                Bigram(directory);
            }
            else
            {
                Trigram(directory);
            }
            
        }
        public void MethodOneSymbol(string directory)
        {
           
            int i = 0;
            double probab = 0.0;
          
            openWith = new Dictionary<char, int>();
            openWith.Clear();
            chart1.Series["S1"].Points.Clear();
            using (var obj = new StreamReader(directory))
            {

                while (i > -1)
                {
                    i = obj.Read();
                    if (i == -1) { break; }
                    if (signs.IndexOf(Convert.ToChar(i)) != -1) { }
                    else
                    {
                        total++;
                        if (!openWith.ContainsKey(Convert.ToChar(i)))
                        {
                            openWith.Add(Convert.ToChar(i), 1);
                        }
                        else
                        {
                            openWith[Convert.ToChar(i)] += 1;
                        }
                    }
                }

                foreach (KeyValuePair<char, int> keyValue in openWith)
                {
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {(Math.Round(probab, 5)) * 100} %", keyValue.Value);
                }
            }
        }
        public void Bigram(string directory)
        {
            double probab = 0.0;
            byte j = 0;
            string text = "";

            string a = "";
            string b = "";

            bigram = new Dictionary<string, int>();
            using (var obj = new StreamReader(directory))
            {
                text = obj.ReadToEnd();
            }

            for (total_Bigram = 0; total_Bigram < text.Length; total_Bigram++)
            {
                if (total_Bigram + 1 == text.Length) { break; }
                a = Convert.ToString(text[total_Bigram]);
                b = Convert.ToString(text[total_Bigram + 1]);
                if (signs.IndexOf(Convert.ToChar(text[total_Bigram])) != -1 || signs.IndexOf(Convert.ToChar(text[total_Bigram + 1])) != -1) { }
                else
                {
                    if (!bigram.ContainsKey(a + b))
                    {
                        bigram.Add((a + b), 1);
                    }
                    else
                    {
                        bigram[a + b] += 1;
                    }
                }
            }


            foreach (KeyValuePair<string, int> keyValue in bigram)
            {
                if (j == 40) break;
                probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Bigram));
                chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {(Math.Round(probab, 5))*100} %", keyValue.Value);
                //Console.WriteLine(keyValue.Key + "-> " + keyValue.Value + " ймовірність = " + probab);
                 j++;
            }            
        }
        public void Trigram(string directory)
        {
            double probab = 0.0;
            byte j = 0;
            string text = "";

            string a = "";
            string b = "";
            string c = "";

            trigram = new Dictionary<string, int>();
            
            using (var obj = new StreamReader(directory))
            {
                text = obj.ReadToEnd();
            }

            for (total_Trigram = 0; total_Trigram < text.Length; total_Trigram++)
            {
                if (total_Trigram + 2 == text.Length) { break; }
                a = Convert.ToString(text[total_Trigram]);
                b = Convert.ToString(text[total_Trigram + 1]);
                c = Convert.ToString(text[total_Trigram + 2]);
                if (signs.IndexOf(Convert.ToChar(text[total_Trigram])) != -1 || signs.IndexOf(Convert.ToChar(text[total_Trigram + 1])) != -1 || signs.IndexOf(Convert.ToChar(text[total_Trigram + 2])) != -1) { }
                else
                {
                    if (!trigram.ContainsKey(a + b + c))
                    {
                        trigram.Add((a + b + c), 1);
                    }
                    else
                    {
                        trigram[a + b + c] += 1;
                    }
                }
            }


            foreach (KeyValuePair<string, int> keyValue in trigram)
            {
                if (j == 40) break;
                probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Trigram));
                chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {(Math.Round(probab, 5)) * 100} %", keyValue.Value);
                j++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte i = 0;
            if (radioButton1.Checked)
            {
                double probab = 0.0;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in openWith.OrderByDescending(keyValue => keyValue.Value))
                {
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5)} %", keyValue.Value);
                }
            }
            else if (radioButton2.Checked)
            {               
                double probab = 0.0;
                var color = Color.Green;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in bigram.OrderByDescending(keyValue => keyValue.Value))
                {
                    if (i == 30) break;
                    if (i < 3) color = Color.Green;
                    else if (i > 2 && i < 10) color = Color.Yellow;
                    else color = Color.Red;
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Bigram));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5) * 100} %", keyValue.Value);
                    chart1.Series["S1"].Points[i].Color = color;
                    i++;
                }
            }
            else
            {
                double probab = 0.0;
                i = 0;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in trigram.OrderByDescending(keyValue => keyValue.Value))
                {
                    if (i == 30) break;
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Trigram));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5)*100} %", keyValue.Value);
                    i++; 
                }
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte i = 0;
            if (radioButton1.Checked)
            {
                double probab = 0.0;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in openWith.OrderBy(keyValue => keyValue.Key))
                {
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5)} %", keyValue.Value);
                }
            }
            else if(radioButton2.Checked)
            {
                double probab = 0.0;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in bigram.OrderBy(keyValue => keyValue.Key))
                {
                    if (i == 40) break;
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Bigram));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5)} %", keyValue.Value);
                    i++;
                }
            }
            else
            {
                double probab = 0.0;
                chart1.Series["S1"].Points.Clear();
                foreach (var keyValue in trigram.OrderBy(keyValue => keyValue.Key))
                {
                    if (i == 40) break;
                    probab = Convert.ToDouble(Convert.ToDouble(keyValue.Value) / Convert.ToDouble(total_Trigram));
                    chart1.Series["S1"].Points.AddXY($"{keyValue.Key} {Math.Round(probab, 5)} %", keyValue.Value);
                    i++;
                }
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Matrix qwer = new Matrix(bigram);

            qwer.Show();
            this.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Aphine obj = new Aphine();
            obj.Show();
            this.Hide();
        }
    }
}
