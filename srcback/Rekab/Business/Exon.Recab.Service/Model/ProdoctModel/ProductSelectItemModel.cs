using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class ProductSelectItemModel
    {
        public long CategoryFeatureId { get; set; }

        public List<long> FeatureValueId { get; set; }

        public string CustomValue { get; set; }

        public ProductSelectItemModel()
        {
            FeatureValueId = new List<long>();
        }
    }
}
