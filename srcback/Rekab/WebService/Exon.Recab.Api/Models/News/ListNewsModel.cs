using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.News
{
    public class ListNewsModel
    {

        public int pageSize { get; set; }

        public int pageIndex { get; set; }

        public NewsType type { get;  set; }
    }
}