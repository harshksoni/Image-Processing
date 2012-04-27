using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        Bitmap newbitmap;
        Image file;
        Boolean opened = false;
        int blurAmount = 1;
        int brightness = 0;
        int lastcol = 0;
        float contrast = 0;
        float gamma = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                newbitmap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
                opened = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (opened)
                {

                    file = pictureBox1.Image;
                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "bmp")
                    {
                        file.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                    }

                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "jpg")
                    {
                        file.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                    }

                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "png")
                    {
                        file.Save(saveFileDialog1.FileName, ImageFormat.Png);
                    }

                    if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3).ToLower() == "gif")
                    {
                        file.Save(saveFileDialog1.FileName, ImageFormat.Gif);
                    }

                }

                
            }
            else
            {
                MessageBox.Show("you need to open a image first");

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int x = 0 ; x < newbitmap.Width; x++)
            {

                for (int y = 0; y < newbitmap.Height; y++)
                {

                    Color originalcolor = newbitmap.GetPixel(x, y);

                    int greyScale = (int)((originalcolor.R * .3) + (originalcolor.G * .59) + (originalcolor.B * .11));
                    Color newColor = Color.FromArgb(greyScale, greyScale, greyScale);
                    newbitmap.SetPixel(x, y, newColor);

                }
            }

            pictureBox1.Image = newbitmap;


        }

        private void button4_Click(object sender, EventArgs e)
        {


            for (int x = 1; x < newbitmap.Width; x++)
            {

                for (int y = 1; y < newbitmap.Height; y++)
                {
                   try
                   {
                       Color prevX = newbitmap.GetPixel(x - 1, y);
                       Color nextX = newbitmap.GetPixel(x + 1, y);
                       Color prevY = newbitmap.GetPixel(x, y-1);
                       Color nextY = newbitmap.GetPixel(x, y+1);
                        
                        int avgR = (int)((prevX.R+prevY.R+nextX.R+nextY.R/4));
                       int avgG = (int)((prevX.G+prevY.G+nextX.G+nextY.G/4));
                       int avgB = (int)((prevX.B+prevY.B+nextX.B+nextY.B/4));


                       newbitmap.SetPixel(x,y,Color.FromArgb(avgR, avgG,avgB));
                       



                   
                   }
                    catch(Exception)

                   {

                    }
                }
            }
            pictureBox1.Image= newbitmap;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
            pictureBox1.Image = AdjustBrightness(newbitmap, trackBar1.Value);

        }

        public static Bitmap AdjustBrightness(Bitmap Image, int Value)
        {
            Bitmap TempBitmap = Image;
            float FinalValue = (float)Value / 255.0f;
           Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                     new float[] {1, 0, 0, 0, 0},
                     new float[] {0, 1, 0, 0, 0},
                     new float[] {0, 0, 1, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                 };

           ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int x = 1; x < newbitmap.Width; x++)
            {

                for (int y = 1; y < newbitmap.Height; y++)
                {

                    Color pixel = newbitmap.GetPixel(x,y);

                    int red = pixel.R;
                    int green = pixel.G;
                    int blue = pixel.B;
                    newbitmap.SetPixel(x, y, Color.FromArgb(255 - red, 255 - green, 255 - blue));
  


                }


            }
            pictureBox1.Image = newbitmap;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap nB = new Bitmap(newbitmap.Width, newbitmap.Height);

            for (int x = 1; x <= newbitmap.Width - 1; x++)
            {
                for (int y = 1; y <= newbitmap.Height - 1; y++)
                {
                    nB.SetPixel(x, y, Color.DarkGray);
                }

            }
            for (int x = 1; x <= newbitmap.Width - 1; x++)
            {
                for (int y = 1; y <= newbitmap.Height - 1; y++)
                {
                    try
                    {
                        Color pixel = newbitmap.GetPixel(x, y);
                        int colVal = (pixel.R + pixel.G + pixel.B);
                        if (lastcol == 0) lastcol = (pixel.R + pixel.G + pixel.B);
                        int diff;
                        if (colVal > lastcol)
                        {
                            diff = colVal - lastcol;
                        }
                        else
                        {
                            diff = lastcol = colVal;
                        }
                        if (diff > 100)
                        {
                            nB.SetPixel(x, y, Color.DarkBlue);
                            lastcol = colVal;

                        }


                    }
                    catch (Exception) { }
                }
            
            
            }


            for (int y = 1; y <= newbitmap.Height - 1; y++)
            {

                for (int x = 1; x <= newbitmap.Width - 1; x++)
                {
                    try
                    {
                        Color pixel = newbitmap.GetPixel(x, y);
                        int colVal = (pixel.R + pixel.G + pixel.B);

                        if (lastcol == 0) lastcol = (pixel.R + pixel.G + pixel.B);
                        int diff;
                        if (colVal > lastcol)
                        {
                            diff = colVal - lastcol;


                        }

                        else
                        {
                            diff = lastcol - colVal;
                        }
                        if (diff > 100)
                        {
                            nB.SetPixel(x, y, Color.DarkRed);
                            lastcol = colVal;

                        }

                    }
                    catch (Exception) { }



                }

            }
            pictureBox1.Image = nB;

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
            contrast = 0.04f * trackBar2.Value;
            Bitmap bm = new Bitmap(newbitmap.Width, newbitmap.Height);
            Graphics g = Graphics.FromImage(bm);
            ImageAttributes ia = new ImageAttributes();
            ColorMatrix cm = new ColorMatrix(new float[][]{
                             new float[]{contrast, 0f,0f,0f,0f},
                             new float[]{0f,contrast,0f,0f,0f},
                             new float[]{0f,0f,contrast, 0f,0f},
                             new float[]{0f,0f,0f,1f, 0f},
                             new float[]{0.001f,0.001f, 0.001f,0.001f,0f,1f}});
            ia.SetColorMatrix(cm);
            g.DrawImage(newbitmap, new Rectangle(0, 0, newbitmap.Width, newbitmap.Height), 0, 0, newbitmap.Width, newbitmap.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            ia.Dispose();
            pictureBox1.Image = bm;


        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

            label5.Text = trackBar3.Value.ToString();

            gamma = 0.04f * trackBar3.Value;
            Bitmap bm = new Bitmap(newbitmap.Width, newbitmap.Height);
            Graphics g = Graphics.FromImage(bm);
            ImageAttributes ia = new ImageAttributes();
            ia.SetGamma(gamma);
            g.DrawImage(newbitmap, new Rectangle(0, 0, newbitmap.Width, newbitmap.Height),0,0, newbitmap.Width , newbitmap.Height, GraphicsUnit.Pixel, ia);
            g.Dispose();
            ia.Dispose();
            pictureBox1.Image= bm;


        }

       

       

    }
}
