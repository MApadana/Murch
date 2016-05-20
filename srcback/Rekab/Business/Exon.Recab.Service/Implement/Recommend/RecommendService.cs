using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Constant;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.Recommend;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Exon.Recab.Service.Implement.Recommend
{
    public class RecommendService
    {
        private SdbContext _sdb;

        private MdbContext _mdb;

        internal RecommendService(ref SdbContext sdb, ref MdbContext mdb)
        {
            _sdb = sdb;
            _mdb = mdb;
        }

        public RecommendService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
        }


        #region ADD
        public bool AddTageForFeatureValue(long featureValueId, string tag)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            if (_mdb.Tag.Find(filter: new BsonDocument { { "FeatureValueId", featureValue.Id.ToString() } }).Count() > 0)
                throw new RecabException((int)ExceptionType.FeatureValueAlreadyExist);

            List<FeatureValueAssign> FeatureValueAssigns = _sdb.FeatureValueAssignFeatureValueItems
                                                              .Where(fvi => fvi.FeatureValueId == featureValue.Id)
                                                              .Select(fvi => fvi.FeatureValueAssignId).Distinct()
                                                              .Join(_sdb.FeatureValueAssign,
                                                                     fvi => fvi,
                                                                     fva => fva.Id,
                                                                     (fvi, fva) => fva).ToList();

            BsonArray RevewArray = new BsonArray();

            BsonArray ArticleArray = new BsonArray();

            BsonArray TodayPriceArray = new BsonArray();

            foreach (var item in FeatureValueAssigns)
            {
                switch (item.EntityType)
                {
                    case EntityType.Article:
                        ArticleArray.Add(item.EntityId);
                        break;

                    case EntityType.Review:
                        RevewArray.Add(item.EntityId);
                        break;

                    case EntityType.TodayPrice:
                        TodayPriceArray.Add(item.EntityId);
                        break;
                    default:
                        break;
                }

            }


            List<Product> products = _sdb.ProductFeatureValueFeatureValueItems
                                          .Where(pfvi => pfvi.FeatureValueId == featureValueId)
                                          .Select(pfvi => pfvi.ProductFeatureValueId).Distinct()
                                          .Join(_sdb.ProductFeatureValues, pfvi => pfvi, pfv => pfv.Id, (pfvi, pfv) => pfv.Product).ToList();

            BsonArray ProductArray = new BsonArray();

            foreach (var item in products)
            {
                ProductArray.Add(item.Id);
            }

            _mdb.Tag.InsertOne(document: new BsonDocument { {"Title",tag },
                                                            {"FeatureValueId",featureValue.Id.ToString() },
                                                            {"CategoryId",featureValue.CategoryFeature.CategoryId.ToString() } ,
                                                            {"ProductItems",ProductArray } ,
                                                            {"ReviewItems",RevewArray },
                                                            {"ArticleItems",ArticleArray},
                                                            {"TodayPriceItems",TodayPriceArray},
                                                            });

            return true;
        }
        

        public bool EditTageForFeatureValue(long featureValueId, string tag)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            _mdb.Tag.DeleteMany(filter: new BsonDocument { { "FeatureValueId", featureValue.Id.ToString() } });

            return this.AddTageForFeatureValue(featureValueId, tag);

        }


        #endregion

        #region Search

        public string GetTageForFeatureValue(long featureValueId)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);


            var temp = _mdb.Tag.Find(filter: new BsonDocument { { "FeatureValueId", featureValue.Id.ToString() } });

            if (temp.Count() == 0)
                return "";

            Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(temp.FirstOrDefault());

            return data["Title"].ToString();


        }


        public List<BriefSearchViewModel> BriefSearch(string key)
        {
            //BriefSearchADS(key);
            //BriefSearchREV(key);
            //BriefSearchART(key);
            //Thread ADSThread = new Thread(BriefSearchADS);

            //ADSThread.Start(key);

            //Thread RVWThread = new Thread(BriefSearchREV);

            //RVWThread.Start(key);

            //Thread ARTThread = new Thread(BriefSearchART);

            //ARTThread.Start(key);

            //while (ThreadCount < 3)
            //{ }
            GC.Collect();

            return BriefSearchMongo(key);
        }

        public List<BriefSearchViewModel> BriefSearchMongo(string key)
        {
            List<BriefSearchViewModel> outPut = new List<BriefSearchViewModel>();
            if (key.Length >= 2)
            {
                List<Category> category = _sdb.Categoris.ToList();

                #region ADS
                BriefSearchViewModel modelAD = new BriefSearchViewModel();

                modelAD.type = BriefSearchType.آگهی;

                modelAD.title = BriefSearchType.آگهی.ToString();

                foreach (var item in category)
                {

                    BsonArray mongoFilter = new BsonArray();

                    mongoFilter.Add(new BsonDocument { { "CategoryId", item.Id.ToString() } });

                    mongoFilter.Add(new BsonDocument { { "$text", new BsonDocument { { "$search", key } } } });

                    BsonDocument match = new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };


                    BsonDocument unwind = new BsonDocument { { "$unwind", "$ProductItems" } };

                    BsonDocument group = new BsonDocument {{"$group",new BsonDocument{
                                                                                      { "_id", "$ProductItems" },
                                                                                      {"count",new BsonDocument{{ "$sum",1}}}
                                                                                     } }};

                    var pipeline = new[] { match, unwind, group };


                    List<object> result = _mdb.Tag.Aggregate<object>(pipeline).ToList();

                    if (result.Count > 0)
                        modelAD.items.Add(new BriefSearchItemViewModel { categoryId = item.Id, categoryTitle = item.Title, count = result.Count });

                }
                outPut.Add(modelAD);

                #endregion

                #region REV
                BriefSearchViewModel modelRV = new BriefSearchViewModel();

                modelRV.type = BriefSearchType.بررسی_فنی;

                modelRV.title = BriefSearchType.بررسی_فنی.ToString();

                foreach (var item in category)
                {

                    BsonArray mongoFilter = new BsonArray();

                    mongoFilter.Add(new BsonDocument { { "CategoryId", item.Id.ToString() } });

                    mongoFilter.Add(new BsonDocument { { "$text", new BsonDocument { { "$search", key } } } });

                    BsonDocument match = new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };


                    BsonDocument unwind = new BsonDocument { { "$unwind", "$ReviewItems" } };

                    BsonDocument group = new BsonDocument {{"$group",new BsonDocument{
                                                                                      { "_id", "$ReviewItems" },
                                                                                      {"count",new BsonDocument{{ "$sum",1}}}
                                                                                     } }};

                    var pipeline = new[] { match, unwind, group };


                    List<object> result = _mdb.Tag.Aggregate<object>(pipeline).ToList();

                    if (result.Count > 0)
                        modelRV.items.Add(new BriefSearchItemViewModel { categoryId = item.Id, categoryTitle = item.Title, count = result.Count });

                }
                outPut.Add(modelRV);
                #endregion

                #region ART
                BriefSearchViewModel modelAT = new BriefSearchViewModel();

                modelAT.type = BriefSearchType.مقاله;

                modelAT.title = "مقالات تخصصی";

                foreach (var item in category)
                {

                    BsonArray mongoFilter = new BsonArray();

                    mongoFilter.Add(new BsonDocument { { "CategoryId", item.Id.ToString() } });

                    mongoFilter.Add(new BsonDocument { { "$text", new BsonDocument { { "$search", key } } } });

                    BsonDocument match = new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };


                    BsonDocument unwind = new BsonDocument { { "$unwind", "$ArticleItems" } };

                    BsonDocument group = new BsonDocument {{"$group",new BsonDocument{
                                                                                      { "_id", "$ArticleItems" },
                                                                                      {"count",new BsonDocument{{ "$sum",1}}}
                                                                                     } }};

                    var pipeline = new[] { match, unwind, group };


                    List<object> result = _mdb.Tag.Aggregate<object>(pipeline).ToList();

                    if (result.Count > 0)
                        modelAT.items.Add(new BriefSearchItemViewModel { categoryId = item.Id, categoryTitle = item.Title, count = result.Count });

                }

                outPut.Add(modelAT);
                #endregion


                #region TO
                BriefSearchViewModel modelTo = new BriefSearchViewModel();

                modelTo.type = BriefSearchType.نرخ_روز;

                modelTo.title = "نرخ روز";

                foreach (var item in category)
                {

                    BsonArray mongoFilter = new BsonArray();

                    mongoFilter.Add(new BsonDocument { { "CategoryId", item.Id.ToString() } });

                    mongoFilter.Add(new BsonDocument { { "$text", new BsonDocument { { "$search", key } } } });

                    BsonDocument match = new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };


                    BsonDocument unwind = new BsonDocument { { "$unwind", "$TodayPriceItems" } };

                    BsonDocument group = new BsonDocument {{"$group",new BsonDocument{
                                                                                      { "_id", "$TodayPriceItems" },
                                                                                      {"count",new BsonDocument{{ "$sum",1}}}
                                                                                     } }};

                    var pipeline = new[] { match, unwind, group };


                    List<object> result = _mdb.Tag.Aggregate<object>(pipeline).ToList();

                    if (result.Count > 0)
                        modelTo.items.Add(new BriefSearchItemViewModel { categoryId = item.Id, categoryTitle = item.Title, count = result.Count });

                }

                outPut.Add(modelTo);
                #endregion
            }

            return outPut;
        }


        public HttpResponseMessage GetReletiveAds(long caregoryId, long entityId, int entityType, int size, int skip)
        {
            Category category = _sdb.Categoris.Find(caregoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            List<ReletiveCategoryFeature> ReletiveItems = new List<ReletiveCategoryFeature>();

            switch ((EntityType)entityType)
            {
                case EntityType.Article:

                    this.FindArticle(ref ReletiveItems, entityId);

                    break;

                case EntityType.Review:
                    this.FindReview(ref ReletiveItems, entityId);
                    break;

                case EntityType.TodayPrice:
                    this.FindTodayPrice(ref ReletiveItems, entityId);
                    break;

                case EntityType.Product:
                    this.FindProduct(ref ReletiveItems, entityId);
                    break;

            }

            int count = category.ReletiveCount;

            AdvertiseService _ProdoctService = new AdvertiseService(sdb: ref _sdb, mdb: ref _mdb);

            var outPut = new List<SearchResultItemViewModel>();

            while (outPut.Count < count && ReletiveItems.Count > 0)
            {
                long temp = 0;
                var temerory = outPut.Concat(_ProdoctService.SearchProduct(keyword: "",
                                                          categoryId: caregoryId,
                                                          sort : 0 ,
                                                          size: count,
                                                          page: 0,
                                                          type: 0,
                                                          Count: ref temp,
                                                          packageReletive: true,
                                                          filters: ReletiveItems.Select(ri => new CFProdoctFilterModel
                                                          {
                                                              CategoryFeatureId = ri.CategoryFeature.Id,
                                                              FeatureValueId = ri.FeatureValues.Select(fv => fv.Id).ToList(),
                                                              CustomValue = ""
                                                          }).ToList()
                                                         ).Where(i => i.advertiseId != entityId)).ToList();

                foreach (var item in temerory)
                {
                    if (!outPut.Any(o => o.advertiseId == item.advertiseId))
                        outPut.Add(item);

                }

                if (ReletiveItems.Count > 1)
                {
                    var deleteItem = ReletiveItems.OrderBy(r => r.CategoryFeature.RelativeADSOrder).FirstOrDefault();

                    if (deleteItem != null)
                        ReletiveItems.Remove(deleteItem);
                }
                else
                {
                    ReletiveItems = new List<ReletiveCategoryFeature>();
                }
            }

           return outPut.Take(count).GetHttpResponseWithCount(count);


        }


        #endregion

        #region private

        private void FindProduct(ref List<ReletiveCategoryFeature> ReletiveItems, long productId)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            var Assign = product.ProductFeatures.Where(pf => pf.CategoryFeature.AvailableInRelativeADS).ToList();

            foreach (var item in Assign)
            {
                ReletiveItems.Add(new ReletiveCategoryFeature
                {
                    CategoryFeature = item.CategoryFeature,
                    FeatureValues = item.ListFeatureValue.Select(fv => fv.FeatureValue).ToList()
                });
            }

            return;
        }

        private void FindTodayPrice(ref List<ReletiveCategoryFeature> ReletiveItems, long tpId)
        {
            TodayPriceConfig TodayPrice = _sdb.TodayPricesConfig.Find(tpId);

            if (TodayPrice == null)
                throw new RecabException((int)ExceptionType.TodayPriceNotFound);

            List<FeatureValueAssign> Assign = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.TodayPrice &&
                                                                                 fva.EntityId == TodayPrice.Id &&
                                                                                 fva.CategoryFeature.AvailableInRelativeADS).ToList();

            foreach (var item in Assign)
            {
                ReletiveItems.Add(new ReletiveCategoryFeature
                {
                    CategoryFeature = item.CategoryFeature,
                    FeatureValues = item.ListFeatureValue.Select(fv => fv.FeatureValue).ToList()
                });
            }

            return;
        }

        private void FindReview(ref List<ReletiveCategoryFeature> ReletiveItems, long reviewId)
        {
            Review review = _sdb.Reviews.Find(reviewId);

            if (review == null)
                throw new RecabException((int)ExceptionType.ReviewNotFound);

            List<FeatureValueAssign> Assign = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.Review &&
                                                                                 fva.EntityId == review.Id &&
                                                                                 fva.CategoryFeature.AvailableInRelativeADS).ToList();

            foreach (var item in Assign)
            {
                ReletiveItems.Add(new ReletiveCategoryFeature
                {
                    CategoryFeature = item.CategoryFeature,
                    FeatureValues = item.ListFeatureValue.Select(fv => fv.FeatureValue).ToList()
                });
            }

            return;
        }

        private void FindArticle(ref List<ReletiveCategoryFeature> ReletiveItems, long articleId)
        {
            Article article = _sdb.Articles.Find(articleId);

            if (article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);

            List<FeatureValueAssign> Assign = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.Article &&
                                                                                 fva.EntityId == article.Id &&
                                                                                 fva.CategoryFeature.AvailableInRelativeADS).ToList();

            foreach (var item in Assign)
            {
                ReletiveItems.Add(new ReletiveCategoryFeature
                {
                    CategoryFeature = item.CategoryFeature,
                    FeatureValues = item.ListFeatureValue.Select(fv => fv.FeatureValue).ToList()
                });
            }

            return;
        }

        #endregion

    }
}
