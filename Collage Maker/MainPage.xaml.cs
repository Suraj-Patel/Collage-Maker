using System;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Collage_Maker.Resources;
using Microsoft.Phone.Tasks;
using System.IO;

namespace Collage_Maker
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        int size = 1;
        int s=0;
        string Filename = string.Format("CollageImage:{0:G}", DateTime.Now);
        private WriteableBitmap backgroundBitmap = null;
        private PhotoChooserTask choose_Image;
        private WriteableBitmap SelectedBitmap = null;
        Image img = new Image();
        Point pt;
        bool imgSelected = false;
      //  bool imgDelete = false;
        public MainPage()
        {
            InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            choose_Image = new PhotoChooserTask();
            choose_Image.Completed +=
              new EventHandler<PhotoResult>(chooseImage_Completed);
        }
        private void setbackground(string s)
        {
            string name = "backgrounds/" + s;
            Uri uri = new Uri(name, UriKind.RelativeOrAbsolute);
            ImageSource imgsource = new BitmapImage(uri);
            background.Source = imgsource;
            pivotControl.SelectedIndex = 1 ;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;
        }
        private void bg1_Click(object sender, RoutedEventArgs e)
        {
            setbackground("1.jpg");
        }

        private void bg2_Click(object sender, RoutedEventArgs e)
        {
            setbackground("2.jpg");
        }

        private void bg3_Click(object sender, RoutedEventArgs e)
        {
            setbackground("3.jpg");
        }

        private void bg4_Click(object sender, RoutedEventArgs e)
        {
            setbackground("4.jpg");
        }

        private void bg5_Click(object sender, RoutedEventArgs e)
        {
            setbackground("5.jpg");
        }

        private void bg6_Click(object sender, RoutedEventArgs e)
        {
            setbackground("6.jpg");
        }

        private void bg7_Click(object sender, RoutedEventArgs e)
        {
            setbackground("7.jpg");
        }

        private void Gallery_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask Chooser = new PhotoChooserTask();
            Chooser.Completed += Chooser_Completed;
            Chooser.Show();
        }

        void Chooser_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK || e.ChosenPhoto == null)
            {
                return;
            }

            try 
            {
                backgroundBitmap = new WriteableBitmap((int)background.Width, (int)background.Height);
                backgroundBitmap.SetSource(e.ChosenPhoto);
                background.Source = backgroundBitmap;
                e.ChosenPhoto.Position = 0;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IsEnabled = true;

                pivotControl.SelectedIndex = 1;    
            }

            catch(Exception exc) 
            {
                MessageBox.Show(exc.Message);
                return;
            }
        }

        private void AddImage_Click(object sender, EventArgs e)
        {
            choose_Image.Show();
            img = new Image();
        }

        int i = 0;
        int j = 0;
        public void chooseImage_Completed(object sender, PhotoResult e)
        {
            j = 0;
            if (e.TaskResult != TaskResult.OK || e.ChosenPhoto == null)
            {
                return;
            }
            SelectedBitmap = new WriteableBitmap(300,300);
            if(size==1)
            {
                img.Width = 60;
                img.Height = 60;
            }
            if (size == 2)
            {
                img.Width = 100;
                img.Height = 100;
            } 
            if (size == 3)
            {
                img.Width = 200;
                img.Height = 200;
            }
            if (size == 4)
            {
                img.Width = 300;
                img.Height = 300;
            }
            e.ChosenPhoto.Position = 0;
            SelectedBitmap.SetSource(e.ChosenPhoto);
            img.Source = SelectedBitmap;
            img.Name = "photo" + i++;
            imgSelected = true;
            s++;
            ((ApplicationBarIconButton)ApplicationBar.Buttons[2]).IsEnabled = true;
        }
        private void CollageCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (imgSelected)
            {
                //img = new Image();    
                pt = e.GetPosition(CollageCanvas);
                img.Source = SelectedBitmap;
                img.Name = "photo" + i++;
                if (size == 1)
                {
                    img.Width = 100;
                    img.Height = 100;
                }
                if (size == 2)
                {
                    img.Width = 150;
                    img.Height = 150;
                } if (size == 3)
                {
                    img.Width = 200;
                    img.Height = 200;
                }
                if (size == 4)
                {
                    img.Width = 300;
                    img.Height = 300;
                }
                CollageCanvas.Children.Remove(img);
                CollageCanvas.Children.Add(img);
                Canvas.SetLeft(img, pt.X);
                Canvas.SetTop(img, pt.Y);
                j++;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            WriteableBitmap bitmap = new WriteableBitmap((int)background.Width, (int)background.Height);
            bitmap.Render(CollageCanvas, null);
            bitmap.Invalidate();
            SaveToMediaLibrary(bitmap, Filename, 100);
        }
        public void SaveToMediaLibrary(WriteableBitmap wb, string name, int quality)
        {
            using (var stream = new MemoryStream())
            {
                wb.SaveJpeg(stream, wb.PixelWidth, wb.PixelHeight, 0, quality);
                stream.Seek(0, SeekOrigin.Begin);
                new MediaLibrary().SavePicture(name, stream);
                MessageBox.Show("Image Saved");
            }
        }
        private void small_Click(object sender, EventArgs e)
        {
            size = 1;
        }

        private void medium_Click(object sender, EventArgs e)
        {
            size = 2;
        }

        private void large_Click(object sender, EventArgs e)
        {
            size = 3;
        }

        private void VeryLarge_Click(object sender, EventArgs e)
        {
            size = 4;
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (s > 0)
            {
                CollageCanvas.Children.RemoveAt(s);
                s--;
            }
        }
    }
}