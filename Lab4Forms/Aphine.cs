using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4Forms
{
    public partial class Aphine : Form
    {
        public Aphine()
        {
            InitializeComponent();
        }
        StringBuilder inputStringBuilder;
        private Dictionary<string, double> frequency = new Dictionary<string, double>();
        List<char> forbiddenCharacters = new List<char>()
            { '.', ',', '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '/', '\'', '\"', ':', ';', ' ', '’', '\n' };
        public static List<char> alphavit = new List<char>() { 'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л',
            'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        private readonly List<int> posibleAlpha = new List<int>() { 1, 2, 3, 4, 5, 7, 8, 10, 13, 14, 16, 17, 19, 20, 23, 25, 26, 28, 29, 31, 32 };
        private readonly List<int> posibleBeta = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

        private void Aphine_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
           
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Text is missing");
            }
            inputStringBuilder = new StringBuilder(richTextBox1.Text.ToLower());
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                if (!alphavit.Contains(inputStringBuilder[i]))
                {
                    inputStringBuilder.Remove(i, 1);
                    i--;
                }
            }

            richTextBox2.Clear();
           
                GetEncryption();
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
           
           
            
            int count = 0;
            inputStringBuilder = new StringBuilder(richTextBox1.Text);
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                count++;
                var key = inputStringBuilder[i].ToString();
                if (frequency.ContainsKey(key))
                {
                    frequency[key]++;
                    continue;
                }
                frequency.Add(key, 1);
            }

            var twoCharWithMaxFrequency = frequency.OrderByDescending(f => f.Value).Select(f => f.Key).Take(2).ToList();

            var aAndB = SolvingEquation(twoCharWithMaxFrequency);
            if (aAndB == null)
            {
                MessageBox.Show("Неможливо розшифрувати, підберіть інші літери");
            }

            GetDecryption(aAndB);
            label3.Text = $"{aAndB.Item1}";
            label4.Text = $"{aAndB.Item2}";

        }

        private void GetEncryption()
        {
            int helper;
            StringBuilder encryptedText = new StringBuilder();
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                helper = (alphavit.IndexOf(inputStringBuilder[i]) * Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text)) % alphavit.Count;
                encryptedText.Append(alphavit.ElementAt(helper));
            }
            richTextBox2.Text = encryptedText.ToString();
        }

        private Tuple<int, int> SolvingEquation(List<string> twoCharWithMaxFrequency)
        {
            int indexOfFirstChar = alphavit.IndexOf(twoCharWithMaxFrequency[0].ToCharArray()[0]);
            int indexOfSecondChar = alphavit.IndexOf(twoCharWithMaxFrequency[1].ToCharArray()[0]);

            int indexFirstMaxFreqChar = alphavit.IndexOf(textBox3.Text.ToCharArray()[0]);
            int indexSecondMaxFreqChar = alphavit.IndexOf(textBox4.Text.ToCharArray()[0]);

            for (int i = 0; i < posibleAlpha.Count; i++)
            {
                for (int j = 0; j < posibleBeta.Count; j++)
                {
                    var solution1 = (posibleAlpha[i] * indexFirstMaxFreqChar + posibleBeta[j]) % alphavit.Count == indexOfFirstChar;
                    var solution2 = (posibleAlpha[i] * indexSecondMaxFreqChar + posibleBeta[j]) % alphavit.Count == indexOfSecondChar;

                    if (solution1 && solution2)
                    {
                        return new Tuple<int, int>(posibleAlpha[i], posibleBeta[j]);
                    }
                }
            }

            return null;
        }
        private void GetDecryption(Tuple<int, int> aAndB)
        {
            int helper = 0;
            StringBuilder decryptedText = new StringBuilder();
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                helper = modInverse(aAndB.Item1, alphavit.Count) * (alphavit.IndexOf(inputStringBuilder[i]) + alphavit.Count - aAndB.Item2) % alphavit.Count;
                decryptedText.Append(alphavit.ElementAt(helper));
            }
            richTextBox2.Text = decryptedText.ToString();
        }

        private int modInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}
