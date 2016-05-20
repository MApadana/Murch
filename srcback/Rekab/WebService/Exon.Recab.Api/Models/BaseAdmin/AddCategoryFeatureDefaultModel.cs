using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class AddCategoryFeatureDefaultModel
    {
        public long id { get; set; }
        public bool isEnable { get; set; }
        public long categoryFeatureId { get;  set; }
        public string customValue { get; set; }
        public long? featureValueId { get;  set; }
       
    }
}
