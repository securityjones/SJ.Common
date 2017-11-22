using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SJ.Common; // for cWebpage class

using System.Xml.Linq; // for XElement
using System.Diagnostics; // for Stopwatch

namespace TestUI
{
    // class to test methods and capabilities of cWebpage.cs
    //
    public class cWebpage_tests : TestBase
    {
        // required method to run tests
        //
        // Note: for now, manually clear the Webcache folders before running
        //
        public override String RunTests()
        {
            StringBuilder sb = new StringBuilder();
            String classname = this.GetType().Name;

            sb.AppendLine(String.Format("** begin {0} **", classname));

            // sb.AppendLine(tbd());

            // https://access.redhat.com/security/security-updates/#/security-advisories
            //
            String url1 = @"https://access.redhat.com/security/security-updates/#/security-advisories";
            String url2 = @"https://securityjones.com";
            String url3 = @"http://securityjones.com";
            String url4 = @"https://securityjones.com/doesnotexist.htm";

            // test url1
            sb.Append(testAgainstUrl(url1));
            sb.Append(testAgainstUrl(url2));
            sb.Append(testAgainstUrl(url3));
            sb.Append(testAgainstUrl(url4));
            
            sb.AppendLine(String.Format("** end {0} **", classname));

            return sb.ToString();
        }

        private String testAgainstUrl(String url)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(String.Format("* begin testing url: {0}", url));

            cWebpage wp = new cWebpage(url, true); // get content from the web

            sb.AppendLine(String.Format("Host {0}, Filename {1}", wp.Host, wp.Filename));
            sb.Append("To XElement() test: ");
            try {
                XElement xe = wp.ToXElement();
                if (xe.IsEmpty) sb.AppendLine(" fail EMPTY");
                else {
                    String s50 = xe.ToString().ReduceWhiteSpace().Substring(0, 50);
                    sb.AppendLine(String.Format(" pass | {0}", s50));
                }
            } catch { sb.AppendLine(" fail conversion"); }

            sb.Append("ToString() test: ");
            try
            {
                String hstr = wp.ToString().ReduceWhiteSpace();
                if (String.IsNullOrEmpty(hstr)) sb.AppendLine(" fail NullOrEmpty");
                else sb.AppendLine(String.Format(" pass | {0}", hstr.Substring(0,50)));
            }
            catch { sb.AppendLine(" fail conversion"); }

            Stopwatch stopw = new Stopwatch();

            sb.Append("* timing test from web :");
            stopw.Start();
            wp = new cWebpage(url, true);
            bool hc = wp.HasContent;
            stopw.Stop();
            sb.AppendLine(stopw.Elapsed.ToString());

            sb.Append("* timing test from cache :");
            stopw.Restart();
            wp = new cWebpage(url, false);
            hc = wp.HasContent;
            stopw.Stop();
            sb.AppendLine(stopw.Elapsed.ToString());


            sb.AppendLine(String.Format("* end testing url: {0}", url));

            return sb.ToString();
        }

    }

}
