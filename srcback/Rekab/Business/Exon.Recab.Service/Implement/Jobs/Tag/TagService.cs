using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exon.Recab.Service.Implement.Jobs.Tag
{
    public class TagService
    {
        private readonly SdbContext _sdb;

        private readonly MdbContext _mdb;

        public TagService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
        }

        public void UpdateProductTage()
        {

            var products = _mdb.Products.Find(new BsonDocument()).ToList();

            var Tags = _mdb.Tag.Find(new BsonDocument()).ToList();

            foreach (var item in Tags)
            {
                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                _mdb.Tag.DeleteOne(filter: new BsonDocument { { "_id", ObjectId.Parse(data["_id"].ToString()) } });

                try
                {
                    this.AddTageForFeatureValue(featureValueId: Convert.ToInt64(data["FeatureValueId"].ToString()), tag: data["Title"].ToString());
                }
                catch (Exception e)
                {
                    throw;
                }
            }


            return;
        }

        private void AddTageForFeatureValue(long featureValueId, string tag)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
            {       
                return;
            }

            if (_mdb.Tag.Find(filter: new BsonDocument { { "FeatureValueId", featureValue.Id.ToString() } }).Count() > 0)
            {              
                return;
            }


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


            List<Domain.Entity.Product> products = _sdb.ProductFeatureValueFeatureValueItems
                                          .Where(pfvi => pfvi.FeatureValueId == featureValueId)
                                          .Select(pfvi => pfvi.ProductFeatureValueId).Distinct()
                                          .Join(_sdb.ProductFeatureValues, pfvi => pfvi, pfv => pfv.Id, (pfvi, pfv) => pfv.Product)
                                          .Where(p=>p.Status == ProdoctStatus.فعال && (p.DealershipId.HasValue ? p.Dealership.Status == DealershipStatus.فعال:true)).ToList();

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

            return;
        }

    }
}
