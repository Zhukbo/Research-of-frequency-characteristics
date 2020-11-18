using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4Forms
{
    public partial class Matrix : Form
    {
        Dictionary<string, int> bigram ;
        public Matrix(Dictionary<string, int> bigram)
        {
            InitializeComponent();
             this.bigram = bigram;         
        }

        private void Matrix_Load(object sender, EventArgs e)
        {
            ArrayList x = new ArrayList();
            ArrayList y = new ArrayList();
            
            foreach (KeyValuePair<string, int> keyValue in bigram)
            {
               
                if (!x.Contains(keyValue.Key[0]))
                {
                    x.Add(keyValue.Key[0]);
                }
                if (!y.Contains(keyValue.Key[1]))
                {
                    y.Add(keyValue.Key[1]);
                }

            }


            dataGridView1.Size = new Size(1200, 900);
            dataGridView1.RowCount = x.Count;
            dataGridView1.ColumnCount = y.Count;
            int i, j;
            for (i = 0; i < x.Count; ++i)
            {
                for (j = 0; j < y.Count; ++j)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.DarkGray;
                    foreach (KeyValuePair<string, int> keyValue in bigram)
                    {
                        if(keyValue.Key == String.Concat(x[i], y[j]))
                        {
                            if (keyValue.Value > 9)
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Green;
                            }
                            else if (keyValue.Value < 10 && keyValue.Value > 4)
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow; 
                            }
                            else
                            {
                                dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            }
                        }
                    }                  
                }
            }

            for (int k = 0; k < y.Count; k++)
            {
                dataGridView1.Columns[k].Name = $"{y[k]}";
                dataGridView1.Columns[k].Width = 30;
            }
            for (int k = 0; k < x.Count; k++)
            {
                dataGridView1.Rows[k].HeaderCell.Value = $"{x[k]}";
            }
           
        }

        private void Matrix_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 obj = new Form1();
            obj.Show();
            this.Hide();
        }
    }
}
