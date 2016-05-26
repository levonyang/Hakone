using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hakone.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hakone.UnitTest
{
    [TestClass]
    public class ExtTest
    {
        [TestMethod]
        public void TestAvatarHandler()
        {
            var testStr = string.Empty;
            testStr.AvatarHandler(13442300);
            testStr.AvatarHandler(30);
            testStr.AvatarHandler(930);
            testStr.AvatarHandler(922);
            testStr.AvatarHandler(900);
        }
    }
}
