using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack; // for retrieving well-formed web pages

// Settings
//  Webpage.cs looks for a default folder value as root for where to cache web pages in Common.settings (under Properties)
//
using System.Configuration;

namespace SJ.Common
{
    public class Webpage
    {
        // cachePath is read from user Settings and a default is created on the User profile if it isn't 
        private String _cachePath = null;
        private String cachePath
        {
            get
            {
                if (String.IsNullOrEmpty(_cachePath))
                {
                    // check settings to see if there is a cachePath
                    String p = Properties.Common.Default.CacheFolder;

                    if (String.IsNullOrEmpty(p))
                    {
                        // choose the default location off of the user profile

                        p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // eg. c:\Users\jrjones
                        String appname = System.AppDomain.CurrentDomain.FriendlyName;  // will be the app name that uses this library

                        appname = appname.Replace(".exe", "");

                        _cachePath = p + @"\" + appname + @"\Webcache"; // root folder where we'll store local copies of web pages retrieved
                    }
                    else
                        _cachePath = p;

                    // use our helper class to make sure the cache folder exists, or creates it if necessary
                    SJHelper helper = new SJHelper();
                    helper.ValidateFolder(_cachePath);
                }
                return _cachePath;
            }
        }

        protected String _Url = null;
        public String Url
        {
            get
            {
                // this should always be set as part of Construction
                return _Url;
            }
            set
            {  // setting it can be done, and it resets everything
                /*
                _SiteName = null;
                _RawSource = null;
                _SourceText = null;
                _Domain = null;
                _CacheFolder = null;
                _Filename = null; */
                _Url = value;
            }
        }

        public Webpage()
        { }

        // require URL as part of construction
        //
        public Webpage(String theurl)
        {
            
            _Url = theurl;
        }

        public String Fetch() { return Fetch(false); }

        public String Fetch(bool flushcache)
        {
            Console.WriteLine(cachePath);

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(Url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            return "Node Name: " + node.Name + "\n" + node.OuterHtml;
        }
    }


}
