using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class ProductCategoryFeatureDetailViewModel
    {
        public string title { get; set; }

        public long categoryFeatureId { get; set; }

        //rename : featureValues
        public List<ProductFeatureValueDetailViewModel> featureValues { get; set; }

        public string customValue { get; set; }

        public string htmlType { get;  set; }
    }
}
