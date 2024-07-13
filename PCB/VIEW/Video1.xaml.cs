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
using OpenCvSharp;
using PCB.Models;

namespace PCB.VIEW
{
    /// <summary>
    /// Video1.xaml에 대한 상호 작용 논리
    /// </summary>
    
    public partial class Video1 : Page
    {
        Models.ImgFuncs ImgFuncs = new Models.ImgFuncs();
        Models.CSharpToPython cSharpToPython = new Models.CSharpToPython();
        Models.Server Server = new Models.Server();
        PCBinfo pcbinfo = new PCBinfo();
        public int Status;
        bool pagestatus = true;
        public Video1()
        {
            this.InitializeComponent();
            InitializeComponent();

            //첫번째 사진찍는 위치로 이동

            VideoCapture cam = new VideoCapture(0);
            Task.Run(async () =>
            {
                while (pagestatus)
                {
                    Mat frame = ImgFuncs.MakeFrame(cam);
                    Mat src = ImgFuncs.PreProcessing();
                    ImgFuncs.OnlyMakeContours(src);
                    //Cv2.ImShow("frame", ImgFuncs.frame);
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.video1.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(ImgFuncs.frame);
                    }));
                } 
            });
        }

        private void FaultyCheck_btn_Click(object sender, RoutedEventArgs e)
        {
            ImgFuncs.save_img();
            Mat src = ImgFuncs.PreProcessing();
            if (ImgFuncs.MakeContours(src) == true)
            {
                Status = 1; //정상
            }
            else
            {
                Status = 0; // 불량
            }
            pagestatus = false;

            Inspection1 inspection1 = new Inspection1(Status);
            NavigationService.Navigate(inspection1);

        }
    }
}
