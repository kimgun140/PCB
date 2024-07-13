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
    /// Inspection1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Inspection1 : Page
    {
        Models.Server server = new Models.Server();
        Models.PCBinfo pcbinfo = new Models.PCBinfo();
        Models.ImgFuncs imgFuncs = new Models.ImgFuncs();
        Models.CSharpToPython cSharpToPython = new Models.CSharpToPython();

        //VIEW.Video1 Video1 = new VIEW.Video1();
        //static public Models.PythonServer python = new Models.PythonServer();
        //public Inspection1()
        //{
        //    InitializeComponent();
        //    //pcbinfo.MCU = 0;
        //    //pcbinfo.LTC = 1;
        //    //pcbinfo.ADC = 0;
        //    //pcbinfo.DAC = 0;
        //    //pcbinfo.XTR = 1;
        //    //pcbinfo.LED1 = 1;
        //    //pcbinfo.LED2 = 0;

        //    // 이미지 전처리 후 데이터 뽑아낸 함수를 여기서 호출해야 함!


        //    // 데이터 확인용 임시

        //    // 이미지 띄우기

        //    Mat mat = Cv2.ImRead(@"C:\Users\lms\Desktop\PCB1\PCB\Resources\save_img.jpg");
        //    image1.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(mat);
        //    datacheck();

        //    string ok = "정상";
        //    string fail = "불량";

        //    if (VIEW.Video1.Status != 0)
        //    {
        //        MessageBox.Show(Video1.Status.ToString());
        //        First_check.Text = ok;
        //        MessageBox.Show(First_check.Text);
        //    }
        //    else
        //    {
        //        MessageBox.Show(Video1.Status.ToString());

        //        First_check.Text = fail;
        //        MessageBox.Show(First_check.Text);
        //    }
        //}
        public Inspection1(int status)
        {
            

            InitializeComponent();
            //pcbinfo.MCU = 0;
            //pcbinfo.LTC = 1;
            //pcbinfo.ADC = 0;
            //pcbinfo.DAC = 0;
            //pcbinfo.XTR = 1;
            //pcbinfo.LED1 = 1;
            //pcbinfo.LED2 = 0;

            // 이미지 전처리 후 데이터 뽑아낸 함수를 여기서 호출해야 함!


            // 데이터 확인용 임시

            // 이미지 띄우기

            Mat mat = Cv2.ImRead(@"C:\Users\lms\Desktop\PCB1\PCB\Resources\save_img.jpg");
            image1.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(mat);
            //datacheck();

            string ok = "정상";
            string fail = "불량";

            if (status != 0)
            {
                First_check.Text = ok;
            }
            else
            {

                First_check.Text = fail;
            }

            if (status != 0)
            {
                Ok_btn.IsEnabled = true;
                Faulty_btn.IsEnabled = false;
            }
            else
            {
                Ok_btn.IsEnabled = false;
                Faulty_btn.IsEnabled = true;
            }

        }


        //public void datacheck()
        //{

        //    if (pcbinfo.Status != 0)
        //    {
        //        Ok_btn.IsEnabled = true;
        //        Faulty_btn.IsEnabled = false;
        //    }
        //    else
        //    {
        //        Ok_btn.IsEnabled = false;
        //        Faulty_btn.IsEnabled = true;
        //    }
        //}
        private void Ok_btn_Click(object sender, RoutedEventArgs e)
        {
            //최종 위치로 이동
            cSharpToPython.OrderEqtInitPos();
            cSharpToPython.OrderEqtPass();
            server.send_INSPECTION1(pcbinfo);

            Uri uri = new Uri("/View/FinalCheck.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void Faulty_btn_Click(object sender, RoutedEventArgs e)
        {
            //2차 검수 위치로 이동
            //2차검수 영상 위치로 이동
            cSharpToPython.OrderEqtInitPos();
            cSharpToPython.OrderEqtFirstErr();
            cSharpToPython.OrderEqtSecondPos();

            server.send_INSPECTION1(pcbinfo);

            Uri uri = new Uri("/View/Inspection2.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}
