using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ConfigModel
{
    public class InitProductExchangeViewModel
    {
        //rename2 : advertiseCategoryFeatureValues
        public List<CategoryFeatureValuesResultModel> advertiseCategoryFeatureValues { get; set; }

        //rename2 : exchangeCategoryFeatureValues
        public List<ExchangeProductselectedItemViewModel> exchangeCategoryFeatureValues { get; set; }


        //rename2 : exchangeCategories

        public List<ExchangeCategoryViewModel> exchangeCategory { get; set; }
        public InitProductExchangeViewModel()
        {
            advertiseCategoryFeatureValues = new List<CategoryFeatureValuesResultModel>();
            exchangeCategoryFeatureValues = new List<ExchangeProductselectedItemViewModel>();
            exchangeCategory = new List<ExchangeCategoryViewModel>();
        }

    }
}
