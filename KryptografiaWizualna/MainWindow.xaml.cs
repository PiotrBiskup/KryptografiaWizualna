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
            teoriaTextBlock.Text = "Wczytujemy dowolny obraz za pomocą przycisku Wczytaj z pliku. Sprawdzane jest czy zdjęcie jest czarno białe. Jeśli nie jest, zdjęcie zmieniane jest na monochromatyczne. Maksymalny rozmiar wczytanego zdjęcia to 4000 x 4000 px. Po naduszeniu przycisku Szyfruj otrzymujemy dwa obrazy o dwa razy większej szerokości. Zaszyfrowany w nich jest obraz wejściowy. Można również wczytać dwa obrazy, które są czarno biało bitmapą. Po naduszeniu Deszyfruj otrzymujemy obraz pierwotny. Za pomocą przycisków Zapisz możemy zapisać do pliku otrzymane obrazy.\n\nTeroia: program sprawdza po kolei każdy pikselel obrazu.Jeżeli jest czarny to z prawdopodbieństwem 0,5 dodaje do udziału pierwszego piksel czarny i biały, a do udziało drugiego piksel biały i czarny lub z prawdopodbieństwem 0,5 dodaje do udziału pierwszego piksel biały i czarny, a do udziału drugiego czarmy i biały. Po nałożeniu na siebie obu udziałów otrzymujemy kolor czarny. Natomiast jeżeli piksel w obrazie źródłowym jest biały to z prawdopodbieństwem 0,5 dodajemy do udziału pierwszego i drugiego piksele czarny i biały lub z prawdopodbieństwem 0,5 dodajemy do udziału pierwsze i drugiego piksele biały i czarny.Po nałożeniu na siebie udziałów otrzymujemy kolor szary.";
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
            szyfruj();

            ImageInButton1.Source = BitmapToBitmapImage(bitmap1);
            ImageInButton2.Source = BitmapToBitmapImage(bitmap2);

        }

        private void szyfruj()
        {
            bitmap1 = new Bitmap(2 * widthSrc, heightSrc, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bitmap2 = new Bitmap(2 * widthSrc, heightSrc, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            System.Drawing.Color pixel;
            int color_value;
            Random rand = new Random();

            for(int i = 0; i < heightSrc; i++)
            {
                for(int j = 0; j < widthSrc; j++)
                {
                    pixel = bitmapSrc.GetPixel(j, i);
                    color_value = pixel.R;

                    if(color_value == 255)
                    {
                        wypelnij(j, i, false, rand.Next(1, 3));

                    } else
                    {
                        wypelnij(j, i, true, rand.Next(1, 3));
                    }

                }
            }

        }

        private void wypelnij(int j, int i, bool isBlack, int rand)
        {
            if(isBlack)
            {
                switch (rand)
                {
                    case 1:

                        bitmap1.SetPixel(j * 2, i, System.Drawing.Color.Black);
                        bitmap1.SetPixel((j * 2) + 1, i, System.Drawing.Color.White);

                        bitmap2.SetPixel(j * 2, i, System.Drawing.Color.White);
                        bitmap2.SetPixel((j * 2) + 1 , i, System.Drawing.Color.Black);

                        break;

                    case 2:

                        bitmap1.SetPixel(j * 2, i, System.Drawing.Color.White);
                        bitmap1.SetPixel((j * 2) + 1, i, System.Drawing.Color.Black);

                        bitmap2.SetPixel(j * 2, i, System.Drawing.Color.Black);
                        bitmap2.SetPixel((j * 2) + 1, i, System.Drawing.Color.White);

                        break;
                }
            }
            else
            {
                switch (rand)
                {
                    case 1:

                        bitmap1.SetPixel(j * 2, i, System.Drawing.Color.Black);
                        bitmap1.SetPixel((j * 2) + 1 , i, System.Drawing.Color.White);

                        bitmap2.SetPixel(j * 2, i, System.Drawing.Color.Black);
                        bitmap2.SetPixel((j * 2) + 1 , i , System.Drawing.Color.White);

                        break;

                    case 2:

                        bitmap1.SetPixel(j * 2, i, System.Drawing.Color.White);
                        bitmap1.SetPixel((j * 2) + 1, i, System.Drawing.Color.Black);

                        bitmap2.SetPixel(j * 2, i, System.Drawing.Color.White);
                        bitmap2.SetPixel((j * 2) + 1 , i ,  System.Drawing.Color.Black);

                        break;
                }
            }
        }

        private void DeszyfrujButton_Click(object sender, RoutedEventArgs e)
        {
            if(bitmap1 != null && bitmap2 != null && bitmap1.Height == bitmap2.Height && bitmap1.Width == bitmap2.Width)
            {
                deszyfruj();
                ImageInButtonSrc.Source = BitmapToBitmapImage(bitmapSrc);

            } else
            {
                MessageBox.Show("Błąd! Różne rozmiary udziałów.");
            }
        }

        private void deszyfruj()
        {
            Bitmap combined = new Bitmap(bitmap1.Width / 2, bitmap1.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for(int i = 0; i < combined.Height; i++)
            {
                for(int j = 0; j < combined.Width; j++)
                {
                    if (bitmap1.GetPixel((j * 2), i) == bitmap2.GetPixel((j * 2), i) &&
                        bitmap1.GetPixel((2 * j) + 1, i) == bitmap2.GetPixel((j * 2) + 1, i))
                        combined.SetPixel(j, i, System.Drawing.Color.White);
                    else
                        combined.SetPixel(j, i, System.Drawing.Color.Black);
                }
            }

            bitmapSrc = combined;
            heightSrc = bitmapSrc.Height;
            widthSrc = bitmapSrc.Width;

        }



        private void SaveImgSrcButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PNG(*.png)|*.png|BMP(*.bmp)|*.bmp";
                if (dialog.ShowDialog() == true)
                {
                    var extension = System.IO.Path.GetExtension(dialog.FileName);
                    switch (extension.ToLower())
                    {
                        case ".png":
                            bitmapSrc.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            bitmapSrc.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zapisanie pliku nie udało sie!");
            }
        }

        private void SaveImg1Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                //dialog.Filter = "PNG(*.png)|*.png|BMP(*.bmp)|*.bmp";
                dialog.Filter = "BMP(*.bmp)|*.bmp|PNG(*.png)|*.png";
                if (dialog.ShowDialog() == true)
                {
                    var extension = System.IO.Path.GetExtension(dialog.FileName);
                    switch (extension.ToLower())
                    {
                        case ".png":
                            bitmap1.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            bitmap1.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zapisanie pliku nie udało sie!");
            }
        }

        private void SaveImg2Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog dialog = new SaveFileDialog();
                // dialog.Filter = "PNG(*.png)|*.png|BMP(*.bmp)|*.bmp";
                dialog.Filter = "BMP(*.bmp)|*.bmp|PNG(*.png)|*.png";
                if (dialog.ShowDialog() == true)
                {
                    var extension = System.IO.Path.GetExtension(dialog.FileName);
                    switch (extension.ToLower())
                    {
                        case ".png":
                            bitmap2.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                        case ".bmp":
                            bitmap2.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(extension);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zapisanie pliku nie udało sie!");
            }
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
