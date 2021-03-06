﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO; // for Path
using System.Xml.Linq; // for XElement
using System.Net; // for WebClient

using HtmlAgilityPack; // for retrieving well-formed web pages

// Settings Notes
//  cWebpage.cs looks for a default folder value as root for where to cache web pages in Common.settings (under Properties)
//
// using System.Configuration; // ?
using System.Xml; // for XmlWriter

namespace SJ.Common
{
    // The cWebpage class is used to cache local copies so that after the page id downloaded once, future references
    // don't have to be connected to the Internet to access the content. When scraping hundreds or thousands of pages, this is a huge
    // performance savings. I save the local copy raw so I can manually inspect it if I want or need to
    //
    // Access to content is either through ToString() which outputs the original downloaded html or ToXElement() which
    // provides it in a format compatible with Xml Linq processing, which my tools depend on
    //
    public class cWebpage
    {
        // cachePath is read from user Settings and a default is created on the User profile if it isn't 
        protected String _cacheRoot = null;
        private String cacheRoot
        {
            get
            {
                if (String.IsNullOrEmpty(_cacheRoot))
                {
                    // check settings to see if there is a cachePath
                    String p = Properties.Common.Default.CacheFolder;

                    if (String.IsNullOrEmpty(p))
                    {
                        // choose the default location off of the user profile

                        p = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // eg. c:\Users\jrjones\Documents
                        String appname = System.AppDomain.CurrentDomain.FriendlyName;  // will be the app name that uses this library

                        appname = appname.Replace(".exe", "");

                        _cacheRoot = p + @"\" + appname + @"\Webcache"; // root folder where we'll store local copies of web pages retrieved
                    }
                    else
                        _cacheRoot = p;

                    // use our helper class to make sure the cache folder exists, or creates it if necessary
                    SJHelper helper = new SJHelper();
                    helper.ValidateFolder(_cacheRoot);

                    // store the Setting for next time
                    Properties.Common.Default.CacheFolder = _cacheRoot;
                    Properties.Common.Default.Save();
                }
                return _cacheRoot;
            }
        }

        protected Uri _Uri = null; // set as part of construction

        // full Uri
        //
        public String Url { get { return _Uri.AbsoluteUri; } }

        // RefreshContent = true means content from the Web only
        //
        public bool RefreshContent = false; // defaults to false

        //  example: Host is www.microsoft.com in http://www.microsoft.com/technet/security/Bulletin/MS07-026.mspx
        //
        public String Host { get { return _Uri.Host; } }

        // example: Filename is MS07-026.mspx in http://www.microsoft.com/technet/security/Bulletin/MS07-026.mspx 
        private String _Filename = null;
        public String Filename
        {
            get
            {
                if (String.IsNullOrEmpty(_Filename))
                {
                    String p = _Uri.AbsolutePath;

                    if (p == "/")
                        p = "root-index";

                    // check to see if the last character is '/' and remove it 
                    while (p[p.Length - 1] == '/') p = p.Remove(p.Length - 1);

                    String[] tokens = p.Split('/');

                    _Filename = tokens[tokens.Length - 1];
                }
                return _Filename;
            }
        }

        // Holder for the original html loaded in a String. Start from this source when loading into 
        // HmtlAgilityPack docs for manipulation
        //
        private String _PageSourceString = null;
        private String PageSourceString
        {
            get
            {
                if (_PageSourceString == null)
                {
                    String src = Fetch();

                    _PageSourceString = src;
                }
                return _PageSourceString;
            }
        }

        // method to check to see if we were able to get content from the page
        public bool HasContent
        {
            get
            {
                if (String.IsNullOrEmpty(PageSourceString)) return false;
                else return true;
            }
        }

        // require URL as part of construction
        //
        public cWebpage(String url, bool refresh = false) {
            if (refresh)
                RefreshContent = true;

            _Uri = new Uri(url);
        }

        // by default do a Fetch, leveraging the value for RefreshContent
        //
        private String Fetch() { return Fetch(RefreshContent); }

        // Fetch either loads from a local cache copy of the file or from the web. If from the web, a local copy is always cached
        // * it is okay to try and retrieve a page that does not exist, but you should check HasContent
        //
        private String Fetch(bool refresh)
        {
            // if we already have it, just return
            if (!refresh) // if trying to use cache
                if (!String.IsNullOrEmpty(_PageSourceString)) 
                    return _PageSourceString; 

            // construct the local cache folder and filename
            String folderpath = Path.Combine(cacheRoot, Host);
            String filepath = Path.Combine(folderpath, Filename);

            // if we're supposed to get a fresh one, then delete local copy if it exists
            if ((refresh) && (File.Exists(filepath)))
                File.Delete(filepath);

            if (!File.Exists(filepath))
            {
                // we don't have a local copy, so got get it and save it locally

                // make sure the path exists so we can save it
                SJHelper h = new SJHelper();
                h.ValidateFolder(folderpath);

                // use WebClient to download, so we preserve original html format
                try
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(_Uri, filepath);
                } catch 
                {
                    _PageSourceString = String.Empty;
                    return String.Empty;
                }
            }

            // there should now be a local copy, so load it
            //HtmlDocument hd = new HtmlDocument();
            //hd.Load(filepath);
            StreamReader sr = new StreamReader(filepath);
            String htmltext = sr.ReadToEnd().Trim();
            sr.Close();

            // if no content && we did not pull from the web, try to pull from the web one more time
            if (String.IsNullOrEmpty(htmltext))
                if (!refresh)
                    return Fetch(true);
                else
                    File.Delete(filepath);  // if pulled from web, but no content, don't keep an empty local file
            
            return htmltext;
        }

        // return the html as a string
        //
        public override String ToString()
        {
            if (String.IsNullOrEmpty(PageSourceString))
                return String.Empty;
            else
                return PageSourceString;
            /*
            var htmlDoc = new HtmlDocument();
            htmlDoc.OptionOutputAsXml = false; // we want html
            htmlDoc.LoadHtml(PageSourceString);

            using (StringWriter sw = new StringWriter())
            {
                htmlDoc.Save(sw);
                return sw.ToString();
            }
            */
        }
        
        public XElement ToXElement()
        {
            if (String.IsNullOrEmpty(PageSourceString))
                return new XElement("Empty");

            var htmlDoc = new HtmlDocument();
            htmlDoc.OptionOutputAsXml = true; // set this option before loading
            htmlDoc.LoadHtml(PageSourceString);

            using (StringWriter sw = new StringWriter())
            {
                htmlDoc.Save(sw);
                using (StringReader sr = new StringReader(sw.ToString()))
                {
                    return XElement.Load(sr);
                }
            }
        }
    }


}
