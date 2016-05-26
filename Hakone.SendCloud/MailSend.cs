using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.SendCloud
{
    public class MailSend
    {
        public static string Send(MailEndityWithTemplate mail)
        {
            var apiUrl = "http://api.sendcloud.net/apiv2/mail/sendtemplate";

            try
            {
                var client = new HttpClient();

                var paramList = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("apiUser", mail.ApiUser),
                    new KeyValuePair<string, string>("apiKey", mail.ApiKey),
                    new KeyValuePair<string, string>("from", "no-reply@haodian8.com"),
                    new KeyValuePair<string, string>("fromName", "好店8"),
                    new KeyValuePair<string, string>("subject", mail.Subject),
                    new KeyValuePair<string, string>("templateInvokeName", mail.TemplateInvokeName),
                    new KeyValuePair<string, string>("xsmtpapi", mail.XsmtpApi)
                };


                var response = client.PostAsync(apiUrl, new FormUrlEncodedContent(paramList)).Result;
                var result = response.Content.ReadAsStringAsync().Result;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return e.Message;
            }
        }
    }
}
