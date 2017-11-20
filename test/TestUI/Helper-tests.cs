using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SJ.Common; // classes being test
using System.IO; // for Directory and File

namespace TestUI
{
    class Helper_tests : TestBase
    {
        public override String RunTests()
        {
            StringBuilder sb = new StringBuilder();
            String classname = this.GetType().Name;

            sb.AppendLine(String.Format("** begin {0} **", classname));

            sb.AppendLine(ValidateFolder_tests());

            sb.AppendLine(String.Format("** end {0} **", classname));

            return sb.ToString();
        }

        // Tests for ValidateFolder
        //
        private String ValidateFolder_tests()
        {
            StringBuilder sb = new StringBuilder();
            String p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // eg. c:\Users\jrjones

            sb.AppendLine("* begin ValidateFolder tests *");

            SJHelper h = new SJHelper();

            sb.Append("- test : null pathname test : ");
            try {
                h.ValidateFolder(null);
                sb.AppendLine("fail");
            }
            catch { sb.AppendLine("pass"); }
            
            sb.Append("- test : Empty pathname test : ");
            try
            {
                h.ValidateFolder(String.Empty);
                sb.AppendLine("fail");
            }
            catch { sb.AppendLine("pass"); }

            sb.Append("- test : existing file conflict test : ");
            String p1 = p;
            p1 += @"\NTUSER.DAT";
            try
            {
                h.ValidateFolder(p1);
                sb.AppendLine("fail");
            }
            catch { sb.AppendLine("pass"); }

            sb.Append("- test : multiple sub-folders to create test : ");
            p1 = p + @"\testa";
            String p2 = p + @"\testa\testb";
            try
            {
                h.ValidateFolder(p2);
                if (Directory.Exists(p2))
                {
                    sb.AppendLine("pass");
                    Directory.Delete(p2); // clean up after
                    Directory.Delete(p1); // clean up after
                }
                else sb.AppendLine("fail");
            }
            catch { sb.AppendLine("fail"); }

            sb.AppendLine("* end ValidateFolder tests *");

            return sb.ToString();
        }

    }

    class Webpage_tests : TestBase { }
}
