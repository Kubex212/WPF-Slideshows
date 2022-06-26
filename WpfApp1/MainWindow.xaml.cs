using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyDataContext dataContext;
        public MainWindow()
        {
            //initializing tree view data and DataContext
            InitializeComponent();
            var items = new List<Item>();
            var drivesArr = Environment.GetLogicalDrives();
            var drives = new List<string>(drivesArr);
            for(int i = 0; i < drives.Count; i++)
            {
                try
                {
                    items.Add(new DirectoryItem(drives[i], drives[i]));
                }
                catch(IOException)
                {
                    drives.Remove(drives[i]);
                }
            }

            DataContext = dataContext = new MyDataContext()
            {
                Left = items,
                Right = null,
            };
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This is some application.");
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    var items = ItemProvider.GetFileItems(dialog.SelectedPath);
                    dataContext.Right = items;
                    DataContext = null;
                    DataContext = dataContext;
                }
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileItem SelectedItem = (FileItem)listView.SelectedItem;
            if (SelectedItem == null)
            {
                table.Visibility = Visibility.Collapsed;
                noFile.Visibility = Visibility.Visible;
            }
            else
            {
                table.Visibility = Visibility.Visible;
                noFile.Visibility = Visibility.Collapsed;
            }    
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            if (dataContext.Right == null || dataContext.Right.Count == 0)
            {
                MessageBox.Show("Please select a directory that contains some pictures!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var ssw = new SlideshowWindow(dataContext.Right, new HorizontalEffect(), this);
            ssw.Show();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (dataContext.Right == null || dataContext.Right.Count == 0)
            {
                MessageBox.Show("Please select a directory that contains some pictures!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var ssw = new SlideshowWindow(dataContext.Right, new VerticalEffect(), this);
            ssw.Show();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (dataContext.Right == null || dataContext.Right.Count == 0)
            {
                MessageBox.Show("Please select a directory that contains some pictures!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var ssw = new SlideshowWindow(dataContext.Right, new OpacityEffect(), this);
            ssw.Show();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try 
            { 
                DirectoryItem SelectedDirectoryItem = (DirectoryItem)treeView.SelectedItem;
                if (SelectedDirectoryItem != null)
                {
                    var items = ItemProvider.GetFileItems(SelectedDirectoryItem.Path);
                    dataContext.Right = items;
                    DataContext = new MyDataContext()
                    {
                        Left = dataContext.Left,
                        Right = items,
                    };
                    //DataContext = dataContext;
                }
                
            }
            catch (InvalidCastException) { return; }
            catch (NullReferenceException) { return; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dataContext.Right == null || dataContext.Right.Count == 0)
            {
                MessageBox.Show("Please select a directory that contains some pictures!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (slideshowComboBox.SelectedIndex == 1)
            {
                var ssw = new SlideshowWindow(dataContext.Right, new HorizontalEffect(), this);
                ssw.Show();
            }
            else if (slideshowComboBox.SelectedIndex == 0)
            {
                var ssw = new SlideshowWindow(dataContext.Right, new VerticalEffect(), this);
                ssw.Show();
            }
            else if (slideshowComboBox.SelectedIndex == 2)
            {
                var ssw = new SlideshowWindow(dataContext.Right, new OpacityEffect(), this);
                ssw.Show();
            }
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public long Size { get; set; }
    }

    public class FileItem : Item
    {

    }

    public class DirectoryItem : Item
    {
        public List<Item> Items { get; set; }
        public DirectoryItem()
        {
            Items = new List<Item>();
        }
        public DirectoryItem(List<Item> list)
        {
            Items = list;
        }
        public DirectoryItem(string name, string path)
        {
            Name = name;
            Path = path;
            Items = ItemProvider.GetItems(path);
        }
    }

    public static class ItemProvider
    {
        public static List<Item> GetItems(string path)
        {
            var items = new List<Item>();
            var dirInfo = new DirectoryInfo(path);
            try
            {
                foreach (var directory in dirInfo.GetDirectories())
                {
                    var item = new DirectoryItem
                    {
                        Name = directory.Name,
                        Path = directory.FullName,
                        Items = GetItems(directory.FullName)
                    };

                    items.Add(item);
                }
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (System.IO.IOException)
            {
                throw;
            }
            foreach (var file in dirInfo.GetFiles())
            {
                if (!file.Name.EndsWith(".png") && !file.Name.EndsWith(".jpg") && !file.Name.EndsWith(".bmp")) continue;
                var f = System.Drawing.Image.FromFile(file.FullName);
                var item = new FileItem
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Width = f.Width,
                    Height = f.Height,
                    Size = file.Length / 1024,
                };
                items.Add(item);
            }
            return items;
        }

        public static List<Item> GetFileItems(string path)
        {
            var items = new List<Item>();
            var dirInfo = new DirectoryInfo(path);
            foreach (var file in dirInfo.GetFiles())
            {
                if (!file.Name.EndsWith(".png") && !file.Name.EndsWith(".jpg") && !file.Name.EndsWith(".bmp")) continue;
                var f = System.Drawing.Image.FromFile(file.FullName);
                var item = new FileItem
                {
                    Name = file.Name,
                    Path = file.FullName,
                    Width = f.Width,
                    Height = f.Height,
                    Size = file.Length / 1024,
                };
                items.Add(item);
            }
            return items;
        }
    }

    public class MyDataContext
    {
        public List<Item> Left { get; set; }
        public List<Item> Right { get; set; }
    }
}
