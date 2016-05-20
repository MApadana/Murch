using Exon.Recab.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.Recommend
{
   public class ReletiveCategoryFeature
    {
       public CategoryFeature CategoryFeature { get; set; }

       public List<FeatureValue> FeatureValues { get; set; }
    }
}
