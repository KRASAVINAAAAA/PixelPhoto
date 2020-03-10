using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace photoPIXEL
{
    public partial class Form1 : Form
    {
        private List<Bitmap> _bitmaps = new List<Bitmap>(); //экземпляр
        private Random _random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = null;
                _bitmaps.Clear(); //очищаем старую картинку
                var bitmap = new Bitmap(openFileDialog1.FileName);
                RunProcessing(bitmap);
            }
        }

        private void RunProcessing(Bitmap bitmap)
        {
            var pixels = GetPixels(bitmap);
            var pixelsInStep = (bitmap.Width * bitmap.Height) / 100;
            var currentPixelSet = new List<Pixel>(pixels.Count - pixelsInStep);
            for (int i = 1; i < trackBar1.Maximum; i++)
            {
                for(int j = 0; j < pixelsInStep; j++)
                {
                    var index = _random.Next(pixels.Count);
                    currentPixelSet.Add(pixels[index]);
                    pixels.RemoveAt(index);
                }
                var currentBitmap = new Bitmap(bitmap.Width, bitmap.Height);

                foreach (var pixel in currentPixelSet)
                    currentBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
                _bitmaps.Add(currentBitmap);
            }

            _bitmaps.Add(bitmap);
        }

        private List<Pixel> GetPixels(Bitmap bitmap) //возврат из класса
        {
            var pixels = new List<Pixel>(bitmap.Width * bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    pixels.Add(new Pixel()      //будет добавляться новый объект типо пикселя 
                    {
                        Color = bitmap.GetPixel(x, y),       //получаем координаты x и y
                        Point = new Point() { X = x, Y = y }
                    }); 
                }
            }
            return pixels;
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (_bitmaps == null || _bitmaps.Count == 0)
                return;
            pictureBox1.Image = _bitmaps[trackBar1.Value - 1];  //берем элемент из битмапов, которое будет соответстваовать трек бару
        }
    }
}
