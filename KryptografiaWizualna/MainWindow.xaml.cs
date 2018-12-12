using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace KryptografiaWizualna
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage bitmapSrc;
        private BitmapImage bitmap1;
        private BitmapImage bitmap2;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImageButtonSrc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp) | *.bmp";

            if (ofd.ShowDialog() == true)
            {

                bitmapSrc = new BitmapImage(new Uri(ofd.FileName));
                ImageInButtonSrc.Source = bitmapSrc;

            }
        }

        private void ImageButton1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp) | *.bmp";

            if (ofd.ShowDialog() == true)
            {

                bitmap1 = new BitmapImage(new Uri(ofd.FileName));
                ImageInButton1.Source = bitmap1;

            }
        }

        private void ImageButton2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Open Image";
            ofd.Filter = "Image files (*.bmp) | *.bmp";

            if (ofd.ShowDialog() == true)
            {

                bitmap2 = new BitmapImage(new Uri(ofd.FileName));
                ImageInButton2.Source = bitmap2;

            }
        }
    }
}
