using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Service.Implement.Recommend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.AddAllTag
{
    class Program
    {
        static void Main(string[] args)
        {
            SdbContext _sdb = new SdbContext();

            RecommendService _RecommendService = new RecommendService();
            long[] list = { 3, 4, 26, 25, 37, 42, 44, 46, 47, 48, 50 , 51,52,72,83,88,90,93,94,96 };

            List<FeatureValue> featureValues = _sdb.FeatureValues.Where(i=> list.ToList().Any(k=> k ==i.CategoryFeatureId)  ).ToList();

            foreach (var item in featureValues)
            {
                _RecommendService.AddTageForFeatureValue(featureValueId: item.Id, tag: item.Title);
            }

        }
    }
}
