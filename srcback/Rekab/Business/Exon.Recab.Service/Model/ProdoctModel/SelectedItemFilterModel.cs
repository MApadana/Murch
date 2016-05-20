using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class SelectItemFilterSearchModel
    {
        public long categoryFeatureId { get; set; }

        //rename : selectedFeatureValues
        public List<SelectedFeatureValue> selectedFeatureValues { get; set; }

        public string customValue { get; set; }

        public SelectItemFilterSearchModel()
        {
            selectedFeatureValues = new List<SelectedFeatureValue>();
        }
    }

}
