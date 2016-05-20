using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceCategoryGroupByViewModel
    {
        public long categoryFeatureId { get;set; }

        public List<TodayPriceGroupByResultItemViewModel> items { get;set; }
    }
}
