using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SJ.Common;
using System.Net;

namespace TestUI
{
    public class Extensions_tests : TestBase
    {
        public override String RunTests()
        {
            StringBuilder sb = new StringBuilder();
            String classname = this.GetType().Name;

            sb.AppendLine(String.Format("** begin {0} **", classname));

            sb.AppendLine(testReduceWhiteSpace());
            sb.AppendLine(testFavDefaults());

            sb.AppendLine(String.Format("** end {0} **", classname));

            return sb.ToString();
        }

        private String testReduceWhiteSpace()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*** BEGIN test ReduceWhiteSpace ***");

            // test for String extension ReduceWhiteSpace()

            String testStr = "   space filled \ntext \t\n string  ";

            sb.AppendLine("input: >" + testStr + "<");
            sb.AppendLine("output: >" + testStr.ReduceWhiteSpace() + "<");

            testStr = "         wow,        the one         has a         lot   ";

            sb.AppendLine("input: >" + testStr + "<");
            sb.AppendLine("output: >" + testStr.ReduceWhiteSpace() + "<");

            testStr = "wow, the one  has a  lot too";

            sb.AppendLine("input: >" + testStr + "<");
            sb.AppendLine("output: >" + testStr.ReduceWhiteSpace() + "<");

            sb.AppendLine("*** END test ReduceWhiteSpace ***");

            return sb.ToString();
        }

        private String testFavDefaults()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("*** BEGIN test testDownload ***");

            using (WebClient wc = new WebClient())
            {
                wc.FavDefaults();

                String url1 = "http://svpetfoodbank.org";
                String c = wc.DownloadString(url1);
                sb.AppendLine("downloaded " + c.Length + " chars");

                url1 = "https://securityjones.com";
                c = wc.DownloadString(url1);
                sb.AppendLine("downloaded " + c.Length + " chars");

                url1 = "https://static.nvd.nist.gov/feeds/xml/cve/2.0/nvdcve-2.0-Modified.meta";
                c = wc.DownloadString(url1);
                sb.AppendLine("downloaded " + c.Length + " chars");
            }

            sb.AppendLine("*** END test testDownload ***");

            return sb.ToString();
        }

    }
}
