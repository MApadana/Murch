using Exon.Recab.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ReviewModel
{
    public class ReviewFilterModelWithCategory
    {
        public ReviewFilterModel Filter { get; set; }

        public CategoryFeature CategoryFeature  { get; set; }
    }
}
