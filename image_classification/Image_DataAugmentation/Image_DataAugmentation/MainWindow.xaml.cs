using OpenCvSharp;
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

namespace Image_DataAugmentation
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Mat src = Cv2.ImRead(@"C:\Users\sdf80\Desktop\단감사진\홍시\h2.jpg", ImreadModes.Grayscale); Mat dst = new Mat();

            Mat matrix = Cv2.GetRotationMatrix2D(new Point2f(src.Width / 2, src.Height / 2), 45.0, 1.0);
            Cv2.WarpAffine(src, dst, matrix, new OpenCvSharp.Size(src.Width, src.Height));

            Cv2.ImShow("dst", dst);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            dst.SaveImage(@"C:\Users\sdf80\Desktop\단감사진\홍시\h2_1.jpg");


        }
    }
}
