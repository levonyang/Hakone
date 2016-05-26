using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hakone.SendCloud
{
    public class MailEndityWithTemplate
    {
        public string ApiUser { get; set; }

        public string ApiKey { get; set; }

        public string From { get; set; }

        public string XsmtpApi { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string TemplateInvokeName { get; set; }

        public string FromName { get; set; }

        public string ReplyTo { get; set; }

        public int LabelId { get; set; }

        public string Headers { get; set; }

        public string Attachments { get; set; }

        public bool RespEmailId { get; set; }

        public bool UseNotification { get; set; }

        public bool UseAddressList { get; set; }
    }
}
