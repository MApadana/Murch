using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exon.Recab.Service.Model.AdminModel
{
    public class CategoryViewModel
    {
        public long id { get;  set; }
        public string title { get;  set; }       
        public long? parentId { get;  set; }
        public int todayPriceChartlastRange { get;  set; }
        public int relativeCount { get; internal set; }
    }
}
