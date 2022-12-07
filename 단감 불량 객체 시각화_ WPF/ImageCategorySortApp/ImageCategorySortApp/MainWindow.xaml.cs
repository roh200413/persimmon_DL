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
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Xml;
using System.Collections.ObjectModel;

namespace ImageCategorySortApp
{
    public partial class MainWindow : Window
    {
        string XMLFolderPath { get; set; }
        string ImageFolderPath { get; set; }
        List<string> Objects { get; set; }
        ObservableCollection<PersimmonImage> PersimmonImages { get; set; } = new ObservableCollection<PersimmonImage> { };
        public MainWindow()
        {
            InitializeComponent();
            Objects = new List<string>() { };
        }

        private void ImageButtonClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFileDlg = new CommonOpenFileDialog();
            openFileDlg.IsFolderPicker = true;
            if (openFileDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                ImageFolderPath = openFileDlg.FileName;
                DirectoryInfo di = new DirectoryInfo(ImageFolderPath);
                if (di.Exists == false)
                    return;
                //['Anthrax','LiftedStem','StinkBug','Wounds','BlackSpot1','BlackSpot2','BlackSpot3','ScaleInsects']
                foreach (System.IO.FileInfo File in di.GetFiles("*.jpg"))
                {
                    var bitmapImage = new BitmapImage(new Uri(File.FullName));
                    var name = File.Name.Split('.');
                    PersimmonImage persimmonImage = PersimmonImages.Where(w => w.Name == $"{name[0]}.{name[1]}").SingleOrDefault();

                    DrawingGroup imageDrawings = new DrawingGroup();
                    ImageDrawing wholeKiwi = new ImageDrawing();
                    wholeKiwi.Rect = new Rect(0, 0, 512, 512);
                    wholeKiwi.ImageSource = new BitmapImage(new Uri(File.FullName));
                    imageDrawings.Children.Add(wholeKiwi);

                    foreach (var PersimmonXML in persimmonImage.PersimmonXML)
                    {
                        GeometryDrawing ellipseDrawing =
                            new GeometryDrawing(
                                new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                                new Pen(Brushes.Black, 3),
                                new RectangleGeometry(new Rect(PersimmonXML.XMin, PersimmonXML.YMin, PersimmonXML.XMax - PersimmonXML.XMin, PersimmonXML.YMax - PersimmonXML.YMin))
                            );
                        imageDrawings.Children.Add(ellipseDrawing);
                    }

                    DrawingImage drawingImageSource = new DrawingImage(imageDrawings);
                    var image = new Image() { Source = drawingImageSource, Width = 200, Height = 200, Stretch = Stretch.Fill, Visibility = Visibility.Visible, Margin = new Thickness(5) }; // 
                    AnthraxStack.Children.Add(image);

                }
            }
        }

        private void XMLLoadButtonClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog openFileDlg = new CommonOpenFileDialog();
            openFileDlg.IsFolderPicker = true;
            if (openFileDlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                XMLFolderPath = openFileDlg.FileName;
                DirectoryInfo di = new DirectoryInfo(XMLFolderPath);
                if (di.Exists == false)
                    return;
                PersimmonImages.Clear();

                foreach (System.IO.FileInfo File in di.GetFiles("*.xml"))
                {
                    PersimmonImage persimmonImage = new PersimmonImage();
                    persimmonImage.FullName = File.FullName;
                    persimmonImage.Name = $"{File.Name.Split('.')[0]}.{File.Name.Split('.')[1]}";

                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(File.FullName);
                    XmlNodeList nodes = xdoc.SelectNodes("/annotation");
                    foreach (XmlNode nodeItem in nodes)
                    {
                        foreach (XmlNode xmlNode in nodeItem.ChildNodes)
                        {
                            if (xmlNode.LocalName == "object")
                            {
                                string name = xmlNode.SelectSingleNode("name").InnerText;
                                XmlNode bndbox = xmlNode.SelectSingleNode("bndbox");
                                double xmin = Double.Parse(bndbox.SelectSingleNode("xmin").InnerText);
                                double ymin = Double.Parse(bndbox.SelectSingleNode("ymin").InnerText);
                                double xmax = Double.Parse(bndbox.SelectSingleNode("xmax").InnerText);
                                double ymax = Double.Parse(bndbox.SelectSingleNode("ymax").InnerText);
                                switch (name)
                                {
                                    case "ScaleInsects":
                                        break;
                                }
                                persimmonImage.PersimmonXML.Add(new PersimmonXML() { Name = name, XMin = xmin, XMax = xmax, YMin = ymin, YMax = ymax});
                                if (!Objects.Contains(name))
                                    Objects.Add(name);
                            }

                        }
                    }
                    PersimmonImages.Add(persimmonImage);
                }
            }

            foreach (var item in Objects)
            {
                ComboBoxItem ComboBoxItem = new ComboBoxItem();
                ComboBoxItem.Content = item;
                PersimmonComboBox.Items.Add(ComboBoxItem);
            }
        }

        private void PersimmonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(ImageFolderPath);


            SelectStack.Children.Clear();
            foreach (System.IO.FileInfo File in di.GetFiles("*.jpg"))
            {
                var bitmapImage = new BitmapImage(new Uri(File.FullName));
                var name = File.Name.Split('.');
                PersimmonImage persimmonImage = PersimmonImages.Where(w => w.Name == $"{name[0]}.{name[1]}").SingleOrDefault();

                ComboBoxItem comboBoxItem = PersimmonComboBox.SelectedValue as ComboBoxItem;
                string selectedItem = comboBoxItem.Content as string;
                if (!persimmonImage.PersimmonXML.Select(w => w.Name == selectedItem).FirstOrDefault()) continue;

                DrawingGroup imageDrawings = new DrawingGroup();
                ImageDrawing wholeKiwi = new ImageDrawing();
                wholeKiwi.Rect = new Rect(0, 0, 512, 512);
                wholeKiwi.ImageSource = new BitmapImage(new Uri(File.FullName));
                imageDrawings.Children.Add(wholeKiwi);

                foreach (var PersimmonXML in persimmonImage.PersimmonXML)
                {
                    Pen pen = new Pen(Brushes.Black, 3);
                    switch (PersimmonXML.Name)
                    {
                        case ("Anthrax"):
                            pen = new Pen(Brushes.Red, 3);
                            break;
                        case ("LiftedStem"):
                            pen = new Pen(Brushes.Orange, 3);
                            break;
                        case ("StinkBug"):
                            pen = new Pen(Brushes.Yellow, 3);
                            break;
                        case ("Wounds"):
                            pen = new Pen(Brushes.Green, 3);
                            break;
                        case ("BlackSpot1"):
                            pen = new Pen(Brushes.Blue, 3);
                            break;
                        case ("BlackSpot2"):
                            pen = new Pen(Brushes.Navy, 3);
                            break;
                        case ("BlackSpot3"):
                            pen = new Pen(Brushes.Purple, 3);
                            break;
                        case ("ScaleInsects"):
                            pen = new Pen(Brushes.Pink, 3);
                            break;
                    }


                    GeometryDrawing ellipseDrawing =
                        new GeometryDrawing(
                            new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                            pen,
                            new RectangleGeometry(new Rect(PersimmonXML.XMin, PersimmonXML.YMin, PersimmonXML.XMax - PersimmonXML.XMin, PersimmonXML.YMax - PersimmonXML.YMin))
                        );
                    imageDrawings.Children.Add(ellipseDrawing);
                }

                DrawingImage drawingImageSource = new DrawingImage(imageDrawings);
                var image = new Image() { Source = drawingImageSource, Width = 200, Height = 200, Stretch = Stretch.Fill, Visibility = Visibility.Visible, Margin = new Thickness(5) }; // 
                SelectStack.Children.Add(image);
            }
        }
    }
    public class PersimmonImage
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public ObservableCollection<PersimmonXML> PersimmonXML { get; set; } = new ObservableCollection<PersimmonXML>();
    }
    public class PersimmonXML
    {
        public string Name { get; set; }
        public double XMin { get; set; }
        public double YMin { get; set; }
        public double XMax { get; set; }
        public double YMax { get; set; }
    }

    //'Anthrax', 2: 'LiftedStem', 3: 'StinkBug', 4: 'Wounds', 5: 'BlackSpot1', 6: 'BlackSpot2', 7: 'BlackSpot3', 8:'ScaleInsects'
    public enum PersimmonGradeEnum
    {
        Anthrax,
        LiftedStem,
        StinkBug,
        Wounds,
        BlackSpot1,
        BlackSpot2,
        BlackSpot3,
        ScaleInsects,
    }
}
