using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Service.Model.ReviewModel;

namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceChartModel
    {
        public long todayPriceId { get; set; }

        public string tolerance { get; set; }

        public string price { get; set; }

        //rename : priceItems
        public List<string> priceItems { get; set; }

        //rename : dateItems
        public List<string> dateItems { get; set; }

        public ReviewViewModel review { get; set; }

        public TodayPriceChartModel()
        {
            priceItems = new List<string>();

            dateItems = new List<string>();
        }
    }
}
