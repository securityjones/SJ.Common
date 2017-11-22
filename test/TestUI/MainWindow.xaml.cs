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

            Webpage_tests t2 = new Webpage_tests();
            sb.AppendLine(t2.RunTests());

            sb.AppendLine("*** end tests ***");

            return sb.ToString();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            /*
            Webpage wp = new Webpage();

            wp.Url = "http://svpetfoodbank.org";
            
            tbMain.Text = wp.LoadHtmlDoc();
            */

            tbMain.Text = RunAllTests();
        }
    }
}
