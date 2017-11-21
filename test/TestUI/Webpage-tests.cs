using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SJ.Common; // for Webpage class

namespace TestUI
{
    // class to test methods and capabilities of Webpage.cs
    //
    class Webpage_tests : TestBase
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
            Webpage wp = new Webpage();
            wp.Url = @"https://access.redhat.com/security/security-updates/#/security-advisories";
            wp.Fetch();
            Console.WriteLine(String.Format("Domain: {0}", wp.Domain));

            sb.AppendLine(String.Format("** end {0} **", classname));

            return sb.ToString();
        }

    }

}
