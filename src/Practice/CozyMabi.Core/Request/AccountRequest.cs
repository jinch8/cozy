﻿using CozyMabi.Core.RequestBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyMabi.Core.Network;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CozyMabi.Core.ResponseParser;
using CozyMabi.Core.Model;

namespace CozyMabi.Core.Request
{
    public class AccountRequest
    {
        private AccountRequestBuilder mBuild = new AccountRequestBuilder();
        private AccountResponseParser mParser = new AccountResponseParser();

        public void Login(string email, string password, string j_captcha = null)
        {
            HttpContent content = mBuild.Login(email, password, j_captcha);
            HttpResponseMessage rsp = HttpPost.Post(RequestBuilderCommon.AccountLogin, content);
            JObject jo = null;
            if (ResponseParserCommon.AreYouOk(rsp, ref jo))
            {
//                 String key = jo["data"]["global_key"].ToString();
//                 String url = RequestBuilderCommon.Host + "/api/user/key/" + key;
//                 HttpGet.Get(url);
            }
        }
    }
}
