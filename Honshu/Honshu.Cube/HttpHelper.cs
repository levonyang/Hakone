using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Honshu.Cube
{
    public class HttpHelper
    {
        /// <summary>
        /// 返回request请求的内容
        /// call like this: var response = await GetResponse("http://www.baidu.com/"); 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Tuple<string,string> GetResponse(string url, bool autoRedirect=true)
        {
            try
            {
                var handler = new HttpClientHandler();

                handler.AllowAutoRedirect = autoRedirect;

                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Add("user-agent",
                "Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.134 Safari/537.36");
                var location = string.Empty;
                var reponseStr = string.Empty;

                
                var response = httpClient.GetAsync(new Uri(url)).Result;

                reponseStr = response.Content.ReadAsStringAsync().Result;

                IEnumerable<string> values;
                if (response.Headers.TryGetValues("location", out values))
                {
                    location = values.First();
                }

                return new Tuple<string, string>(reponseStr, location);
            }
            catch (AggregateException e)
            {
                var a= e.ToString();
                return new Tuple<string, string>(string.Empty,string.Empty);
            }
        }

        public static HttpResult GetHtml(string url)
        {
            Thread.Sleep(2000);
            HttpUtil http = new HttpUtil();
            HttpItem request = new HttpItem();

            //request.ProxyIp = "61.152.102.40:8088";

            var header = new WebHeaderCollection();
            header.Add("Cookie", "v=0; cna=KojpDap28xkCAd5MpOIa2PAt; thw=cn; lzstat_uv=22267468321177670866|3314048@3314489@3314431; lzstat_ss=962509518_0_1432557422_3314048|206977203_0_1432557424_3314489|1732851143_0_1432557427_3314431; t=d4b8c2b00fefbe985c876c7c9954c071; linezing_session=d06LsAesW5L2Esx72jJPYkaM_1432534904541EFVB_1; _m_h5_tk=c953b11ec81435fdd3abb076d6043a8d_1432540125009; _m_h5_tk_enc=122a7eef2911ba5b17edfcf7281e35ec; cookie2=1c79dab22d25f3645021d41bdffcc864; pnm_cku822=063UW5TcyMNYQwiAiwQRHhBfEF8QXtHcklnMWc%3D%7CUm5Ockt1SnJMdkl9RntGciQ%3D%7CU2xMHDJ7G2AHYg8hAS8WKAYmCFQ1Uz9YJlxyJHI%3D%7CVGhXd1llXGJdZVthXmpRbFFkU25MdEpzR39EfkZ%2FRHxBdUp3TGI0%7CVWldfS0QMA83AiIdPRM%2FQW85bw%3D%3D%7CVmhIGCUFOBgkEC8SMg03Di4SJhkkBDgFMA0tESUaJwc7Bj8CVAI%3D%7CV25Tbk5zU2xMcEl1VWtTaUlwJg%3D%3D; mt=ci%3D-1_0; _tb_token_=e8f0e7ee313e3; l=Apubr1Vv58ZiYePWfQiyqGlNq/EFcK9y; isg=D38C29BDE327C937997E17CA4CABC9EE");
            header.Add("Cache-Control", "max-age=0");
            header.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            header.Add("Accept-Language", "en-US,en;q=0.8,zh-CN;q=0.6,zh;q=0.4");
            header.Add("Accept-Encoding", "gzip,deflate");
            header.Add("Pragma", "");

            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Referer = "http://www.taobao.com/";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.65 Safari/537.36";
            request.Timeout = 10000;
            request.Allowautoredirect = true;
            request.Header = header;
            request.URL = url;

            return http.GetHtml(request);
        }

    }
}
