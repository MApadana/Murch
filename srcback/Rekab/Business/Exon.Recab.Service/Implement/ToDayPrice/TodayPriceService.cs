using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.TodayPriceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Domain.Constant.Media;
using MongoDB.Bson;
using System.Web.Script.Serialization;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Exon.Recab.Service.Implement.ReView;
using Exon.Recab.Service.Model.ReviewModel;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Implement.Helper;

namespace Exon.Recab.Service.Implement.ToDayPrice
{
    public class TodayPriceService
    {
        private SdbContext _sdb;
        private MdbContext _mdb;


        public TodayPriceService()
        {
            this._sdb = new SdbContext();
            this._mdb = new MdbContext();

        }

        #region add
        public bool AddNewTodayPriceConfig(long userId,
                                            long categoryId,
                                            string sellOption,
                                            string persianLastUpdateDate,
                                            long price,
                                            long dealershipPrice,
                                            List<SelectItemModel> todayPriceItem)

        {
            #region Validation 

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                                 cf.AvailableTodayPrice).ToList();

            string Title = "";
            List<SelectItemModel> ConfirmItems = todayPriceItem.GetConfirmedItems(entityCategoryFeature: InsertList,
                                                                                  reqiredCategoryFeature: InsertList.Where(cf => cf.RequierdInTPInsert).ToList(),
                                                                                  titleItems: InsertList.Where(cf => cf.AvailableInRVTitle).ToList(),
                                                                                  title: ref Title);
            #endregion

            #region Header

            TodayPriceConfig newTodayPrice = new TodayPriceConfig
            {
                Title = Title,
                Price = price,
                Tolerance = 0.0,
                CategoryId = category.Id,
                VisitCount = 0,
                LastUpdateDate = persianLastUpdateDate.PersianToGregorian(),
                SellOption = sellOption ?? "",
                CreateTime = DateTime.UtcNow,
                UserId = user.Id,
                DealershipPrice = dealershipPrice
            };

            _sdb.TodayPricesConfig.Add(newTodayPrice);
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
                        EntityType = EntityType.TodayPrice,
                        CustomValue = "",
                        EntityId = newTodayPrice.Id,
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
                        EntityType = EntityType.TodayPrice,
                        CustomValue = item.CustomValue,
                        EntityId = newTodayPrice.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                }

                _sdb.FeatureValueAssign.Add(FeatureValueAssign);
            }

            _sdb.SaveChanges();

            #endregion

            MongoTodayPriceSave(newTodayPrice);

            return true;
        }

        public bool AddTodayPriceOption(long categoryId, string title)
        {

            Category category = _sdb.Categoris.Find(categoryId);


            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            _sdb.TodayPriceOption.Add(new TodayPriceOption { Title = title ?? "", CategoryId = category.Id });
            _sdb.SaveChanges();
            return true;


        }
        #endregion

        #region search
        public List<TodayPriceSearchResultItemViewModel> TodayPriceSearch(List<TodayPriceFilterModel> filter,
                                                              string keyword,
                                                              long categoryId,
                                                              ref long count,
                                                              int size = 1,
                                                              int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<CategoryFeature> categoryFeature = _sdb.CategoryFeatures.Where(cf => cf.AvailableTodayPrice && cf.CategoryId == categoryId)
                                                         .Take(2).OrderBy(cf => cf.OrderId).ToList();

            if (categoryFeature.Count != 2)
                throw new RecabException("config error system have no 2 element for TodayPrice search");

            List<TodayPriceFilterModelWithCategory> filterCategory = new List<TodayPriceFilterModelWithCategory>();

            foreach (var item in filter)
            {
                if (categoryFeature.Any(cf => cf.Id == item.CategoryFeatureId))
                {
                    if (!categoryFeature.First(cf => cf.Id == item.CategoryFeatureId).FeatureValueList.Any(fv => fv.Id == item.FeatureValueId))
                    {
                        throw new RecabException((int)ExceptionType.FeatureValueInvalid);
                    }
                    else {

                        filterCategory.Add(new TodayPriceFilterModelWithCategory
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


            BsonDocument match = this.GetMongoFilterSearch(categoryId, filterCategory , keyword);

            var projection = Builders<BsonDocument>.Projection.Exclude("_id");

            var MongoResult = _mdb.TodayPrice.Find(match).Project(projection).ToList();

            List<TodayPriceSearchResultItemViewModel> model = new List<TodayPriceSearchResultItemViewModel>();

            foreach (var resultItem in MongoResult)
            {

                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(resultItem);

                FeatureValue[] outputItem = new FeatureValue[2];

                for (int i = 0; i < 2; i++)
                {
                    object newObject = new object();
                    data.TryGetValue("CF_" + categoryFeature[i].Id.ToString(), out newObject);

                    if (newObject != null)
                    {
                        if (!categoryFeature[i].HasMultiSelectValue && !categoryFeature[i].HasCustomValue)
                        {
                            FeatureValue featureValue = categoryFeature[i].FeatureValueList.Find(fv => fv.Id.ToString() == newObject as string);
                            outputItem[i] = featureValue;
                        }

                    }
                }

                long TodayPriceId = 0;
                long.TryParse(data["Id"].ToString(), out TodayPriceId);

                if (TodayPriceId != 0)
                {

                    List<FeatureValueAssign> categoryFeatureDepend = _sdb.FeatureValueAssign.Where(fva => fva.CategoryFeature.AvailableTodayPrice &&
                                                                                                          fva.CategoryFeature.CategoryId == categoryId &&
                                                                                                          fva.EntityType == EntityType.TodayPrice && fva.EntityId == TodayPriceId)
                                                                                                .OrderBy(cf => cf.CategoryFeature.OrderId).ToList();




                    TodayPriceSearchResultItemViewModel temp = new TodayPriceSearchResultItemViewModel
                    {
                        imageUrl = "",
                        todayPriceId = TodayPriceId,
                        tolerance = data["Tolerance"].ToString(),
                        sellOption = data["SellOption"].ToString(),
                        price = data["Price"].ToString().ToStringToman(),
                        title = data["Title"].ToString(),
                        dealershipPrice = data["DealershipPrice"] != null ? data["DealershipPrice"].ToString().ToStringToman() : "",
                        relatedFeatureValues = categoryFeatureDepend.Select(cf => new TodayPriceCategoryFeatureEditViewModel
                        {
                            categoryFeatureId = cf.CategoryFeatureId,
                            title = cf.CategoryFeature.Title,
                            featureValues = cf.ListFeatureValue.Select(fv => new TodayPriceFeatureValueEditViewModel
                            {
                                title = fv.FeatureValue.Title,
                                featureValueId = fv.FeatureValueId
                            }).ToList()
                        }).ToList()

                    };



                    model.Add(temp);
                }
            }
            count = model.Count();

            return model.OrderBy(m => m.todayPriceId).Skip(size * skip).Take(size).ToList();

        }

        public TodayPriceGroupByResultItemViewModel TodayPriceSearchGroupBy(List<TodayPriceFilterModel> filter,
                                                                            string keyword,
                                                                            long categoryId,
                                                                            ref long count,
                                                                            int size = 1,
                                                                            int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            CategoryFeature Brand = _sdb.CategoryFeatures.Where(cf => cf.AvailableTodayPrice && cf.CategoryId == categoryId)
                                                         .OrderBy(cf => cf.OrderId)
                                                         .FirstOrDefault();
            if (Brand == null)
                throw new RecabException("config error system have no 1 element for TodayPrice search");


            var brandFilter = filter.FirstOrDefault(f => f.CategoryFeatureId == Brand.Id);

            if (brandFilter == null)
                throw new RecabException((int)ExceptionType.TodayPriceWrongFilter);

            if (!Brand.FeatureValueList.Any(k => k.Id == brandFilter.FeatureValueId))
                throw new RecabException((int)ExceptionType.TodayPriceWrongFilter);

            var categoryFeature = _sdb.CategoryFeatures.Where(cf => cf.AvailableTodayPrice && cf.CategoryId == categoryId).ToList();


            List<TodayPriceFilterModelWithCategory> filterCategory = new List<TodayPriceFilterModelWithCategory>();

            foreach (var item in filter)
            {
                if (categoryFeature.Any(cf => cf.Id == item.CategoryFeatureId))
                {
                    if (!categoryFeature.First(cf => cf.Id == item.CategoryFeatureId).FeatureValueList.Any(fv => fv.Id == item.FeatureValueId))
                    {
                        throw new RecabException((int)ExceptionType.FeatureValueNotFound);
                    }
                    else {

                        filterCategory.Add(new TodayPriceFilterModelWithCategory
                        {
                            CategoryFeature = categoryFeature.First(cf => cf.Id == item.CategoryFeatureId),
                            Filter = item
                        });
                    }
                }
                else
                    throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);
            }

            JavaScriptSerializer scr = new JavaScriptSerializer();

            BsonDocument match = this.GetMongoFilterSearch(categoryId, filterCategory , keyword);

            var projection = Builders<BsonDocument>.Projection.Exclude("_id");

            var MongoResult = _mdb.TodayPrice.Find(match).Project(projection).ToList();

            List<TodayPriceSearchResultItemViewModel> model = new List<TodayPriceSearchResultItemViewModel>();

            foreach (var resultItem in MongoResult)
            {

                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(resultItem);

                long TodayPriceId = 0;
                long.TryParse(data["Id"].ToString(), out TodayPriceId);


                if (TodayPriceId != 0)
                {

                    List<FeatureValueAssign> categoryFeatureDepend = _sdb.FeatureValueAssign.Where(fva => fva.CategoryFeature.AvailableTodayPrice &&
                                                                                                          fva.CategoryFeature.CategoryId == categoryId &&
                                                                                                          fva.EntityType == EntityType.TodayPrice && fva.EntityId == TodayPriceId)
                                                                                                .OrderBy(cf => cf.CategoryFeature.OrderId).ToList();


                    TodayPriceSearchResultItemViewModel temp = new TodayPriceSearchResultItemViewModel
                    {
                        todayPriceId = TodayPriceId,
                        tolerance = new string(data["Tolerance"].ToString().Take(5).ToArray()).Replace("/","."),
                        sellOption = data["SellOption"].ToString(),
                        price = data["Price"].ToString().ToStringToman(),
                        dealershipPrice = data["DealershipPrice"] != null ? data["DealershipPrice"].ToString().ToStringToman() : "",
                        title = data["Title"].ToString(),
                        imageUrl = "",
                        relatedFeatureValues = categoryFeatureDepend.Select(cf => new TodayPriceCategoryFeatureEditViewModel
                        {
                            categoryFeatureId = cf.CategoryFeatureId,
                            title = cf.CategoryFeature.Title,
                            featureValues = cf.ListFeatureValue.Select(fv => new TodayPriceFeatureValueEditViewModel
                            {
                                title = fv.FeatureValue.Title,
                                featureValueId = fv.FeatureValueId
                            }).ToList()
                        }).ToList()

                    };

                    model.Add(temp);
                }
            }

            count = model.Count;
          return new TodayPriceGroupByResultItemViewModel
            {
                featureValueId = brandFilter.FeatureValueId,
                featureValueTitle = Brand.FeatureValueList.First(fv => fv.Id == brandFilter.FeatureValueId).Title,
                priceItems = model.OrderBy(m => m.todayPriceId).Skip(size * skip).Take(size).ToList(),
                logoUrl = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == brandFilter.FeatureValueId) != null ?
                           _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == brandFilter.FeatureValueId).MediaURL : "",
                categoryId = Brand.CategoryId,
                categoryTitle = Brand.Category.Title
            };





        }


        public TodayPriceCategoryGroupByViewModel TodayPriceCategoryGroupBy(long categoryId)

        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            CategoryFeature Brand = _sdb.CategoryFeatures.Where(cf => cf.AvailableTodayPrice && cf.CategoryId == categoryId).OrderBy(cf => cf.OrderId).FirstOrDefault();

            if (Brand == null)
                throw new RecabException("config error system have no 2 element for TodayPrice search");

            JavaScriptSerializer scr = new JavaScriptSerializer();

            List<TodayPriceGroupByResultItemViewModel> result = new List<TodayPriceGroupByResultItemViewModel>();

            foreach (var item in Brand.FeatureValueList)
            {
                TodayPriceFilterModelWithCategory filterCategory = new TodayPriceFilterModelWithCategory
                {
                    CategoryFeature = Brand,
                    Filter = new TodayPriceFilterModel { CategoryFeatureId = item.CategoryFeatureId, FeatureValueId = item.Id }
                };

                BsonDocument match = this.GetMongoFilterSearchGroupBy(categoryId, filterCategory);

                var projection = Builders<BsonDocument>.Projection.Exclude("_id");

                var MongoResult = _mdb.TodayPrice.Find(match).Project(projection).ToList();

                List<TodayPriceSearchResultItemViewModel> model = new List<TodayPriceSearchResultItemViewModel>();
                foreach (var resultItem in MongoResult)
                {

                    Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(resultItem);

                    long TodayPriceId = 0;
                    long.TryParse(data["Id"].ToString(), out TodayPriceId);


                    TodayPriceSearchResultItemViewModel temp = new TodayPriceSearchResultItemViewModel
                    {
                        todayPriceId = TodayPriceId,
                        tolerance = data["Tolerance"].ToString(),
                        sellOption = data["SellOption"].ToString(),
                        price = Convert.ToInt64(data["Price"].ToString()).ToString("##,###"),
                        title = data["Title"].ToString(),
                    };


                    model.Add(temp);
                }
                if (model.Count > 0)

                    result.Add(new TodayPriceGroupByResultItemViewModel
                    {
                        featureValueId = item.Id,
                        featureValueTitle = item.Title,
                        logoUrl = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == item.Id) != null ? _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == item.Id).MediaURL : ""
                    });
            }


            return new TodayPriceCategoryGroupByViewModel { items = result, categoryFeatureId = Brand.Id };

        }


        public List<TodayPriceOptionViewModel> TodayOptionList(long categoryId,
                                                              ref long count,
                                                              int size = 1,
                                                              int skip = 0)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            var ListTodayPrice = _sdb.TodayPriceOption.Where(tpo => tpo.CategoryId == categoryId).ToList();

            count = ListTodayPrice.Count();

            if (count == 0)
                return new List<TodayPriceOptionViewModel>();

            return ListTodayPrice.OrderBy(tpo => tpo.Id).Skip(size * skip).Take(size).Select(tpo => new TodayPriceOptionViewModel
            {
                title = tpo.Title,
                Id = tpo.Id
            }).ToList();

        }

        public List<HistoryDetailViewModel> GetAllTodayPriceHistory(long todayPriceId)
        {
            TodayPriceConfig todayPrice = _sdb.TodayPricesConfig.Find(todayPriceId);

            if (todayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            var history = _mdb.TodayPriceHistory.Find(new BsonDocument { { "TodayPriceId", todayPrice.Id.ToString() } }).ToList();

            List<HistoryDetailViewModel> model = new List<HistoryDetailViewModel>();
            foreach (var item in history)
            {

                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                model.Add(new HistoryDetailViewModel
                {
                    historyId = data["_id"].ToString(),
                    price = data["Price"].ToString(),
                    persianDate = data["DateTime"].ToString()
                });
            }

            return model;
        }


        public TodayPriceChartModel GetChartDataForTodayPrice(long todayPriceId)
        {
            TodayPriceConfig todayPrice = _sdb.TodayPricesConfig.Find(todayPriceId);

            if (todayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            var ReviewItem = _sdb.FeatureValueAssign.Where(cf => cf.EntityId == todayPrice.Id && cf.EntityType == EntityType.TodayPrice);

            long reviewId = 0;
            if (ReviewItem.Count() > 0)
            {
                var filter = ReviewItem.Select(R => new TodayPriceFilterModelWithCategory
                {
                    CategoryFeature = R.CategoryFeature,
                    Filter = new TodayPriceFilterModel
                    {
                        CategoryFeatureId = R.CategoryFeatureId,
                        FeatureValueId = R.ListFeatureValue.FirstOrDefault() == null ? 0 : R.ListFeatureValue.FirstOrDefault().FeatureValueId
                    }
                }).ToList();


                var match = GetMongoFilterSearch(categoryId: todayPrice.CategoryId, filter: filter ,keyword: "");

                JavaScriptSerializer scr = new JavaScriptSerializer();


                var projection = Builders<BsonDocument>.Projection.Exclude("_id");

                var MongoResult = _mdb.Reviews.Find(match).Project(projection).ToList();

                if (MongoResult.Count > 0)
                {
                    Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(MongoResult.First());

                    reviewId = Convert.ToInt64(data["Id"]);
                }
            }


            var history = _mdb.TodayPriceHistory.Find(new BsonDocument { { "TodayPriceId", todayPrice.Id.ToString() } }).ToList();

            ReviewService _ReviewService = new ReviewService(sdb: ref _sdb, mdb: ref _mdb);

            TodayPriceChartModel model = new TodayPriceChartModel();
            model.todayPriceId = todayPriceId;
            model.tolerance = Math.Round(todayPrice.Tolerance , digits :2).ToString().Replace("/",".");
            model.price = todayPrice.Price.ToString("##,###");
            model.review = reviewId == 0 ? new ReviewViewModel() : _ReviewService.GetSingleReview(reviewId: reviewId, userId: 0);

            foreach (var item in history)
            {

                model.priceItems.Add(item.GetElement("Price").Value.ToString());
                model.dateItems.Add(item.GetElement("DateTime").Value.ToString());
            }

            return model;

        }

        #endregion

        #region Edit

        public bool UpdatePrice(long todayPriceId, long price, long dealershipPrice, string persianUpdateTime)
        {

            TodayPriceConfig todayPrice = _sdb.TodayPricesConfig.Find(todayPriceId);

            if (todayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            todayPrice.Tolerance = (Double)(((Double)price / (Double)todayPrice.Price) - 1) * 100;

            todayPrice.Price = price;

            todayPrice.LastUpdateDate = persianUpdateTime.PersianToGregorian();

            todayPrice.DealershipPrice = dealershipPrice;

            _sdb.SaveChanges();

            _mdb.TodayPriceHistory.InsertOne(document: new BsonDocument { {"TodayPriceId", todayPrice.Id.ToString() },
                                                                          {"DateTime", persianUpdateTime } ,
                                                                          {"Price", price.ToString() },
                                                                          {"DealershipPrice", dealershipPrice.ToString() }
                                                                            });



            var filter = Builders<BsonDocument>.Filter.Eq("Id", todayPrice.Id.ToString());

            var update = Builders<BsonDocument>.Update
                .Set("Tolerance", ((float)todayPrice.Tolerance).ToString().Replace("/", "."))
                .Set("Price", price.ToString())
                .Set("LastUpdateDate", todayPrice.LastUpdateDate.ToShortDateString())
                .Set("DealershipPrice", dealershipPrice.ToString())
                .CurrentDate("lastModified");

            _mdb.TodayPrice.UpdateMany(filter, update);


            return true;
        }

        public bool EditTodayPriceOption(long todayPriceOptionId, long categoryId, string title)
        {

            TodayPriceOption todayPriceOption = _sdb.TodayPriceOption.Find(todayPriceOptionId);

            if (todayPriceOption == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            todayPriceOption.Title = title ?? "";
            todayPriceOption.CategoryId = category.Id;

            _sdb.SaveChanges();


            return true;


        }

        public AdminTodayPriceViewModel InitEditTodayPrice(long todayPriceId)
        {
            TodayPriceConfig todayPrice = _sdb.TodayPricesConfig.Find(todayPriceId);

            if (todayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            List<FeatureValueAssign> AssingeFeature = _sdb.FeatureValueAssign.Where(cf => cf.EntityId == todayPrice.Id &&
                                                                                        cf.EntityType == EntityType.TodayPrice &&
                                                                                        cf.CategoryFeature.RequiredInRVInsert).ToList();

            List<string> media = _sdb.Media.Where(m => m.EntityId == todayPrice.Id && EntityType.TodayPrice == m.EntityType)
                                         .OrderBy(m => m.Order)
                                         .Select(m => m.MediaURL).ToList();


            return new AdminTodayPriceViewModel
            {
                todayPriceId = todayPrice.Id,
                title = todayPrice.Title,
                price = todayPrice.Price,
                sellOption = todayPrice.SellOption,
                dealershipPrice = todayPrice.DealershipPrice,
                lastDate = todayPrice.LastUpdateDate.UTCToPersianDateShort(),
                categoryFeatures = AssingeFeature.Select(cf => new TodayPriceCategoryFeatureEditViewModel
                {
                    title = cf.CategoryFeature.Title,
                    categoryFeatureId = cf.CategoryFeatureId,
                    featureValues = cf.ListFeatureValue.Select(fv => new TodayPriceFeatureValueEditViewModel
                    {
                        featureValueId = fv.FeatureValueId,
                        title = fv.FeatureValue.Title
                    }).ToList()
                }).ToList()
            };

        }

        #endregion

        #region delete
        public bool DeleteTodayPrice(long todayPriceId)
        {
            TodayPriceConfig todayPrice = _sdb.TodayPricesConfig.Find(todayPriceId);
            if (todayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);



            _sdb.TodayPricesConfig.Remove(todayPrice);

            _sdb.SaveChanges();

            _mdb.TodayPrice.DeleteMany(Builders<BsonDocument>.Filter.Eq("Id", todayPrice.Id.ToString()));

            return true;
        }

        public bool DeleteTodayPriceOption(long todayPriceOptionId)
        {

            TodayPriceOption todayPriceOption = _sdb.TodayPriceOption.Find(todayPriceOptionId);

            if (todayPriceOption == null)
                throw new RecabException((int)ExceptionType.TodayPriceOptionNotFound);

            _sdb.TodayPriceOption.Remove(todayPriceOption);

            _sdb.SaveChanges();

            return true;

        }

        public bool DeletePriceHistory(string historyId)
        {
            long count = 0;

            var temp = _mdb.TodayPriceHistory.DeleteMany(filter: Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(historyId)));

            count = temp.DeletedCount;


            if (count == 0)
                throw new RecabException((int)ExceptionType.TodayPriceHistoryNotFound);

            return true;

        }

        #endregion

        #region mongo

        private void MongoTodayPriceSave(object obj)
        {

            TodayPriceConfig todayPrice = obj as TodayPriceConfig;

            BsonDocument newMongotodayPrice = new BsonDocument {

                { "Id" , todayPrice.Id.ToString() },
                { "UserId",todayPrice.UserId.ToString() },
                { "CategoryId", todayPrice.CategoryId.ToString() },
                { "Title" , todayPrice.Title },
                { "SellOption" , todayPrice.SellOption},
                { "Tolerance" , todayPrice.Tolerance.ToString().Replace("/",".")},
                { "Price" , todayPrice.Price.ToString()},
                { "DealershipPrice" , todayPrice.DealershipPrice.ToString()},
                { "LastUpdateDate", todayPrice.LastUpdateDate.ToShortDateString() },
                { "VisitCount", todayPrice.VisitCount.ToString() } ,
                { "CreateTime" , todayPrice.CreateTime.ToShortDateString()}
            };


            List<FeatureValueAssign> FeatureValueAssigns = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.TodayPrice &&
                                                                                                  fva.EntityId == todayPrice.Id).ToList();


            foreach (FeatureValueAssign item in FeatureValueAssigns)
            {
                if (item.CategoryFeature.HasCustomValue)
                {
                    newMongotodayPrice.Add("CF_" + item.CategoryFeatureId.ToString(), "CUM_" + item.CustomValue ?? "");
                }
                else
                {
                    if (item.CategoryFeature.HasMultiSelectValue)
                    {
                        BsonArray bArray = new BsonArray();

                        var listId = item.ListFeatureValue.Select(fv => fv.FeatureValueId).ToList();
                        foreach (var fv in listId)
                        {
                            bArray.Add(fv);
                        }

                        newMongotodayPrice.Add("CF_" + item.CategoryFeatureId.ToString(), bArray);
                    }
                    else
                    {
                        newMongotodayPrice.Add("CF_" + item.CategoryFeatureId.ToString(), item.ListFeatureValue.FirstOrDefault() != null ? item.ListFeatureValue.FirstOrDefault().FeatureValueId.ToString() : "0");
                    }
                }
            }


            _mdb.TodayPrice.InsertOneAsync(newMongotodayPrice);

        }

        private BsonDocument GetMongoFilterSearch(long categoryId, List<TodayPriceFilterModelWithCategory> filter ,string keyword)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });

            foreach (var item in filter)
            {
                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.ToString() } });

            }

            if ( keyword != null &&  keyword.Length >= 2)
            {

                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.TodayPrice, keyword: keyword, categoryId: categoryId, sdb: ref _sdb, mdb: ref _mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        private BsonDocument GetMongoFilterSearchGroupBy(long categoryId, TodayPriceFilterModelWithCategory filter)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });


            mongoFilter.Add(new BsonDocument { { "CF_" + filter.CategoryFeature.Id.ToString(), filter.Filter.FeatureValueId.ToString() } });

            return new BsonDocument { { "$and", mongoFilter } };

        }


        #endregion
    }
}
