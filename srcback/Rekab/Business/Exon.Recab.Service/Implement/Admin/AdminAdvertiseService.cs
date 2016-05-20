using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Service.Model.AdminModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;


namespace Exon.Recab.Service.Implement.Admin
{
    public class AdminAdvertiseService
    {
        private SdbContext _sdb;
        private  MdbContext _mdb;

        private readonly AdvertiseService _AdvertiseService;

        public AdminAdvertiseService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();

            _AdvertiseService = new AdvertiseService(ref _sdb, ref _mdb);
        }

        public List<AdminProductViewModel> ListProductByCategoryStatus(long categoryId,
                                                                        int status,
                                                                        ref long count,
                                                                        int feedback,
                                                                        int dealership,
                                                                        int size = 1,
                                                                        int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            ProdoctStatus ProdoctStatus = (ProdoctStatus)status;

            List<Product> products = _sdb.Product.Where(p => p.CategoryId == category.Id && p.Status == ProdoctStatus).ToList();

            switch (feedback)
            {
                case 1:
                    products = products.Join(_sdb.FeedbackProduct,
                                             p => p.Id,
                                             fp => fp.ProductId,
                                             (p, f) => p).ToList();
                    break;
                case 2:

                    products.RemoveAll(p => _sdb.FeedbackProduct.Any(fp => fp.ProductId == p.Id));

                    break;

                default:
                    break;

            }

            switch (dealership)
            {
                case 1:
                    products = products.Where(p => p.DealershipId.HasValue).ToList();
                    break;
                case 2:

                    products = products.Where(p => !p.DealershipId.HasValue).ToList();

                    break;

                default:
                    break;

            }


            count = products.Count;
            if (count == 0)
                return new List<AdminProductViewModel>();

            List<AdminProductViewModel> model = new List<AdminProductViewModel>();
            var temp = products.OrderBy(c => c.InsertDate).Skip(size * skip).Take(size).ToList();

            foreach (var p in temp)
            {
                string ADTitle = "";

                var collection = p.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle);

                foreach (var item in collection)
                {
                    var titleItem = item.ListFeatureValue.FirstOrDefault();
                    if (titleItem != null)
                    {
                        ADTitle = ADTitle + " " + titleItem.FeatureValue.Title;
                    }
                    else
                    {
                        ADTitle = ADTitle + item.CustomValue;
                    }
                }

                model.Add(new AdminProductViewModel
                {
                    advertiseId = p.Id,
                    description = p.Description,
                    imageUrl = _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product) != null ? _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product).MediaURL : "",
                    status = (int)p.Status,
                    dealershipId = p.DealershipId,
                    exchangeCategoryId = p.CategoryExchangeId,
                    title = ADTitle,
                    updateAble = p.UpdateCount > 0,
                    updateCount = p.UpdateCount,
                    insertDate = p.InsertDate.UTCToPersianDateLong(),
                    confirmDate = p.ConfirmDate.HasValue ? p.ConfirmDate.Value.UTCToPersianDateLong() : "تایید نشده",
                    packageTitle = p.UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.Title + "-" +
                                   p.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title,
                    cumUserId = p.UserId,
                    userName = p.User.FirstName + " " + p.User.LastName,
                    mobile = p.User.Mobile ?? "",
                    packageId = p.UserPackageCreditId.Value,
                    categoryTitle = p.Category.Title,
                    categoryId = p.CategoryId

                });
            }

            return model;
        }


        public bool ProductChangeStatus(long productId, int status)
        {
            Product Product = _sdb.Product.Find(productId);

            if (Product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            ProdoctStatus ProdoctStatus = (ProdoctStatus)status;

            Product.Status = ProdoctStatus;

            _sdb.SaveChanges();

            _AdvertiseService.MongoProductUpdate(Product);

            return true;

        }


        public List<FeedBackDetailViewModel> ProductFeedBack(long productId,
                                                             ref long count, 
                                                             int size = 1, 
                                                             int skip = 0)
        {

            List<FeedbackProduct> FeedbackProducts = _sdb.FeedbackProduct.Where(fp => fp.ProductId == productId).ToList();

            count = FeedbackProducts.Count;
            if (count == 0)
                return new List<FeedBackDetailViewModel>();

            return FeedbackProducts.Select(fp => new FeedBackDetailViewModel
            {
                feedBackId = fp.Id,
                userName = fp.User.FirstName + " " + fp.User.LastName,
                mobile = fp.User.Mobile ?? "",
                title = fp.CategoryFeatureTitle ?? "",
                body = fp.UserComment ?? "",
                advertise = fp.ProductId

            }).OrderBy(fp => fp.feedBackId).Skip(skip * size).Take(size).ToList();
        }

    }
}
