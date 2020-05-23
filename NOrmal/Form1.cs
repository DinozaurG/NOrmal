using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NOrmal
{
    public partial class Form1 : Form
    {
        DateTime date1 = new DateTime(0, 0);
        DateTime date2 = new DateTime(0, 0);
        DateTime date3 = new DateTime(0, 0);
        public Form1()
        {
            InitializeComponent();
        }
        private void buttonAnalysis_Click(object sender, EventArgs e)
        {
            chartForAnalysis.Series[0].Points.Clear();
            chartForAnalysis.Series[1].Points.Clear();
            chartForAnalysis.Series[2].Points.Clear();
            Random rnd = new Random();
            int i;
            int n = (int)nVal.Value;
            int N = (int)NNval.Value;
            int m = (int)(Math.Log(N) + 1);
            int[] numsInInter1 = new int[m];
            int[] numsInInter2 = new int[m];
            int[] numsInInter3 = new int[m];
            for (i = 0; i < m; i++)
            {
                numsInInter1[i] = 0;
                numsInInter2[i] = 0;
                numsInInter3[i] = 0;
            }
            double[] random1 = new double[N];
            double[] random2 = new double[N];
            double[] random3 = new double[N];
            double sum = 0, E1 = 0, E2 = 0, E3 = 0, D1 = 0, D2 = 0, D3 = 0, Chi1 = 0, Chi2 = 0, Chi3 = 0;
            double max1 = -1000000000, min1 = 1000000000, max2 = -1000000000, min2 = 1000000000, max3 = -1000000000, min3 = 1000000000, MIN, MAX;
            double inter;
            timer1.Start();
            for (i = 0; i < N; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += rnd.NextDouble() % 1;
                }
                random1[i] = Math.Sqrt(12 / n) * (sum - n / 2);
                E1 += random1[i];
                if (random1[i] <= min1)
                    min1 = random1[i];
                if (random1[i] >= max1)
                    max1 = random1[i];
            }
            timer1.Stop();
            timer2.Start();
            for (i = 0; i < N; i++)
            {
                sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += rnd.NextDouble() % 1;
                }
                double tmp = Math.Sqrt(12 / n) * (sum - n / 2);
                random2[i] = tmp + (1 / 240 * (Math.Pow(tmp,3) - 3 * tmp));
                E2 += random2[i];
                if (random2[i] <= min2)
                    min2 = random2[i];
                if (random2[i] >= max2)
                    max2 = random2[i];
            }
            timer2.Stop();
            timer3.Start();
            for (i = 0; i < N; i += 2)
            {
                double b1 = rnd.NextDouble() % 1;
                double b2 = rnd.NextDouble() % 1;
                random3[i] = Math.Sqrt(-2 * Math.Log(b1)) * Math.Cos(2 * Math.PI * b2);
                random3[i + 1] = Math.Sqrt(-2 * Math.Log(b1)) * Math.Sin(2 * Math.PI * b2);
                E3 += random3[i];
                E3 += random3[i + 1];
                if (random3[i] <= min3)
                    min3 = random3[i];
                if (random3[i] >= max3)
                    max3 = random3[i];
                if (random3[i + 1] <= min3)
                    min3 = random3[i + 1];
                if (random3[i + 1] >= max3)
                    max3 = random3[i + 1];
            }
            timer3.Stop();
            E1 *= (1 / N);
            E2 *= (1 / N);
            E3 *= (1 / N);
            D1 *= (1 / N);
            D2 *= (1 / N);
            D3 *= (1 / N);
            D1 -= Math.Pow(E1, 2);
            D2 -= Math.Pow(E2, 2);
            D3 -= Math.Pow(E3, 2);
            labelForE1.Text = "Average = " + (Double)E1;
            labelForE2.Text = "Average = " + (Double)E2;
            labelForE3.Text = "Average = " + (Double)E3;
            labelForD1.Text = "Variance = " + (Double)D1;
            labelForD2.Text = "Variance = " + (Double)D2;
            labelForD3.Text = "Variance = " + (Double)D3;
            if (min1 < min2)
            {
                if (min1 < min3)
                    MIN = min1;
                else
                    MIN = min3;
            }
            else
            {
                if (min2 < min3)
                    MIN = min2;
                else
                    MIN = min3;
            }
            if (max1 > max2)
            {
                if (max1 > max3)
                    MAX = max1;
                else
                    MAX = max3;
            }
            else
            {
                if (max2 > max3)
                    MAX = max2;
                else
                    MAX = max3;
            }
            inter = (Math.Abs(MIN) + Math.Abs(MAX)) / m;
            for (i = 0; i < N; i++)
            {
                for(int j = 0; j < m - 1; j++)
                {
                    if (random1[i] > (MIN + inter * j) && random1[i] < (MIN + (inter * (j + 1))))
                    {
                        numsInInter1[j]++;
                        break;
                    }
                }
            }
            for (i = 0; i < N; i++)
            {
                for (int j = 0; j < m - 1; j++)
                {
                    if (random2[i] > (MIN + inter * j) && random2[i] < (MIN + (inter * (j + 1))))
                    {
                        numsInInter2[j]++;
                        break;
                    }
                }
            }
            for (i = 0; i < N; i++)
            {
                for (int j = 0; j < m - 1; j++)
                {
                    if (random3[i] > (MIN + inter * j) && random3[i] < (MIN + (inter * (j + 1))))
                    {
                        numsInInter3[j]++;
                        break;
                    }
                }
            }
            for (i = 0; i < m; i++)
            {
                Chi1 += Math.Pow(numsInInter1[i], 2) / (N * (numsInInter1[i] / N));
            }
            for (i = 0; i < m; i++)
            {
                Chi2 += Math.Pow(numsInInter2[i], 2) / (N * (numsInInter2[i] / N));
            }
            for (i = 0; i < m; i++)
            {
                Chi3 += Math.Pow(numsInInter3[i], 2) / (N * (numsInInter3[i] / N));
            }
            Chi1 -= N;
            Chi2 -= N;
            Chi3 -= N;
            labelForChi1.Text = "Chi-squared = " + (Double)Chi1;
            labelForChi2.Text = "Chi-squared = " + (Double)Chi2;
            labelForChi3.Text = "Chi-squared = " + (Double)Chi3;
            for (i = 0; i < m; i++)
            {
                chartForAnalysis.Series[0].Points.AddXY((((MIN + inter * i) + (inter * (i + 1))) / 2), (numsInInter1[i] / m));
            }
            for (i = 0; i < m; i++)
            {
                chartForAnalysis.Series[1].Points.AddXY((((MIN + inter * i) + (inter * (i + 1))) / 2), (numsInInter2[i] / m));
            }
            for (i = 0; i < m; i++)
            {
                chartForAnalysis.Series[2].Points.AddXY((((MIN + inter * i) + (inter * (i + 1))) / 2), (numsInInter3[i] / m));
            }
        }
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            date1 = date1.AddSeconds(1);
            labelForTime1.Text = date1.ToString("mm:ss");
        }
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            date2 = date2.AddSeconds(1);
            labelForTime2.Text = date2.ToString("mm:ss");
        }
        private void timer3_Tick_1(object sender, EventArgs e)
        {
            date3 = date3.AddSeconds(1);
            labelForTime3.Text = date3.ToString("mm:ss");
        }
    }
}
