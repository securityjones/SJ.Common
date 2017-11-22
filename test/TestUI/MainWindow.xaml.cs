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

using SJ.Common;

namespace TestUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private String RunAllTests()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("*** begin tests ***");

            Helper_tests t1 = new Helper_tests();
            sb.AppendLine(t1.RunTests());

            cWebpage_tests t2 = new cWebpage_tests();
            sb.AppendLine(t2.RunTests());

            Extensions_tests t3 = new Extensions_tests();
            sb.AppendLine(t3.RunTests());

            sb.AppendLine("*** end tests ***");

            return sb.ToString();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            /*
            cWebpage wp = new cWebpage();

            wp.Url = "http://svpetfoodbank.org";
            
            tbMain.Text = wp.LoadHtmlDoc();
            */

            tbMain.Text = RunAllTests();
        }
    }
}
