using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; // for Directory, File

namespace SJ.Common
{
    // helper class for SJ.Common
    //
    public class SJHelper
    {

        // This method validates a volume and folder, including naming, creates the folder if needed and returns 
        // the original (corrected, if necessary) folderpath if successful
        //  * throws an exception if it is an existing file
        //  * throws an exception if the folder does not exist and can not be created
        //
        public void ValidateFolder(String folderpath)
        {
            if (folderpath == null)
                throw new ArgumentNullException("folderpath");
            if (folderpath == String.Empty)
                throw new ArgumentException("folderpath can not be String.Empty");

            String wpath = folderpath.Replace(@"\\", @"\").Trim();

            if (File.Exists(wpath))
                throw new ArgumentException(String.Format("{0} is an existing file, not a folder", wpath));

            // if it doesn't exist, create it. Let it throw the exception if it fails
            if (!Directory.Exists(wpath))
                Directory.CreateDirectory(wpath);

            // if it still doesn't exist, it might be on a volume that is not there, so reset it
            if (!Directory.Exists(wpath))
                throw new ArgumentException(String.Format("{0} could not be created", wpath));
        }

    }
}
