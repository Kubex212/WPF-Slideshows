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
    public interface ISlideshowEffect
    {
        string Name { get; }
        void PlaySlideshow(SlideshowWindow window, double windowWidth, double windowHeight);
    }

    public class HorizontalEffect : ISlideshowEffect
    {
        public string Name { get; } = "Horizontal effect";

        public void PlaySlideshow(SlideshowWindow window, double windowWidth, double windowHeight)
        {
            //create Image control and put it on top
            NameScope.SetNameScope(window, new NameScope());
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Name = "myImageControl";
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(window.images[window.currentIndex].Path, UriKind.Absolute);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;
            window.currentIndex++;
            window.currentIndex %= window.images.Count;
            window.ContentRoot.Children.Add(image);
            window.ContentRoot.Children.Remove(window.oldImage);
            window.ContentRoot.Children.Add(window.oldImage);
            var contextMenu = new ContextMenu();

            //create context menu
            MenuItem item = new MenuItem();
            item.Header = "Pause/unpause slideshow";
            item.Click += delegate 
            { 
                if(window.dispatcherTimer.IsEnabled) window.dispatcherTimer.Stop();
                else window.dispatcherTimer.Start();
            };
            contextMenu.Items.Add(item);
            MenuItem item2 = new MenuItem();
            item2.Header = "Close slideshow";
            item2.Click += delegate
            {
                window.Close();
            };
            contextMenu.Items.Add(item2);
            image.ContextMenu = contextMenu;

            window.RegisterName(window.oldImage.Name, window.oldImage);

            //animate
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = window.Width;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(System.Windows.Controls.Image.WidthProperty));
            Storyboard.SetTargetName(doubleAnimation, window.oldImage.Name);
            storyboard.Begin(window.ContentRoot);
            window.oldImage = image;
        }
    }

    public class OpacityEffect : ISlideshowEffect
    {
        public string Name { get; } = "Opacity effect";

        public void PlaySlideshow(SlideshowWindow window, double windowWidth, double windowHeight)
        {
            //create Image control and put it on top
            NameScope.SetNameScope(window, new NameScope());
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Name = "myImageControl";
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(window.images[window.currentIndex].Path, UriKind.Absolute);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;
            window.currentIndex++;
            window.currentIndex %= window.images.Count;
            window.ContentRoot.Children.Add(image);

            //create context menu
            var contextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Pause/unpause slideshow";
            item.Click += delegate
            {
                if (window.dispatcherTimer.IsEnabled) window.dispatcherTimer.Stop();
                else window.dispatcherTimer.Start();
            };
            contextMenu.Items.Add(item);
            MenuItem item2 = new MenuItem();
            item2.Header = "Close slideshow";
            item2.Click += delegate
            {
                window.Close();
            };
            contextMenu.Items.Add(item2);
            image.ContextMenu = contextMenu;

            window.RegisterName(image.Name, image);

            //animate
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(System.Windows.Controls.Image.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimation, image.Name);
            storyboard.Begin(window.ContentRoot);
        }
    }

    public class VerticalEffect : ISlideshowEffect
    {
        public string Name { get; } = "Opacity effect";

        public void PlaySlideshow(SlideshowWindow window, double windowWidth, double windowHeight)
        {
            //create Image control and put it on top
            NameScope.SetNameScope(window, new NameScope());
            System.Windows.Controls.Image image = new System.Windows.Controls.Image();
            image.Name = "myImageControl";
            image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(window.images[window.currentIndex].Path, UriKind.Absolute);
            bi3.EndInit();
            image.Stretch = Stretch.Fill;
            image.Source = bi3;
            image.VerticalAlignment = VerticalAlignment.Bottom;
            window.currentIndex++;
            window.currentIndex %= window.images.Count;
            window.ContentRoot.Children.Add(image);
            window.ContentRoot.Children.Remove(window.oldImage);
            window.ContentRoot.Children.Add(window.oldImage);

            //create context menu
            var contextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Pause/unpause slideshow";
            item.Click += delegate
            {
                if (window.dispatcherTimer.IsEnabled) window.dispatcherTimer.Stop();
                else window.dispatcherTimer.Start();
            };
            contextMenu.Items.Add(item);
            MenuItem item2 = new MenuItem();
            item2.Header = "Close slideshow";
            item2.Click += delegate
            {
                window.Close();
            };
            contextMenu.Items.Add(item2);
            image.ContextMenu = contextMenu;

            window.RegisterName(window.oldImage.Name, window.oldImage);

            //animate
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = window.Height;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(System.Windows.Controls.Image.HeightProperty));
            Storyboard.SetTargetName(doubleAnimation, window.oldImage.Name);
            storyboard.Begin(window.ContentRoot);
            window.oldImage = image;
        }
    }
}
