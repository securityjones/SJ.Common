using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SJ.Common; // for Webpage class

using System.Xml.Linq; // for XElement

namespace TestUI
{
    // class to test methods and capabilities of Webpage.cs
    //
    public class Webpage_tests : TestBase
    {
        // required method to run tests
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
                        
            Webpage wp = new Webpage(url1);
            wp.UseCache = false;
            Console.WriteLine(String.Format("Host: {0}", wp.Host));
            Console.WriteLine(String.Format("Filename: {0}", wp.Filename));
            XElement xe = wp.ToXElement();
            String htmlstr = wp.ToString();

            wp = new Webpage(url2);
            wp.UseCache = false;
            Console.WriteLine(String.Format("Host: {0}", wp.Host));
            Console.WriteLine(String.Format("Filename: {0}", wp.Filename));
            xe = wp.ToXElement();
            htmlstr = wp.ToString();

            sb.AppendLine(String.Format("** end {0} **", classname));

            return sb.ToString();
        }

    }

}
