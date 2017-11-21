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

                        p = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile); // eg. c:\Users\jrjones
                        String appname = System.AppDomain.CurrentDomain.FriendlyName;  // will be the app name that uses this library

                        appname = appname.Replace(".exe", "");

                        _cacheRoot = p + @"\" + appname + @"\Webcache"; // root folder where we'll store local copies of web pages retrieved
                    }
                    else
                        _cacheRoot = p;

                    // use our helper class to make sure the cache folder exists, or creates it if necessary
                    SJHelper helper = new SJHelper();
                    helper.ValidateFolder(_cacheRoot);
                }
                return _cacheRoot;
            }
        }

        protected Uri _Uri = null;
        public String Url
        {
            get
            {
                // this should always be set as part of Construction
                return _Uri.AbsoluteUri;
            }
            set
            {
                // setting it can be done, and it resets everything
                /*
                _SiteName = null;
                _RawSource = null;
                _SourceText = null;
                _Filename = null; */
                _Domain = null;
                _Uri = new Uri(value);
            }
        }

        //  example: Domain is microsoft.com in http://www.microsoft.com/technet/security/Bulletin/MS07-026.mspx
        //
        protected String _Domain = null;
        public String Domain
        {
            get
            {
                if (_Domain == null)
                {
                    ParseUrl();
                }
                return _Domain;
            }
        }

        public Webpage()
        { }

        // require URL as part of construction
        //
        public Webpage(String theurl)
        {
            
            Url = theurl;
        }

        // chops up the URL and sets _SiteName and _Domain
        virtual public void ParseUrl()
        {
            // sample http://www.microsoft.com/technet/security/Bulletin/MS07-026.mspx
            String[] tokens = Url.Split('/');
            String _SiteName = tokens[0] + @"//" + tokens[2];
            _Domain = HostToDomain(tokens[2]);

        }

        public String Fetch() { return Fetch(false); }

        public String Fetch(bool flushcache)
        {
            Console.WriteLine(cacheRoot);

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(Url);
            

            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");

            return "Node Name: " + node.Name + "\n" + node.OuterHtml;
        }

        // simple domain extractor that won't work for everything (doubledoms defined by godaddy.com)
        //
        // NOTE: Replace with Uri.GetLeftPart(UriPartial.Authority)
        //
        protected String HostToDomain(String hostname)
        {
            String[] doubledoms = { "com", "co", "net", "org", "nom", "firm", "gen", "ind" };

            // example www.microsoft.com and www.microsoft.co.uk

            // let's just get rid of www., since we don't want it
            String ws = hostname.Replace("www.", "");

            String[] tokens = ws.Split('.');
            int len = tokens.Length;


            // if we don't have 3 segments, assume it is reduced as much as possible
            if (tokens.Length < 3) return ws;

            // the 2nd to last token matches a doubledom, use the last 3 segments
            foreach (String dom in doubledoms)
                if (tokens[len - 2] == dom)
                    return tokens[len - 3] + "." + tokens[len - 2] + "." + tokens[len - 1];

            // no previous matches, so just return the last two segments
            return tokens[len - 2] + "." + tokens[len - 1];
        }


    }


}
