using Exon.Recab.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class FilterAndCategoryFeatureModel
    {
        public CategoryFeature CategoryFeature { get; set; }

        public CFProdoctFilterModel Filter { get; set; }

        public FilterAndCategoryFeatureModel()
        {
            Filter = new CFProdoctFilterModel();
            CategoryFeature = new CategoryFeature();
        }
    }
}
