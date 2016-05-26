using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hakone.Web.Models
{
    public class PageInfo
    {
        //总记录数
        public int TotalRecords { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get 
            {
                return (int)Math.Ceiling((decimal)TotalRecords / PageSize);
            }
        }
    }
}