using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.FeedbackModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Implement.FeedBack
{
    public class FeedBackService
    {
        public readonly SdbContext _sdb;

        public FeedBackService()
        {
            _sdb = new SdbContext();
        }

        public List<FeedBackCfModel> GetAllFeedbackCategoryFeature(long categoryId)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            var temp = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id && cf.AvailableInFeedback);

            return temp.Count() == 0 ? new List<FeedBackCfModel>() : temp.Select(cf => new FeedBackCfModel { categoryFeatureId = cf.Id, title = cf.Title  }).ToList();
        }

        public bool AddFeedback(long userId, long productId, string comment, string categoryFeatureTitle)
        {

            Product product = _sdb.Product.Find(productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);
           
            if (_sdb.FeedbackProduct.Any(fb => fb.UserId == userId && fb.ProductId == product.Id))
                throw new RecabException((int)ExceptionType.FeedBackExist);

            _sdb.FeedbackProduct.Add(new FeedbackProduct
            {
                UserComment = comment,
                ProductId = product.Id,
                CategoryFeatureTitle = categoryFeatureTitle,
                UserId = userId                
            });

            try
            {
                _sdb.SaveChanges();
            }
            catch (Exception e)
            {
                throw new RecabException(e.Message);
            }
            return true;
        }


    }
}
