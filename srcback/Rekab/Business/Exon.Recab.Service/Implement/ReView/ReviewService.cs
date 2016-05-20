using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.ReviewModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Web.Script.Serialization;
using Exon.Recab.Service.Implement.Helper;
using Exon.Recab.Domain.Constant.CS.Exception;
using System.Text;
using Exon.Recab.Service.Model.PublicModel;

namespace Exon.Recab.Service.Implement.ReView
{
    public class ReviewService
    {
        #region init
        private SdbContext _sdb;
        private MdbContext _mdb;

        internal ReviewService(ref SdbContext sdb, ref MdbContext mdb)
        {
            this._sdb = sdb;
            this._mdb = mdb;
        }

        public ReviewService()
        {
            this._sdb = new SdbContext();
            this._mdb = new MdbContext();
        }
        #endregion

        #region add
       
        public bool AddNewReview(long userId,
                                  long categoryId,
                                  string body,
                                  List<SelectItemModel> reviewItem,
                                  List<MediaModel> media)

        {
            #region Validation

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            Category category = _sdb.Categoris.Find(categoryId);


            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId && cf.AvailableInReview).ToList();

            string RvCfTitle = "";

            List<SelectItemModel> ConfirmItems = reviewItem.GetConfirmedItems(entityCategoryFeature: InsertList,
                                                                              reqiredCategoryFeature: InsertList.Where(cf => cf.RequiredInRVInsert).ToList(),
                                                                              titleItems: InsertList.Where(cf => cf.AvailableInRVTitle).ToList(),
                                                                              title: ref RvCfTitle
                                                                              );

            #endregion

            #region Header


            Review newReview = new Review
            {
                Title = RvCfTitle,
                Body = body ?? "",
                UserId = user.Id,
                CategoryId = category.Id,
                Rate = 0,
                VisitCount = 0,
                CreateTime = DateTime.UtcNow
            };

            _sdb.Reviews.Add(newReview);

            _sdb.SaveChanges();


            #endregion

            #region FeatureValue

            foreach (var item in ConfirmItems)
            {
                FeatureValueAssign FeatureValueAssign;
                if (item.FeatureValueIds.Count > 0)
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Review,
                        CustomValue = "",
                        EntityId = newReview.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                    foreach (var fvItem in item.FeatureValueIds)
                    {
                        FeatureValueAssign.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem { FeatureValueId = fvItem });
                    }

                }
                else
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Review,
                        CustomValue = item.CustomValue,
                        EntityId = newReview.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                }

                _sdb.FeatureValueAssign.Add(FeatureValueAssign);
            }

            #endregion

            #region Media
            if (media.Count > 0)
            {
                foreach (var item in media)
                {
                    _sdb.Media.Add(new Media
                    {
                        MediaURL = item.Url,
                        EntityType = EntityType.Review,
                        EntityId = newReview.Id,
                        MediaType = (MediaType)((int)item.Type),
                        Order = item.OrderId

                    });
                }

            } 
            #endregion

            _sdb.SaveChanges();

            MongoReviewSave(newReview);

            return true;
        }


        #endregion

        #region  edit
        public bool EditReview(long reviewId, string body, List<MediaModel> media)
        {
            Review review = _sdb.Reviews.Find(reviewId);

            if (review == null)
                throw new RecabException((int)ExceptionType.ReviewNotFound);

            review.Body = body ?? "";

            _sdb.Media.RemoveRange(_sdb.Media.Where(m => m.EntityId == review.Id && EntityType.Review == m.EntityType));

            if (media.Count > 0)
            {

                foreach (var item in media)
                {
                    _sdb.Media.Add(new Media
                    {
                        MediaURL = item.Url,
                        EntityType = EntityType.Review,
                        EntityId = review.Id,
                        MediaType = (MediaType)item.Type,
                        Order = item.OrderId

                    });
                }
               
            }


            _sdb.SaveChanges();


            return true;

        }

        public RateResultViewModel AddRate(long userId, long reviewId, int rate)
        {
            if (rate > 5 || rate < 0)
                throw new RecabException((int)ExceptionType.BadRateData);

            Review review = _sdb.Reviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                throw new RecabException((int)ExceptionType.ReviewNotFound);


            var existRate = _mdb.Rates.Find(GetMongoRateFilter(userId: userId, reviewId: reviewId)).ToList();


            var AllRate = _mdb.Rates.Find(new BsonDocument { { "ReviewId", review.Id.ToString() } })
                                    .Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();

            if (existRate.Count() > 0)
            { _mdb.Rates.DeleteMany(filter: Builders<BsonDocument>.Filter.Eq("_id", existRate.First()["_id"])); };


            if (existRate.Count() > 0)
            {
                double t1 = Convert.ToDouble(existRate.First()["Rate"].ToString());


                review.Rate = ((review.Rate * AllRate.Count()) + (rate - t1)) / ((AllRate.Count() == 0 ? 1 : (AllRate.Count())));
            }
            else
            {
                review.Rate = (double)((double)(review.Rate * AllRate.Count()) + rate) / ((double)(AllRate.Count() + 1));
            }
            _sdb.SaveChanges();

            _mdb.Rates.InsertOne(document: new BsonDocument { {"ReviewId", review.Id.ToString() },
                                                               {"UserId", userId.ToString() } ,
                                                               {"Rate", rate.ToString() } });

            return new RateResultViewModel { rate = (float)review.Rate };
        }

        #endregion

        #region Search
        public List<ReviewViewModel> GetALLReview(long categoryId, ref long count, int size = 1, int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<Review> reviews = _sdb.Reviews.Where(r => r.CategoryId == category.Id).ToList();
            count = reviews.Count;
            return reviews.Count == 0 ? new List<ReviewViewModel>() :
                reviews.OrderBy(r => r.Id).Skip(size * skip).Take(size)
                .Select(r => new ReviewViewModel
                {
                    reviewId = r.Id,
                    title = r.Title,
                    htmlContent = r.Body,
                    rate = r.Rate,
                    visitCount = r.VisitCount,
                    createTime = r.CreateTime.UTCToPersianDateLong()
                }).ToList();


        }

        public ReviewViewModel GetSingleReview(long reviewId, long userId)
        {
            Review review = _sdb.Reviews.Find(reviewId);

            if (review == null)
                throw new RecabException((int)ExceptionType.ReviewNotFound);


            Double userRate = 0;

            if (userId != 0)
            {
                var existRate = _mdb.Rates.Find(GetMongoRateFilter(userId: userId, reviewId: reviewId)).ToList();
                if (existRate.Count() > 0)
                { userRate = Convert.ToDouble(existRate.First()["Rate"].ToString()); }

            }

            review.VisitCount = review.VisitCount + 1;
            _sdb.SaveChanges();

            List<CategoryFeature> categoryFeature = _sdb.CategoryFeatures.Where(c => c.CategoryId == review.CategoryId &&
                                                                                     c.AvailableInReview).ToList();

            var categoryFeatureTitle = categoryFeature.Where(cf => cf.AvailableInRVTitle)
                                                       .Join(_sdb.FeatureValueAssign.Where(fva => fva.EntityId == review.Id && fva.EntityType == EntityType.Review),
                                                             cf => cf.Id,
                                                             fva => fva.CategoryFeatureId,
                                                             (cf, fva) => fva).ToList();


            List<BsonDocument> obj = _mdb.Reviews.Find(Builders<BsonDocument>.Filter.Eq("Id", review.Id.ToString())).Project(new BsonDocument()).ToList();


            Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(obj.FirstOrDefault());

            JavaScriptSerializer js = new JavaScriptSerializer();

            List<ReviewCategoryFeatureViewModel> model = js.Deserialize<List<ReviewCategoryFeatureViewModel>>
                (Encoding.UTF8.GetString(Convert.FromBase64String(data["Result"].ToString())));



            var search = model.Where(m => categoryFeatureTitle.Where(cf => cf.CategoryFeature.AvailableInLigthSearch).Any(cf => cf.CategoryFeatureId == m.categoryFeatureId)).ToList();

            //model.RemoveAll(m => categoryFeatureTitle.Any(cf => cf.CategoryFeatureId == m.categoryFeatureId));

            List<string> media = _sdb.Media.Where(m => m.EntityId == review.Id && EntityType.Review == m.EntityType)
                                         .OrderBy(m => m.Order)
                                         .Select(m => m.MediaURL).ToList();


            var temp = categoryFeature.FirstOrDefault(cf => cf.Id == search.First().categoryFeatureId && cf.AvailableFVIcon);

            string logoURL = "";

            if (temp != null)
            {
                var categoryLogo = search.First();
                var featureValueLogo = categoryLogo.featureValues.First().featureValueId;
                var logo = _sdb.Media.FirstOrDefault(m => m.EntityId == featureValueLogo && m.EntityType == EntityType.FeatureValue);

                logoURL = logo != null ? logo.MediaURL : "";
            }

            string articleSearchKeyword = "";
            var brand = _sdb.FeatureValueAssign.Where(fva=> fva.CategoryFeature.AvailableInArticle && fva.EntityType == EntityType.Review && fva.EntityId == review.Id).OrderBy(pcf => pcf.CategoryFeature.OrderId).FirstOrDefault();

            BsonDocument Result = _mdb.Tag.Find(new BsonDocument { { "Title", brand.ListFeatureValue.FirstOrDefault().FeatureValue.Title } }).FirstOrDefault();

            if (Result.GetElement("ArticleItems").Value.AsBsonArray.Count > 0)
               articleSearchKeyword = brand.ListFeatureValue.FirstOrDefault().FeatureValue.Title;

            return new ReviewViewModel
            {

                title = review.Title,
                htmlContent = review.Body,
                rate = review.Rate,
                userRate = (float)userRate,
                visitCount = review.VisitCount,
                createTime = review.CreateTime.UTCToPersianDateLong(),
                reviewCategoryFeatures = model,
                reviewId = reviewId,
                categoryId = review.Category.Id,
                categoryTitle = review.Category.Title,
                advertiseSearchFilterItems = search.Select(sa => new Model.ReviewModel.SelectItemFilterSearchModel
                {
                    categoryFeatureId = sa.categoryFeatureId,
                    selectedFeatureValues = sa.featureValues.Select(fva => fva.featureValueId).ToList(),
                    customValue = ""
                }).ToList(),
                todayPriceSearchFilterItems = search.Select(sa => new TodayPriceSearchFilterViewModel
                {
                    categoryFeatureId = sa.categoryFeatureId,
                    featureValueIds = sa.featureValues.Select(fva => fva.featureValueId).ToList()
                }).ToList(),
                articleSearchKeyword = articleSearchKeyword,
                logoUrl = logoURL,
                mediaUrl = media.Count > 0 ? media : new List<string>()
            };


        }

        public AdminReviewViewModel InitEditReview(long reviewId)
        {
            Review review = _sdb.Reviews.Find(reviewId);

            if (review == null)
                throw new RecabException((int)ExceptionType.ReviewNotFound);

            List<FeatureValueAssign> AssingeFeature = _sdb.FeatureValueAssign.Where(cf => cf.EntityId == review.Id &&
                                                                                        cf.EntityType == EntityType.Review &&
                                                                                        cf.CategoryFeature.RequiredInRVInsert).ToList();

            List<string> media = _sdb.Media.Where(m => m.EntityId == review.Id && EntityType.Review == m.EntityType)
                                         .OrderBy(m => m.Order)
                                         .Select(m => m.MediaURL).ToList();


            return new AdminReviewViewModel
            {
                title = review.Title,
                body = review.Body,
                reviewId = reviewId,
                media = media.Count > 0 ? media : new List<string>(),
                categoryFeatures = AssingeFeature.Select(cf => new ReviewCategoryFeatureEditViewModel
                {
                    title = cf.CategoryFeature.Title,
                    categoryFeatureId = cf.CategoryFeatureId,
                    featureValue = cf.ListFeatureValue.Select(fv => new ReviewFeatureValueEditViewModel
                    {
                        featureValueId = fv.FeatureValueId,
                        title = fv.FeatureValue.Title
                    }).ToList()
                }).ToList()
            };


        }

        public List<ReviewSearchResultItemViewModel> ReviewSearch(List<ReviewFilterModel> filter,
                                                                   string keyword,
                                                                    long categoryId,
                                                                    ref long count,
                                                                    int size = 1,
                                                                    int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<CategoryFeature> categoryFeature = _sdb.CategoryFeatures.Where(cf => cf.AvailableInRVTitle && cf.CategoryId == categoryId)
                                                         .Take(2).OrderBy(cf => cf.OrderId).ToList();

            if (categoryFeature.Count != 2)
                throw new RecabException("config error system have no 2 element for review search");

            List<ReviewFilterModelWithCategory> filterCategory = new List<ReviewFilterModelWithCategory>();

            foreach (var item in filter)
            {
                if (categoryFeature.Any(cf => cf.Id == item.CategoryFeatureId))
                {
                    if (!categoryFeature.First(cf => cf.Id == item.CategoryFeatureId).FeatureValueList.Any(fv => fv.Id == item.FeatureValueId))
                    {
                        throw new RecabException((int)ExceptionType.FeatureValueInvalid);
                    }
                    else
                    {

                        filterCategory.Add(new ReviewFilterModelWithCategory
                        {
                            CategoryFeature = categoryFeature.First(cf => cf.Id == item.CategoryFeatureId),
                            Filter = item
                        });
                    }
                }
                else
                    throw new RecabException((int)ExceptionType.CategoryFeatureInvalid);
            }

            JavaScriptSerializer scr = new JavaScriptSerializer();


            BsonDocument match = ProductHelperService.GetMongoFilterSearchRV(categoryId,
                                                                            filter: filterCategory,
                                                                            keyword: keyword,
                                                                            _sdb : ref _sdb,
                                                                            _mdb: ref _mdb);

            
            var MongoResult = _mdb.Reviews.Find(match).ToList();

            List<ReviewSearchResultItemViewModel> model = new List<ReviewSearchResultItemViewModel>();

            foreach (var resultItem in MongoResult)
            {

                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(resultItem);

                long reviewId = 0;
                long.TryParse(data["Id"].ToString(), out reviewId);

                Media media = _sdb.Media.FirstOrDefault(m => m.EntityId == reviewId && m.EntityType == EntityType.Review);

                Review review = _sdb.Reviews.Find(reviewId);

                if (review == null)

                    return new List<ReviewSearchResultItemViewModel>();

                ReviewSearchResultItemViewModel temp = new ReviewSearchResultItemViewModel
                {
                    reviewId = reviewId,
                    imageUrl = media != null ? media.MediaURL : "",
                    rate = review.Rate.ToString(),
                    title = review.Title,
                };


                model.Add(temp);

            }
            count = model.Count();

            return model.OrderBy(m => m.reviewId).Skip(size * skip).Take(size).ToList();

        }

        public ReviewGroupByViewModel ReviewSearchLogo(long categoryId)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            CategoryFeature Brand = _sdb.CategoryFeatures.Where(cf => cf.AvailableInReview && cf.CategoryId == categoryId).OrderBy(cf => cf.OrderId).FirstOrDefault();

            if (Brand == null)
                throw new RecabException("config error system have no 1 element for Review search");

            ReviewGroupByViewModel model = new ReviewGroupByViewModel { categoryFeatureId = Brand.Id };

            List<ReviewLogoItemViewModel> logos = Brand.FeatureValueList.Join(_sdb.Media.Where(m => m.EntityType == EntityType.FeatureValue),
                                                        fv => fv.Id,
                                                        m => m.EntityId,
                                                        (fv, m) => new ReviewLogoItemViewModel
                                                        {
                                                            featureValueId = fv.Id,
                                                            featureValueTitle = fv.Title,
                                                            logoUrl = m.MediaURL
                                                        }
                                                 ).ToList();
            model.featureValues = logos;

            return model;
        }


        #endregion

        #region delete
        public bool RemoveReview(long reviewId)
        {
            Review review = _sdb.Reviews.Find(reviewId);

            if (review == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            _sdb.Reviews.Remove(review);


            _mdb.Reviews.DeleteMany(filter: Builders<BsonDocument>.Filter.Eq("Id", review.Id.ToString()));
            _sdb.SaveChanges();


            return true;

        }

        #endregion

        #region mongo
        private void MongoReviewSave(Review review)
        {

            ProductHelperService.MongoReviewSave(review, ref _sdb, ref _mdb);
          
        }

        internal void UpdateMongoReview(Review review)
        {
            _mdb.Reviews.DeleteMany(filter: new BsonDocument { { "Id", review.Id.ToString() } });

            this.MongoReviewSave(review);
        }

        
        internal BsonDocument GetMongoRateFilter(long userId, long reviewId)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "UserId", userId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "ReviewId", reviewId.ToString() } });


            return new BsonDocument { { "$and", mongoFilter } };

        }
        #endregion

    }
}
