using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Media.Animation;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for SlideshowWindow.xaml
    /// </summary>
    public partial class SlideshowWindow : Window
    {
        public int currentIndex = 0;
        //list of images to display
        public List<Item> images;
        public System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public System.Windows.Controls.Image oldImage;

        ISlideshowEffect effect;
        MainWindow mainWindow;

        public SlideshowWindow(List<Item> imgs, ISlideshowEffect slideshowEffect, MainWindow parent)
        {
            InitializeComponent();

            mainWindow = parent;
            mainWindow.WindowStyle = WindowStyle.None;

            images = imgs;
            effect = slideshowEffect;

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();

            //display first image
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Name = "myImageControl";
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(images[currentIndex].Path, UriKind.Absolute);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;
            currentIndex++;
            currentIndex %= images.Count;
            ContentRoot.Children.Add(image);
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Bottom;
            oldImage = image;
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            effect.PlaySlideshow(this, 100, 100);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }
    }
}
