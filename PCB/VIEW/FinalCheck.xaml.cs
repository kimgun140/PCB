using OpenCvSharp;
using PCB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace PCB.VIEW
{
    /// <summary>
    /// FinalCheck.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FinalCheck : Page, INotifyPropertyChanged
    {
        ObservableCollection<recvInfo> recvinfos;
        recvInfo _SelectedInfo;
        CSharpToPython cSharpToPython = new CSharpToPython();
        recvInfo SelectedInfo
        {
            get { return _SelectedInfo; }
            set { _SelectedInfo = value; OnPropertyChanged("SelectedInfo"); }
        }
        public FinalCheck()
        {
            InitializeComponent();
            //Mat mat = Cv2.ImRead(@"C:\Users\Administrator\Desktop\PCB\PCB\Resources\save_img.jpg");
            //image2.Source = OpenCvSharp.WpfExtensions.WriteableBitmapConverter.ToWriteableBitmap(mat);

            recvinfos = new ObservableCollection<recvInfo>();
            SelectedInfo = new recvInfo();

            Models.Server serv = new Models.Server();
            serv.Connect();
            Task listen = Task.Run(async () =>
            {
                serv.recvMsgSync();
            });
            serv.send_REQ_INF();
            listen.Wait();
            string recvMsg = serv.recinfo;
            string[] sepMsgs = recvMsg.Split('\n');
            int cnt = 0;
            foreach (string str in sepMsgs)
            {
                string[] str2 = str.Split(',');
                if (str2[0] == "") break;
                cnt++;
                recvinfos.Add(new recvInfo()
                {
                    NO = cnt.ToString(),
                    MCU = (str2[0] == "1" ? "정상" : "불량"),
                    LTC = (str2[1] == "1" ? "정상" : "불량"),
                    ADC = (str2[2] == "1" ? "정상" : "불량"),
                    DAC = (str2[3] == "1" ? "정상" : "불량"),
                    XTR = (str2[4] == "1" ? "정상" : "불량"),
                    LED1 = (str2[5] == "1" ? "정상" : "불량"),
                    LED2 = (str2[6] == "1" ? "정상" : "불량")
                });
            }
            this.lv_infos.ItemsSource = recvinfos;


            this.Sp_NO.Text = SelectedInfo.NO;
            this.Sp_MCU.Text = SelectedInfo.MCU;
            this.Sp_LTC.Text = SelectedInfo.LTC;
            this.Sp_LED2.Text = SelectedInfo.LED2;
            this.Sp_XTR.Text = SelectedInfo.XTR;
            this.Sp_LED1.Text = SelectedInfo.LED1;
            this.Sp_ADC.Text = SelectedInfo.ADC;
            this.Sp_DAC.Text = SelectedInfo.DAC;
        }

        private void FinalOk_btn_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/View/Video1.xaml", UriKind.Relative);
            cSharpToPython.OrderEqtFirstPos();
            NavigationService.Navigate(uri);
        }

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void lv_infos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedInfo = (recvInfo)this.lv_infos.SelectedItem;
            this.Sp_NO.Text = SelectedInfo.NO;
            this.Sp_MCU.Text = SelectedInfo.MCU;
            this.Sp_LTC.Text = SelectedInfo.LTC;
            this.Sp_LED2.Text = SelectedInfo.LED2;
            this.Sp_XTR.Text = SelectedInfo.XTR;
            this.Sp_LED1.Text = SelectedInfo.LED1;
            this.Sp_ADC.Text = SelectedInfo.ADC;
            this.Sp_DAC.Text = SelectedInfo.DAC;
        }
    }

    public class recvInfo
    {
        public string NO { get; set; }
        public string MCU { get; set; }
        public string LTC { get; set; }
        public string ADC { get; set; }
        public string DAC { get; set; }
        public string XTR { get; set; }
        public string LED1 { get; set; }
        public string LED2 { get; set; }
        public recvInfo() { }
    }
    //public class ViewModelBase : INotifyPropertyChanged
    //{
    //    public void OnPropertyChanged(string propertyName)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = "")
    //    {
    //        if (EqualityComparer<T>.Default.Equals(member, value)) return false;
    //        member = value;
    //        OnPropertyChanged(propertyName);
    //        return true;
    //    }
    //}

}
