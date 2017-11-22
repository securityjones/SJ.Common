using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net; // for WebClient

namespace SJ.Common
{
    public static class MyExtensions
    {

        // String extensions

        // ReduceWhiteSpace - converts multiple whitespace to a single space characters and trims beginning and end
        //
        public static String ReduceWhiteSpace(this String str)
        {
            /* conceptual, slow-performance implementation
             * 
            String result = str.Replace("\t", " ").Replace("\n", " ");
            while (result.Contains("  ")) result = result.Replace("  ", " ");
            result = result.Trim();
            */

            // now a better performance one

            char[] input = str.ToCharArray();

            var sb = new StringBuilder();

            int iBegin = 0;
            int iEnd = str.Length - 1;

            // trims all whitespace off the beginning and end
            while (char.IsWhiteSpace(input[iBegin])) iBegin++;
            while (char.IsWhiteSpace(input[iEnd])) iEnd--;

            var prevIsWhitespace = false;

            for (int i = iBegin; i <= iEnd; i++)
            {
                char c = input[i];

                var isWhitespace = char.IsWhiteSpace(c);

                if (isWhitespace) c = ' '; // change any whitespace to a space

                if (prevIsWhitespace && isWhitespace)
                {
                    continue;
                }
                sb.Append(c);
                prevIsWhitespace = isWhitespace;
            }

            return sb.ToString();

        }

        // WebClient extension
        public static WebClient FavDefaults(this WebClient wc)
        {
            String ie11str = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko"; // Header used by IE11
            wc.Headers.Add("user-agent", ie11str);
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12; // necessary for nvd.nist.gov which only supports tls 1.2

            return wc;
        }
    }
}
