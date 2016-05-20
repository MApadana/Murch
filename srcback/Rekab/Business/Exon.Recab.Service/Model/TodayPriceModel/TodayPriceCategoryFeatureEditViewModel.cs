using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceCategoryFeatureEditViewModel
    {
        public long categoryFeatureId { get; set; }

     
        public string title { get; set; }

        //rename : featureValues
        public List<TodayPriceFeatureValueEditViewModel> featureValues { get; set; }

        public TodayPriceCategoryFeatureEditViewModel()
        {
            this.featureValues = new List<TodayPriceFeatureValueEditViewModel>();
        }
    }
}
