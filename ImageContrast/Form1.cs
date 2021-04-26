using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageContrast
{
    public partial class Form1 : Form
    {
        private int szer = 0, wys = 0;
        public Form1()
        {
            InitializeComponent();
        }

        //załaduj zdjęcie
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                szer = pictureBox1.Image.Width;
                wys = pictureBox1.Image.Height;
                pictureBox2.Image = new Bitmap(szer, wys);
            }
        }

        //kontrast wariant 2
        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            Bitmap b2 = (Bitmap)pictureBox2.Image;
            Color k1, k2;
            double delta_c = trackBar1.Value;
            double red_p = 0;
            double green_p = 0;
            double blue_p = 0;

            for (int x = 0; x < szer; x++)
            {
                for (int y = 0; y < wys; y++)
                {
                    k1 = b1.GetPixel(x, y);

                    if (delta_c < 0)
                    {
                        if (k1.R < 127 + delta_c) red_p = ((127 / (127 + delta_c)) * (double)k1.R);
                        else if (k1.R > 127 - delta_c) red_p = ((((127 * (double)k1.R) + (255 * delta_c)) / 127) + delta_c);
                        else red_p = 127;

                        if (k1.G < 127 + delta_c) green_p = ((127 / (127 + delta_c)) * (double)k1.G);
                        else if (k1.G > 127 - delta_c) green_p = ((((127 * (double)k1.G) + (255 * delta_c)) / 127) + delta_c);
                        else green_p = 127;

                        if (k1.B < 127 + delta_c) blue_p = ((127 / (127 + delta_c)) * (double)k1.B);
                        else if (k1.B > 127 - delta_c) blue_p = ((((127 * (double)k1.B) + (255 * delta_c)) / 127) + delta_c);
                        else blue_p = 127;
                    }
                    else if (delta_c >= 0)
                    {
                        if (k1.R < 127) red_p = (((127 - delta_c) / 127) * (double)k1.R);
                        if (k1.R >= 127) red_p = ((((127 - delta_c) / 127) * (double)k1.R) + (2 * delta_c));

                        if (k1.G < 127) green_p = (((127 - delta_c) / 127) * (double)k1.G);
                        if (k1.G >= 127) green_p = ((((127 - delta_c) / 127) * (double)k1.G) + (2 * delta_c));

                        if (k1.B < 127) blue_p = (((127 - delta_c) / 127) * (double)k1.B);
                        if (k1.B >= 127) blue_p = ((((127 - delta_c) / 127) * (double)k1.B) + (2 * delta_c));
                    }
                    if (red_p > 255) red_p = 255;
                    if (green_p > 255) green_p = 255;
                    if (blue_p > 255) blue_p = 255;
                    if (red_p < 0) red_p = 0;
                    if (green_p < 0) green_p = 0;
                    if (blue_p < 0) blue_p = 0;
                    k2 = Color.FromArgb((int)red_p, (int)green_p, (int)blue_p);
                    b2.SetPixel(x, y, k2);
                }
            }
            pictureBox2.Invalidate();

            int[] r = new int[256];
            int[] g = new int[256];
            int[] b = new int[256];
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            for (int i = 0; i < b2.Height; i++)
            {
                for (int j = 0; j < b2.Width; j++)
                {
                    Color pixel = b2.GetPixel(j, i);
                    r[pixel.R]++;
                    g[pixel.G]++;
                    b[pixel.B]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                chart1.Series[0].Points.AddXY(i, r[i]);
                chart1.Series[1].Points.AddXY(i, g[i]);
                chart1.Series[2].Points.AddXY(i, b[i]);
            }
            chart1.Invalidate();
        }


        //zapisz zdjecie
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        pictureBox2.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }

        //konstrast wariant 1
        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap b1 = (Bitmap)pictureBox1.Image;
            Bitmap b2 = (Bitmap)pictureBox2.Image;
            Color k1, k2;
            double delta_c = trackBar1.Value;
            double red_p = 0;
            double green_p = 0;
            double blue_p = 0;

            for (int x = 0; x < szer; x++)
            {
                for (int y = 0; y < wys; y++)
                {
                    k1 = b1.GetPixel(x, y);
                    if (delta_c < 0)
                    {
                        red_p = (((127 + delta_c) / 127) * (k1.R - delta_c));
                        green_p = (((127 + delta_c) / 127) * (k1.G - delta_c));
                        blue_p = (((127 + delta_c) / 127) * (k1.B - delta_c));
                    }
                    else if(delta_c >= 0)
                    {
                        red_p = ((127 / (127 - delta_c)) * (k1.R - delta_c));
                        green_p = ((127 / (127 - delta_c)) * (k1.G - delta_c));
                        blue_p = ((127 / (127 - delta_c)) * (k1.B - delta_c));
                    }
                    if (red_p > 255) red_p = 255;
                    if (green_p > 255) green_p = 255;
                    if (blue_p > 255) blue_p = 255;
                    if (red_p < 0) red_p = 0;
                    if (green_p < 0) green_p = 0;
                    if (blue_p < 0) blue_p = 0;
                    k2 = Color.FromArgb((int)red_p, (int)green_p, (int)blue_p);
                    b2.SetPixel(x, y, k2);
                }
            }
            pictureBox2.Invalidate();

            int[] r = new int[256];
            int[] g = new int[256];
            int[] b = new int[256];
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            for (int i = 0; i < b2.Height; i++)
            {
                for (int j = 0; j < b2.Width; j++)
                {
                    Color pixel = b2.GetPixel(j, i);
                    r[pixel.R]++;
                    g[pixel.G]++;
                    b[pixel.B]++;
                }
            }
            for (int i = 0; i < 256; i++)
            {
                chart1.Series[0].Points.AddXY(i, r[i]);
                chart1.Series[1].Points.AddXY(i, g[i]);
                chart1.Series[2].Points.AddXY(i, b[i]);
            }
            chart1.Invalidate();
        }
    }
}
