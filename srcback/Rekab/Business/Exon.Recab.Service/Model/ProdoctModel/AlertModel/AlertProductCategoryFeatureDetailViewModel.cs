using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel.AlertModel
{
    public class AlertProductCategoryFeatureDetailViewModel
    {
        public long categoryFeatureId { get; set; }

        public string customValue { get; set; }

        public List<long> selectedFeatureValues { get; set; }

        public AlertProductCategoryFeatureDetailViewModel()
        {
            selectedFeatureValues = new List<long>();
        }
    }
}
