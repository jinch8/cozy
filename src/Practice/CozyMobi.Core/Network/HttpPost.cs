﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CozyMobi.Core.Model;
using System.Net;
using CozyMobi.Core.RequestBuilder;

namespace CozyMobi.Core.Network
{
    class HttpPost
    {
        public static HttpResponseMessage Post(string url, HttpContent content)
        {
            HttpClientHandler handler = new HttpClientHandler { UseCookies = false };
            HttpClient client = new HttpClient(handler);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
            if (AccountInfo.Instance.Cookie != null)
            {
                request.Headers.Add("Cookie", AccountInfo.Instance.Cookie);
            }
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (RequestBuilderCommon.AccountLogin == url)
            {
                var v = response.Headers.GetValues("Set-Cookie").ToArray();
                if (v.Count() > 0)
                {
                    AccountInfo.Instance.Cookie = v[0];
                }
            }
            return response;
        }
    }
}
