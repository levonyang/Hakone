using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.RS;
using Qiniu.RPC;
using Qiniu.Conf;
using PGK.Extensions;
using Honshu.Cube;
using System.Configuration;
using Newtonsoft.Json;
using Qiniu.FileOp;

namespace Honshu.Fetcher.Cube.Qiniu
{
    public class Uploader
    {
        static Uploader()
        {
            Config.Init();
        }
        private static bool UploadRemoteFile(string url, EntryPath path)
        {
            RSClient client = new RSClient();
            CallRet result = client.Fetch(url, path);
            return result.OK;
        }

        public static string UploadFile(string url,string channel="")
        {
            var guid = Guid.NewGuid().ToString().RemoveDash();
            EntryPath path = new EntryPath(Buket, channel + "/" + guid);
            if (UploadRemoteFile(url, path))
            {
                return channel + "/" + guid;
            }
            return string.Empty;
        }

        public static Tuple<int,int>  GetImgSize(string url)
        {
            ImageInfoRet ret = ImageInfo.Call(url + "?imageInfo");

            return new Tuple<int, int>(ret.Width, ret.Height);
        }

        public static string Buket
        {
            get { return ConfigurationManager.AppSettings["qiniu_Buket"]; }
        }

        public static string BuketDomain
        {
            get { return ConfigurationManager.AppSettings["qiniu_Buket_Domain"]; }
        }
    }
}
