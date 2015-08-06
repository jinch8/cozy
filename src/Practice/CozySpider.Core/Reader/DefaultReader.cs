﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace CozySpider.Core.Reader
{
    public class DefaultReader : IUrlReader
    {
        public string Read(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url.Trim());
            request.AllowAutoRedirect = true;
            request.UserAgent = @"Mozilla/5.0 (Windows NT 5.2) AppleWebKit/534.30 (KHTML, like Gecko) Chrome/12.0.742.122 Safari/534.30";
            request.Accept = @"*/*";

            WebResponse respone = request.GetResponse();
            using (var rspStream = respone.GetResponseStream())
            {
                StreamReader reader = new StreamReader(rspStream, Encoding.Default);
                string result = reader.ReadToEnd();

                return result;
            }
        }
    }
}
