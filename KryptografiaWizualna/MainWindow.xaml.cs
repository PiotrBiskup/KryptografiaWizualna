using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;

namespace KryptografiaWizualna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap bitmapSrc;
        private Bitmap bitmap1;
        private Bitmap bitmap2;
        private int widthSrc;
        private int heightSrc;
        private String pixelFormat;
        private String bitmapSrcFormat;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImageButtonSrc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp *.jpeg *.png *.jpg) | *.bmp; *.jpeg; *.png; *.jpg";

            if (ofd.ShowDialog() == true)
            {

                bitmapSrc = (Bitmap)System.Drawing.Image.FromFile(ofd.FileName);
                widthSrc = bitmapSrc.Width;
                heightSrc = bitmapSrc.Height;
                pixelFormat = bitmapSrc.PixelFormat.ToString();
                bitmapSrcFormat = bitmapSrc.RawFormat.ToString();
                if(widthSrc <= 4000 && heightSrc <= 4000)
                {
                    bitmapSrc = Binarny(bitmapSrc);
                    ImageInButtonSrc.Source = BitmapToBitmapImage(bitmapSrc);
                    Console.WriteLine(CzyBinarny(bitmapSrc));
                } else
                {
                    bitmapSrc = null;
                    MessageBox.Show("Wczytany obraz jest za duży. Maksymalny rozmiar to 4000 x 4000 px.");
                }
            }
        }

        private void ImageButton1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp) | *.bmp";

            if (ofd.ShowDialog() == true)
            {

                bitmap1 = (Bitmap)System.Drawing.Image.FromFile(ofd.FileName);

                if (bitmap1.Height <= 8000 && bitmap1.Width <= 8000)
                {
                    if (!CzyBinarny(bitmap1))
                    {
                        bitmap1 = null;
                        MessageBox.Show("Obraz nie jest monochromatyczny");
                    }
                    else
                    {
                        ImageInButton1.Source = BitmapToBitmapImage(bitmap1);
                    }
                }
                else
                {
                    MessageBox.Show("Obraz jes za duży! Maksymalny rozmiar 8000 x 8000 px");
                }
            }
        }

        private void ImageButton2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp) | *.bmp";

            if (ofd.ShowDialog() == true)
            {

                bitmap2 = (Bitmap)System.Drawing.Image.FromFile(ofd.FileName);
               
                if (bitmap2.Height <= 8000 && bitmap2.Width <= 8000)
                {
                    if (!CzyBinarny(bitmap2))
                    {
                        bitmap2 = null;
                        MessageBox.Show("Obraz nie jest monochromatyczny");
                    } else
                    {
                        ImageInButton2.Source = BitmapToBitmapImage(bitmap2);
                    }
                }
                else
                {
                    MessageBox.Show("Obraz jes za duży! Maksymalny rozmiar 8000 x 8000 px");
                }
            }
        }

        private void SzyfrujButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeszyfrujButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveImgSrcButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveImg1Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveImg2Button_Click(object sender, RoutedEventArgs e)
        {

        }

        public Byte[] ImageToByte(BitmapImage imageSource)
        {
            byte[] data;
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        public Bitmap Binarny(Bitmap bmp)
        {
            Bitmap pom = new Bitmap(widthSrc, heightSrc, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            System.Drawing.Color pixel;
            int S, R, G, B = 0;
            for (int i = 0; i < heightSrc; i++)
            {
                for (int j = 0; j < widthSrc; j++)
                {
                    pixel = bitmapSrc.GetPixel(j, i);
                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;
                    S = (R + G + B) / 3;
                    if (S > 127) //Poprawić jak by nie byl czarno bialy
                        pom.SetPixel(j, i, System.Drawing.Color.White);
                    else
                        pom.SetPixel(j, i, System.Drawing.Color.Black);
                }
            }
            return pom;
        }

        public bool CzyBinarny(Bitmap bmp)
        {
            System.Drawing.Color pixel;
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    pixel = bmp.GetPixel(j, i);
                    if (pixel.Name != "ffffffff")
                    {
                        if (pixel.Name != "ff000000")
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private BitmapImage BitmapToBitmapImage (Bitmap src)
        {
            using (var memory = new MemoryStream())
            {
                src.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
