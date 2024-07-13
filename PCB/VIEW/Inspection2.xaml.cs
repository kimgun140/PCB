using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace PCB.VIEW
{
    /// <summary>
    /// Inspection2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Inspection2 : Page
    {
        ObservableCollection<PCBinfo2> items = new ObservableCollection<PCBinfo2>
        {
            new PCBinfo2 { Name = "MCU", CheckBoxStatus = false, num = "1"},
            new PCBinfo2 { Name = "LTC", CheckBoxStatus = false,  num = "2"},
            new PCBinfo2 { Name = "ADC", CheckBoxStatus = false , num = "3"},
            new PCBinfo2 { Name = "DAC", CheckBoxStatus = false , num = "4"},
            new PCBinfo2 { Name = "XTR", CheckBoxStatus = false , num = "5"},
            new PCBinfo2 { Name = "LED1", CheckBoxStatus = false , num = "6"},
            new PCBinfo2 { Name = "LED2", CheckBoxStatus = false , num = "7"},
        };
        Models.Server server = new Models.Server();
        Models.PCBinfo pcbinfo = new Models.PCBinfo();
        Models.ImgFuncs imgFuncs = new Models.ImgFuncs();
        Models.CSharpToPython cSharpToPython = new Models.CSharpToPython();
        bool pagestatus = true;
        public Inspection2()
        {

            InitializeComponent();
            VideoCapture cam = new VideoCapture(0);

            Task.Run(async () =>
            {
                while (pagestatus)
                {
                    Mat frame = imgFuncs.MakeFrame(cam);
                    Mat src = imgFuncs.PreProcessing();
                    imgFuncs.OnlyMakeContours(src);
                    //Cv2.ImShow("frame", ImgFuncs.frame);
                    await Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.video2.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(imgFuncs.frame);
                    }));
                } // while
            });

            this.Inspec2_listview.ItemsSource = items;


        }

        private void OperCheck_btn_Click(object sender, RoutedEventArgs e)
        {
            //최종 위치로 이동
            server.send_INSPECTION2(pcbinfo);
            if (pcbinfo.MCU == 1 && pcbinfo.LTC == 1 && pcbinfo.ADC == 1 && pcbinfo.DAC == 1 && pcbinfo.XTR == 1 && pcbinfo.LED1 == 1 && pcbinfo.LED2 == 1)
            {
                cSharpToPython.OrderEqtInitPos();
                cSharpToPython.OrderEqtFirstErrPass();
            }
            else
            {
                cSharpToPython.OrderEqtInitPos();
                cSharpToPython.OrderEqtFirstErrErr();
            }

            pagestatus = false;
            Uri uri = new Uri("/View/FinalCheck.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        private void datacheck_btn_Click(object sender, RoutedEventArgs e)
        {

            pcbinfo.MCU = items[0].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.LTC = items[1].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.ADC = items[2].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.DAC = items[3].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.XTR = items[4].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.LED1 = items[5].CheckBoxStatus == true ? 1 : 0;
            pcbinfo.LED2 = items[6].CheckBoxStatus == true ? 1 : 0;

            MessageBox.Show($"{pcbinfo.MCU}{pcbinfo.LTC}{pcbinfo.ADC}{pcbinfo.DAC}{pcbinfo.XTR}{pcbinfo.LED1}{pcbinfo.LED2}");
        }
    }
}
