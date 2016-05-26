using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Hakone.UnitTest
{
    [TestClass]
    public class OAuthTest
    {
        [TestMethod]
        public void TestJsonSplit()
        {
            string token;
            string openID;
            var strJson = "{\"access_token\":\"2.00tDJdtBoBhJJB9abbe868290UlhMY\",\"remind_in\":\"157679999\",\"expires_in\":157679999,\"uid\":\"1738160941\"}";

            dynamic myObject = JsonConvert.DeserializeObject<dynamic>(strJson);
            token = myObject.access_token;
            openID = myObject.uid;
            int second = myObject.expires_in;

        }
    }
}
