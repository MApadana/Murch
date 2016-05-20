using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class CFAggregateModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public List<FeatuerValueAggModel> feachurValue { get; set; }

        public CFAggregateModel()
        {
            feachurValue = new List<FeatuerValueAggModel>();
        }
    }
}
