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
using PCB.Models;

namespace PCB.View
{
    /// <summary>
    /// Main.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Main : Page
    {
        Models.CSharpToPython cSharpToPython = new Models.CSharpToPython();
        public Main()
        {
            InitializeComponent();

        }

        private void NV_Video1_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("/VIEW/Video1.xaml", UriKind.Relative);
            cSharpToPython.OrderEqtFirstPos();
            NavigationService.Navigate(uri);
        }
    }
}
