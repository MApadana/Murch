using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.AdminModel
{
    public class GetCategoryFeatureDefaultViewModel
    {
        public long? categoryFeatureId { get;  set; }
        public string categoryFeatureTitle { get;  set; }

        public string featureValueTitle { get; set; }
        public string customValue { get;  set; }
        public long? featureValueId { get;  set; }
    }
}
