using System;
using System.Reflection;
using Honshu.Cube;
using Honshu.Fetcher;
using Honshu.Fetcher.Cube;
using Honshu.Fetcher.Cube.Qiniu;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Qiniu.RS;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Honshu.Test
{
    [TestClass]
    public class UnitTest
    {
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        

        [TestMethod]
        public void TaobaoFetcherTest()
        {
            var fetcher = new Honshu.Fetcher.TaobaoFetcher();
            fetcher.GetProducts("https://zhilanjia.taobao.com/");
        }

        [TestMethod]
        public void Haodian8FetchTest()
        {
            var fetcher = new Haodian8ProductFetcher();
            var result = fetcher.Fetch();
        }

        [TestMethod]
        public void LogTest()
        {
            log.Info("test");
        }

        [TestMethod]
        public void GetResponseTest()
        {
            HttpHelper.GetResponse("http://union.click.jd.com/jdc?e=&p=AyIBZRprFQYaA1McXSVGTV8LRGtMR1dGXgVFTUdGW0pADgpQTFtLG18dBhQAUwQCUF5PNwdbME8YUEw8eSlzZlJzAkM5akVoWRMXVyUFFwBTElIQBiIHUhhSFjIQBlIdWxUCFzc0aWtDbBIGVBpaFAMWBFEraxUHEA5dHlkWABUCVRxrEg%3D%3D&t=W1dCFBBFC1pXUwkEAEAdQFkJBVsRChYBUh1ETEdOWg%3D%3D", false);
        }

        [TestMethod]
        public void GetPhotomaJSTest()
        {
            //PhantomJSHelper.GetPageSource("http://union.click.jd.com/jdc?e=&p=AyIBZRprFQYaA1McXSVGTV8LRGtMR1dGXgVFTUdGW0pADgpQTFtLG18dBhQAUwQCUF5PNwdbME8YUEw8eSlzZlJzAkM5akVoWRMXVyUFFwBTElIQBiIHUhhSFjIQBlIdWxUCFzc0aWtDbBIGVBpaFAMWBFEraxUHEA5dHlkWABUCVRxrEg%3D%3D&t=W1dCFBBFC1pXUwkEAEAdQFkJBVsRChYBUh1ETEdOWg%3D%3D");
        }

        [TestMethod]
        public void SegmentTest()
        {
            var result = PanguHelper.Segment("秋冬新款 韩版女装 兔绒修身纯色圆领长袖兔毛短款毛衣");
        }

        [TestMethod]
        public void TaobaoAPITest()
        {
            var item = TaobaoAPI.GetItem("44693897779");
        }

        [TestMethod]
        public void UploadFile()
        {
            EntryPath path = new EntryPath("honshu", "gogopher.jpg");
            Uploader.UploadFile("https://img.alicdn.com/bao/uploaded/i3/TB1V1NsIpXXXXX7aXXXXXXXXXXX_!!0-item_pic.jpg_300x300.jpg");
        }

        [TestMethod]
        public void GetImgInfo()
        {
            var result = Uploader.GetImgSize("http://7xkxvt.com1.z0.glb.clouddn.com/zhi/e79d15cde5b94a2baed5448939e0586a");
        }
    }
}
