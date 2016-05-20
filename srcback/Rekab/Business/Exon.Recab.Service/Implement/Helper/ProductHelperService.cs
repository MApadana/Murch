using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Constant;
using Exon.Recab.Service.Implement.Email;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.ReviewModel;
using Exon.Recab.Service.Model.TodayPriceModel;
using Exon.Recab.Service.Resource;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Exon.Recab.Service.Implement.Helper
{
    public class ProductHelperService
    {
        #region Advertise

        #region Filter
        internal static BsonDocument GetMongoFilterAggregate(long categoryId,
                                                      string keyword,
                                                      List<FilterAndCategoryFeatureModel> filter,
                                                      ref BsonDocument Location,
                                                      ref SdbContext sdb,
                                                      ref MdbContext mdb
                                                        )
        {

            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });
            mongoFilter.Add(new BsonDocument { { "DealershipStatus", ((int)DealershipStatus.فعال).ToString() } });

            if (keyword.Length >= 2)
            {
                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.Product, keyword: keyword, categoryId: categoryId, sdb: ref sdb, mdb: ref mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            foreach (var item in filter)
            {
                #region customValue
                if (item.Filter.CustomValue != "")
                {
                    if (item.CategoryFeature.IsMap)
                    {
                        string[] temp = item.Filter.CustomValue.Split(',');

                        if (temp.Count() == 3)
                        {
                            double lng = 0;
                            double lat = 0;
                            int distanc = 0;

                            if (double.TryParse(temp[0], out lat) && double.TryParse(temp[1], out lng) && int.TryParse(temp[2], out distanc))
                            {
                                BsonArray Coordinatearray = new BsonArray();

                                Coordinatearray.Add(lat);
                                Coordinatearray.Add(lng);

                                Location = new BsonDocument { { "$geoNear", new BsonDocument { { "near", new BsonDocument {{ "type" , "Point" },{"coordinates" , Coordinatearray}}},
                                                                                                     { "maxDistance",distanc  },
                                                                                                     {"distanceField", "Dis" },
                                                                                                     {"spherical", true },
                                                                                                     { "query",new BsonDocument { { "type", "public" } } } ,
                                                                                                     { "num", 50 },
                                                                                                     {"includeLocs", "dist.location" }
                                                                                             }} };

                            }
                        }
                    }
                    else
                    {

                        string[] temp = item.Filter.CustomValue.Split(',');

                        if (temp.Count() == 2)
                        {
                            long namericValue = 0;

                            var renge = new BsonDocument();

                            if (temp[0] != null && temp[0] != "" && long.TryParse(temp[0], out namericValue))
                                renge.Add(name: "$gte", value: namericValue);

                            if (temp[1] != null && temp[1] != "" && long.TryParse(temp[1], out namericValue))
                            {
                                renge.Add(name: "$lte", value: namericValue);
                            }
                            mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), renge } });
                        }
                    }
                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(fv); }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }


                    }
                    else
                    {

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion


            }

            return new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };

        }

        internal static BsonDocument GetMongoFilterLocaton(long categoryId,
                                              string keyword,
                                              List<FilterAndCategoryFeatureModel> filter,
                                              double lat,
                                              double lng,
                                              int distance,
                                              ref BsonDocument Location,
                                              ref SdbContext sdb,
                                              ref MdbContext mdb
                                                )
        {
            BsonArray Coordinatearray = new BsonArray();

            Coordinatearray.Add(lat);
            Coordinatearray.Add(lng);

            Location = new BsonDocument { { "$geoNear", new BsonDocument { { "near", new BsonDocument {{ "type" , "Point" },{ "coordinates" , Coordinatearray}}},
                                                                           { "maxDistance",distance  },
                                                                           { "distanceField", "Dis" },
                                                                           { "spherical", true }
                                                                          }
                                            } };

            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });
            mongoFilter.Add(new BsonDocument { { "DealershipStatus", ((int)DealershipStatus.فعال).ToString() } });


            if (keyword.Length >= 2)
            {
                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.Product, keyword: keyword, categoryId: categoryId, sdb: ref sdb, mdb: ref mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            foreach (var item in filter)
            {
                #region customValue
                if (item.Filter.CustomValue != "")
                {

                    string[] temp = item.Filter.CustomValue.Split(',');

                    if (temp.Count() == 2)
                    {
                        long namericValue = 0;

                        var renge = new BsonDocument();

                        if (temp[0] != null && temp[0] != "" && long.TryParse(temp[0], out namericValue))
                            renge.Add(name: "$gte", value: namericValue);

                        if (temp[1] != null && temp[1] != "" && long.TryParse(temp[1], out namericValue))
                        {
                            renge.Add(name: "$lte", value: namericValue);
                        }
                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), renge } });
                    }

                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(fv); }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }


                    }
                    else
                    {

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion


            }

            return new BsonDocument { { "$match", new BsonDocument { { "$and", mongoFilter } } } };

        }



        internal static BsonDocument GetMongoFilterSearch(long categoryId,
                                                  long userId,
                                                  bool visited,
                                                  long alertId,
                                                  string keyword,
                                                  List<FilterAndCategoryFeatureModel> filter,
                                                  ref MdbContext mdb,
                                                  ref SdbContext sdb)
        {
            BsonArray mongoFilter = new BsonArray();

            if (userId != 0)
            {
                BsonArray arrayItem = new BsonArray();

                List<long> temp = UserAlertVisit(userId, alertId, ref mdb);

                if (temp.Count > 0)
                {
                    foreach (var fv in temp)
                    { arrayItem.Add(fv.ToString()); }

                    if (visited)
                    { mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } }); }
                    else
                    { mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$nin", arrayItem } } } }); }
                }
            }


            if (keyword.Length >= 2)
            {

                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.Product, keyword: keyword, categoryId: categoryId, sdb: ref sdb, mdb: ref mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });
            mongoFilter.Add(new BsonDocument { { "DealershipStatus", ((int)DealershipStatus.فعال).ToString() } });

            foreach (var item in filter)
            {
                #region customValue
                if (item.Filter.CustomValue != "")
                {
                    string[] temp = item.Filter.CustomValue.Split(',');
                    if (temp.Count() == 2)
                    {
                        long namericValue = 0;

                        var renge = new BsonDocument();

                        if (temp[0] != null && temp[0] != "" && long.TryParse(temp[0], out namericValue))
                            renge.Add(name: "$gte", value: namericValue);

                        if (temp[1] != null && temp[1] != "" && long.TryParse(temp[1], out namericValue))
                        {
                            renge.Add(name: "$lte", value: namericValue);
                        }

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), renge } });
                    }
                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(fv); }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }


                    }
                    else
                    {

                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }


        internal static BsonDocument GetMongoFilterSearch(long categoryId,
                                                        string keyword,
                                                        List<FilterAndCategoryFeatureModel> filter,
                                                        bool packageReletive,
                                                        ref SdbContext sdb,
                                                        ref MdbContext mdb)
        {

            BsonArray mongoFilter = new BsonArray();

            List<CategoryFeature> searchText = sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId && cf.AvailableInADTextSearch).ToList();

            if (keyword.Length >= 2)
            {

                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.Product, keyword: keyword, categoryId: categoryId, sdb: ref sdb, mdb: ref mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });
            mongoFilter.Add(new BsonDocument { { "DealershipStatus", ((int)DealershipStatus.فعال).ToString() } });


            if (packageReletive)
            {
                PackageType GoldPackage = sdb.PackageTypes.FirstOrDefault(pt => pt.Title == PackageConfig.Gold);

                if (GoldPackage != null)
                {
                    List<long> cpptIds = sdb.CategoryPurchasePackageTypes.Where(cppt => cppt.PackageTypeId == GoldPackage.Id).Select(cppt => cppt.Id).ToList();

                    BsonArray arrayItem = new BsonArray();

                    foreach (var id in cpptIds)
                    {
                        arrayItem.Add(id.ToString());
                    }

                    mongoFilter.Add(new BsonDocument { { "CPPTId", new BsonDocument { { "$in", arrayItem } } } });

                }

            }

            foreach (var item in filter)
            {
                #region customValue

                if (item.Filter.CustomValue != "")
                {
                    string[] temp = item.Filter.CustomValue.Split(',');

                    if (temp.Count() == 2)
                    {
                        long namericValue = 0;

                        var renge = new BsonDocument();

                        if (temp[0] != null && temp[0] != "" && long.TryParse(temp[0], out namericValue))
                            renge.Add(name: "$gte", value: namericValue);

                        if (temp[1] != null && temp[1] != "" && long.TryParse(temp[1], out namericValue))
                        {
                            renge.Add(name: "$lte", value: namericValue);
                        }


                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), renge } });
                    }
                }
                #endregion

                #region Multi
                else
                {

                    if (item.CategoryFeature.AvailableSearchMultiSelect)
                    {
                        if (item.CategoryFeature.HasMultiSelectValue)//in insert multi
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            {
                                arrayItem.Add(fv);
                            }

                            mongoFilter.Add(new BsonDocument {
                            {
                                    "CF_" + item.CategoryFeature.Id.ToString(),new BsonDocument { { "$in", arrayItem } }
                            }});
                        }
                        else //single in insert but multi in view
                        {
                            BsonArray arrayItem = new BsonArray();

                            foreach (var fv in item.Filter.FeatureValueId)
                            { arrayItem.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), fv.ToString() } }); }

                            if (arrayItem.Count == 1)
                            {
                                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                            }
                            else
                            {
                                mongoFilter.Add(new BsonDocument { { "$or", arrayItem } });
                            }
                        }
                    }
                    else
                    {
                        mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.First().ToString() } });
                    }

                }
                #endregion

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        internal static BsonDocument GetMongoFilterSearch(long categoryId, List<TodayPriceFilterModelWithCategory> filter, bool ads = true)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            if (ads)
            {
                mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });
                mongoFilter.Add(new BsonDocument { { "DealershipStatus", ((int)DealershipStatus.فعال).ToString() } });
            }
            foreach (var item in filter)
            {
                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.ToString() } });

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        internal static List<FilterAndCategoryFeatureModel> ApplyFilter(long categoryId, List<CFProdoctFilterModel> filters, ref SdbContext sdb)
        {
            List<FilterAndCategoryFeatureModel> AppliedFilter = new List<FilterAndCategoryFeatureModel>();

            List<FilterAndCategoryFeatureModel> JoinCategoryFeatureFilter =
                                filters.Join(sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                               cf.AvailableInADS &&
                                                                               cf.AvailableInSearchBox),
                                              f => f.CategoryFeatureId,
                                              cf => cf.Id,
                                              (f, cf) => new FilterAndCategoryFeatureModel
                                              {
                                                  Filter = f,
                                                  CategoryFeature = cf
                                              })
                                              .OrderBy(af => af.CategoryFeature.ParentList.Count())
                                              .ToList();

            foreach (var item in JoinCategoryFeatureFilter)
            {
                #region no custom Value and  cf.count >0

                if (item.Filter.CustomValue == "" && item.Filter.FeatureValueId.Count > 0)
                {
                    #region //if cf have parent

                    if (item.CategoryFeature.ParentList.Count > 0) //if cf have parent
                    {
                        int missdParent = 0;

                        foreach (var parent in item.CategoryFeature.ParentList)
                        {
                            if (!AppliedFilter.Any(ap => ap.Filter.CategoryFeatureId == parent.CategoryFeatureId))
                                missdParent++;
                        }


                        if (missdParent == 0 || item.CategoryFeature.LoadInFirstTime)
                        {
                            int missdFv = 0;

                            foreach (var fv in item.Filter.FeatureValueId)
                            {
                                if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                    missdFv++;
                            }

                            if (missdFv == 0)
                            {
                                AppliedFilter.Add(item);
                            }
                        }
                    }
                    #endregion

                    #region //if cf have no parent
                    else
                    {
                        int missdFv = 0;

                        foreach (var fv in item.Filter.FeatureValueId)
                        {
                            if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                missdFv++;
                        }

                        if (missdFv == 0)
                        {
                            AppliedFilter.Add(item);
                        }

                    }
                    #endregion
                }

                #endregion

                if (item.Filter.CustomValue != "")
                {
                    #region //if cf have parent
                    if (item.CategoryFeature.ParentList.Count > 0)
                    {
                        int missdParent = 0;

                        foreach (var parent in item.CategoryFeature.ParentList)
                        {
                            if (!AppliedFilter.Any(ap => ap.Filter.CategoryFeatureId == parent.CategoryFeatureId))
                                missdParent++;
                        }

                        if (missdParent == 0)
                        {
                            AppliedFilter.Add(item);

                        }
                    }
                    #endregion

                    #region //if cf have no parent
                    else
                    {
                        AppliedFilter.Add(item);
                    }
                    #endregion
                }



            }

            return AppliedFilter;
        }


        internal static List<FilterAndCategoryFeatureModel> ApplyFilterForOutput(long categoryId, List<CFProdoctFilterModel> filters, ref SdbContext sdb)
        {
            List<FilterAndCategoryFeatureModel> AppliedFilter = new List<FilterAndCategoryFeatureModel>();

            List<FilterAndCategoryFeatureModel> JoinCategoryFeatureFilter =
                                filters.Join(sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                               cf.AvailableInADS &&
                                                                               cf.AvailableInSearchBox),
                                             f => f.CategoryFeatureId,
                                             cf => cf.Id,
                                             (f, cf) => new FilterAndCategoryFeatureModel
                                             {
                                                 Filter = f,
                                                 CategoryFeature = cf
                                             })
                                             .OrderBy(af => af.CategoryFeature.ParentList.Count())
                                             .OrderBy(af => af.CategoryFeature.OrderId)
                                             .ToList();

            foreach (var item in JoinCategoryFeatureFilter)
            {
                if (item.CategoryFeature.LoadInFirstTime || AppliedFilter.Any(cf => item.CategoryFeature.ParentList.Any(p => p.CategoryFeatureId == cf.CategoryFeature.Id)))
                {
                    if (item.Filter.CustomValue == "" && item.Filter.FeatureValueId.Count > 0)
                    {
                        if (!item.CategoryFeature.AvailableSearchMultiSelect)
                        {
                            if (item.Filter.FeatureValueId.Count == 1)//must have singel fv selected
                            {

                                int missdFv = 0;

                                foreach (var fv in item.Filter.FeatureValueId)
                                {
                                    if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                        missdFv++;
                                }

                                if (missdFv == 0)
                                {
                                    AppliedFilter.Add(item);
                                }

                            }
                        }

                        else
                        {
                            int missdFv = 0;

                            foreach (var fv in item.Filter.FeatureValueId)
                            {
                                if (!item.CategoryFeature.FeatureValueList.Any(c => c.Id == fv))
                                    missdFv++;
                            }

                            if (missdFv == 0)
                            {
                                AppliedFilter.Add(item);
                            }

                        }

                    }

                    if (item.Filter.CustomValue != "")
                    {

                        AppliedFilter.Add(item);
                    }
                }
            }

            return AppliedFilter;
        }


        internal static List<AdvertiseLocationSerarchViewModel> ConfigLocationOutPutResult(List<BsonDocument> searchResult, List<long> VisitedProduct)
        {

            List<AdvertiseLocationSerarchViewModel> model = new List<AdvertiseLocationSerarchViewModel>();

            JavaScriptSerializer js = new JavaScriptSerializer();

            foreach (var resultItem in searchResult)
            {

                BsonElement output = resultItem.Elements.FirstOrDefault(re => re.Name == "Output");

                if (output.Name != null)
                {
                    AdvertiseLocationSerarchViewModel temp = new AdvertiseLocationSerarchViewModel();
                    temp.advertise = js.Deserialize<SearchResultItemViewModel>(Encoding.UTF8.GetString(Convert.FromBase64String(output.Value.ToString())));

                    temp.advertise.date = Convert.ToDateTime(resultItem["InsertTime"].ToString()).ToUniversalTime().UTCToPrettyPersian();
                    temp.advertise.text10 = VisitedProduct.Any(c => c.ToString() == resultItem.Elements.FirstOrDefault(re => re.Name == "Id").Value.ToString());

                    temp.distance = Math.Round(resultItem.GetElement("Dis").Value.ToDouble(), digits: 2);


                    var point = resultItem.GetElement("Location").Value.ToBsonDocument().GetElement("coordinates").Value.AsBsonArray.ToArray();

                    temp.lat = point[0].ToString().StringToDouble();
                    temp.lng = point[1].ToString().StringToDouble();

                    model.Add(temp);
                }


            }

            return model;

        }

        #endregion

        #region Sort
        internal static SortDefinition<BsonDocument> GetProductSort(SortType type)
        {
            if (type == SortType.Mostvisit)
                return Builders<BsonDocument>.Sort.Descending("VisitCount");

            return Builders<BsonDocument>.Sort.Descending("InsertTime");
        }
        #endregion

        #region out Put
        internal static List<SearchResultItemViewModel> ConfigOutPutResult(List<BsonDocument> searchResult,
                                                                 List<long> VisitedProduct)
        {

            List<SearchResultItemViewModel> model = new List<SearchResultItemViewModel>();

            JavaScriptSerializer js = new JavaScriptSerializer();

            foreach (var resultItem in searchResult)
            {

                BsonElement output = resultItem.Elements.FirstOrDefault(re => re.Name == "Output");

                if (output.Name != null)
                {
                    SearchResultItemViewModel temp = js.Deserialize<SearchResultItemViewModel>(Encoding.UTF8.GetString(Convert.FromBase64String(output.Value.ToString())));

                    temp.date = Convert.ToDateTime(resultItem["InsertTime"].ToString()).ToUniversalTime().UTCToPrettyPersian();
                    temp.text10 = VisitedProduct.Any(c => c.ToString() == resultItem.Elements.FirstOrDefault(re => re.Name == "Id").Value.ToString());

                    model.Add(temp);
                }


            }

            return model;

        }


        internal static string ConfigOutPutResultSingle(BsonDocument resultItem,
                                                      List<long> VisitedProduct,
                                                      long categoryId,
                                                      ref SdbContext sdb)
        {
            List<CategoryFeature> CategoryFeature = sdb.CategoryFeatures.Where(cf => cf.AvailableInSearchResult &&
                                                                                     cf.CategoryId == categoryId)
                                                                        .OrderBy(cf => cf.OrderId)
                                                                        .Take(7)
                                                                        .ToList();
            if (CategoryFeature.Count != 7)
                throw new RecabException("config error system have no 7 element for resultsearch");


            Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(resultItem);

            string[] outputItem = new string[7];

            for (int i = 0; i < 7; i++)
            {
                object newObject = new object();
                data.TryGetValue("CF_" + CategoryFeature[i].Id.ToString(), out newObject);

                if (newObject != null)
                {
                    if (!CategoryFeature[i].HasMultiSelectValue && !CategoryFeature[i].HasCustomValue)
                    {
                        FeatureValue featureValue = CategoryFeature[i].FeatureValueList.Find(fv => fv.Id.ToString() == newObject.ToString());
                        outputItem[i] = featureValue.Title;
                    }

                    if (!CategoryFeature[i].HasMultiSelectValue && CategoryFeature[i].HasCustomValue)
                    {
                        var sadsasdasd = newObject.ToString();
                        if (sadsasdasd != "CUM_[]")
                        { outputItem[i] = (sadsasdasd).Replace("CUM_", "").Replace("[", "").Replace("]", "").Replace(" ", ""); }
                        else
                        { outputItem[i] = ""; }
                    }

                    if (CategoryFeature[i].HasMultiSelectValue)
                    {
                        List<long> multiValues = newObject as List<long>;

                        string multiData = "";

                        foreach (var multiValueItem in multiValues)
                        {
                            multiData = (multiData == "") ?
                                (multiData = multiValueItem.ToString()) :
                                (multiData = multiData + "," + multiValueItem.ToString());

                        }
                    }

                }
                else
                {
                    outputItem[i] = "";
                }

            }

            long advertiseId = 0;
            long.TryParse(data["Id"].ToString(), out advertiseId);

            Media media = sdb.Media.FirstOrDefault(m => m.EntityId == advertiseId && m.EntityType == EntityType.Product);

            SearchResultItemViewModel model = new SearchResultItemViewModel
            {
                advertiseId = advertiseId,
                imageUrl = media != null ? media.MediaURL : "",
                description = data["Description"].ToString() ?? "",
                date = " ",
                text1 = outputItem[0],
                text2 = outputItem[1],
                text3 = outputItem[2],
                text4 = outputItem[3],
                text5 = outputItem[4],
                text6 = data["HasDealership"].ToString() ?? "",
                text7 = data["HasExchange"].ToString() ?? "",
                text8 = outputItem[5],
                text9 = outputItem[6],
                text10 = VisitedProduct.Any(c => c == advertiseId)

            };

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(model);

        }
        #endregion

        #region Ads org
        internal static long GetUserAlertId(long userId, ref SdbContext sdb)
        {
            long alertId = 0;
            if (userId != 0)
            {
                if (!sdb.Users.Any(u => u.Id == userId))
                    throw new RecabException((int)ExceptionType.UserNotFound);

                AlertProduct alert = sdb.AlertProduct.OrderByDescending(ap => ap.InsertDate).FirstOrDefault(ap => ap.Status == AlertProductStatus.فعال && ap.UserId == userId);
                if (alert != null)
                    alertId = alert.Id;
            }

            return alertId;
        }


        internal static BsonDocument GetMongoFavouriteSearch(long categoryId, List<long> productIds)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });
            mongoFilter.Add(new BsonDocument { { "Status", ((int)ProdoctStatus.فعال).ToString() } });

            BsonArray arrayItem = new BsonArray();

            foreach (var fv in productIds)
            {
                arrayItem.Add(fv.ToString());
            }

            mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });

            return new BsonDocument { { "$and", mongoFilter } };

        }


        internal static List<long> UserAlertVisit(long userId, long alertId, ref MdbContext mdb)
        {

            var productIds = mdb.UserAlertVisit.Find(filter: new BsonDocument {
                                                                                 { "UserId", userId.ToString() },
                                                                                 { "AlertId", alertId.ToString() }
                                                                                }).ToList();

            return productIds.Select(c => Convert.ToInt64(c["ProductId"])).ToList();
        }


        internal static bool UserProductVisitInAlert(long userId, long productId, long alertId, ref MdbContext mdb)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "UserId", userId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "ProductId", productId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "AlertId", alertId.ToString() } });

            var filter = new BsonDocument { { "$and", mongoFilter } };

            long count = mdb.UserAlertVisit.Find(filter: filter).Count();


            return count > 0;

        }


        internal static void AddUserProductVisit(long userId, long productId, ref MdbContext mdb)
        {
            if (userId == 0)
                return;

            BsonDocument newMongoProduct = new BsonDocument {

                { "UserId",userId.ToString() },
                { "ProductId" , productId.ToString() }

            };
            mdb.UserProductVisit.InsertOneAsync(newMongoProduct);
        }


        internal static List<CFProdoctFilterModel> DecreaseRelativeADSFilter(List<FilterAndCategoryFeatureModel> appliedFilter)
        {
            appliedFilter = appliedFilter.Where(af => af.CategoryFeature.AvailableInRelativeADS).OrderByDescending(af => af.CategoryFeature.RelativeADSOrder).ToList();

            if (appliedFilter.Count == 1)
                return new List<CFProdoctFilterModel>();

            appliedFilter.Remove(appliedFilter.FirstOrDefault());

            return appliedFilter.Select(ap => ap.Filter).ToList();

        }


        internal static void UpdateProductVisit(long id, long count, ref MdbContext mdb)
        {

            var filter = Builders<BsonDocument>.Filter.Eq("Id", id.ToString());

            var update = Builders<BsonDocument>.Update
                .Set("VisitCount", count)
                .CurrentDate("lastModified");

            mdb.Products.UpdateMany(filter, update);
        }


        internal static void MongoProdoctSave(Product prodoct, ref MdbContext _mdb, ref SdbContext _sdb)
        {

            if (prodoct.Status != ProdoctStatus.فعال)
                return;

            BsonDocument newMongoProduct = new BsonDocument {

                { "Id" , prodoct.Id.ToString() },
                { "UserId",prodoct.UserId.ToString() },
                { "CategoryId", prodoct.CategoryId.ToString() },
                { "Description", prodoct.Description},
                { "Tell", prodoct.Tell },
                { "Status",((int)prodoct.Status).ToString() },
                { "Priority",prodoct.Priority},
                { "RaiseDate", prodoct.RaiseDate},
                { "RaiseHourTime",prodoct.RaiseHourTime },
                { "RaiseBaseQuota",prodoct.RaiseBaseQuota },
                { "RaiseUsedQuota", prodoct.RaiseUsedQuota},
                { "CPPTId",(!prodoct.UserPackageCreditId.HasValue?"0":( (int)prodoct.UserPackageCredit.CategoryPurchasePackageType.Id).ToString())},
                { "InsertTime",prodoct.InsertDate},
                { "WebVisitCount", prodoct.WebVisitCount.ToString() },
                { "AndroidVisitCount", prodoct.AndroidVisitCount.ToString() },
                { "VisitCount" , (prodoct.AndroidVisitCount+prodoct.WebVisitCount) },
                { "IosVisitCount", prodoct.IosVisitCount.ToString() },
                { "HasDealership",prodoct.DealershipId.HasValue.ToString()},
                { "HasExchange",prodoct.CategoryExchangeId.HasValue.ToString()},
                { "HasMedia",_sdb.Media.Any(m=> m.EntityId == prodoct.Id && m.EntityType == EntityType.Product) ? 1 : 0 }
            };

            if (prodoct.DealershipId.HasValue)
            {
                newMongoProduct.Add("DealershipStatus", ((int)prodoct.Dealership.Status).ToString());
            }
            else
            {
                newMongoProduct.Add("DealershipStatus", ((int)DealershipStatus.فعال).ToString());
            }

            if (prodoct.DealershipId.HasValue)
                newMongoProduct.Add("DealershipId", prodoct.DealershipId.Value.ToString());

            if (prodoct.CategoryExchangeId.HasValue)
            {
                BsonDocument newMongoExchange = new BsonDocument
                {
                    { "UserId",prodoct.UserId.ToString() },
                    { "ProdoctId",prodoct.Id.ToString() } ,
                    { "CategoryExchangeId",prodoct.CategoryExchangeId.Value.ToString() }
                };

                foreach (ExchangeFeatureValue item in prodoct.ExchangeFeatureValues)
                {
                    if (item.CategoryFeature.HasCustomValue)
                    {

                        long numericValue = 0;

                        if (long.TryParse(item.CustomValue, out numericValue))
                        {
                            newMongoExchange.Add("CF_" + item.CategoryFeatureId.ToString(), numericValue);
                        }
                        else
                        {
                            newMongoExchange.Add("CF_" + item.CategoryFeatureId.ToString(), "CUM_" + item.CustomValue ?? "");
                        }
                    }
                    else
                    {
                        newMongoExchange.Add("CF_" + item.CategoryFeatureId.ToString(), item.FeatureValueId != null ? item.FeatureValueId.ToString() : "0");
                    }
                }

                _mdb.ExchangeProdocts.InsertOneAsync(newMongoExchange);

            }


            foreach (ProductFeatureValue item in prodoct.ProductFeatures)
            {

                if (item.CategoryFeature.HasCustomValue)
                {
                    if (item.CategoryFeature.IsMap)
                    {

                        string[] cordinate = item.CustomValue
                                                 .Replace(" ", "")
                                                 .Replace("[", "")
                                                 .Replace("]", "")
                                                 .Split(',');
                        if (cordinate.Length == 2)
                        {


                            BsonArray coordinatArray = new BsonArray();
                            coordinatArray.Add(cordinate[0].StringToDouble());
                            coordinatArray.Add(cordinate[1].StringToDouble());

                            newMongoProduct.Add("Location", new BsonDocument { { "type" , "Point" },
                                                                                   { "coordinates",coordinatArray } });

                        }

                    }

                    else
                    {
                        long numericValue = 0;

                        if (long.TryParse(item.CustomValue, out numericValue))
                        {
                            newMongoProduct.Add("CF_" + item.CategoryFeatureId.ToString(), numericValue);
                        }
                        else
                        {
                            newMongoProduct.Add("CF_" + item.CategoryFeatureId.ToString(), "CUM_" + item.CustomValue ?? "");
                        }
                    }
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

                        newMongoProduct.Add("CF_" + item.CategoryFeatureId.ToString(), bArray);
                    }
                    else
                    {
                        newMongoProduct.Add("CF_" + item.CategoryFeatureId.ToString(), item.ListFeatureValue.FirstOrDefault() != null ? item.ListFeatureValue.FirstOrDefault().FeatureValueId.ToString() : "0");
                    }
                }
            }

            string outpute = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ProductHelperService.ConfigOutPutResultSingle(resultItem: newMongoProduct,
                                                                                           VisitedProduct: new List<long>(),
                                                                                            categoryId: prodoct.CategoryId,
                                                                                            sdb: ref _sdb)));


            newMongoProduct.Add("Output", outpute);

            _mdb.Products.InsertOneAsync(newMongoProduct);

            EmailService emailservice = new EmailService(ref _sdb, ref _mdb);

            emailservice.SendConfirmAds(prodoct);

        }


        internal static void MongoProductUpdate(Product prodoct, ref MdbContext _mdb, ref SdbContext _sdb)
        {

            _mdb.Products.DeleteMany(filter: new BsonDocument { { "Id", prodoct.Id.ToString() } });

            _mdb.UserAlertVisit.DeleteMany(filter: new BsonDocument { { "ProductId", prodoct.Id.ToString() } });

            MongoProdoctSave(prodoct: prodoct, _mdb: ref _mdb, _sdb: ref _sdb);

        }


        internal static bool RaiseUp(long productId, ref MdbContext _mdb, ref SdbContext _sdb)
        {
            Product product = _sdb.Product.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            if (product.RaiseUsedQuota == 0)

                throw new RecabException((int)ExceptionType.RaiseQuotaLimit);

            product.RaiseUsedQuota = product.RaiseUsedQuota - 1;

            product.RaiseDate = DateTime.UtcNow;

            product.Priority = PriorityStatus.بالا;

            _sdb.SaveChanges();


            var filter = Builders<BsonDocument>.Filter.Eq("Id", product.Id.ToString());

            var update = Builders<BsonDocument>.Update
                .Set("Priority", product.Priority)
                .Set("RaiseDate", product.RaiseDate)
                .Set("RaiseUsedQuota", product.RaiseUsedQuota);

            _mdb.Products.UpdateMany(filter: filter, update: update);

            return true;

        }


        #endregion

        #region search

        internal static List<SearchResultItemViewModel> SearchProduct(string keyword,
                                                     long categoryId,
                                                     List<CFProdoctFilterModel> filters,
                                                     int type,
                                                     int sort,
                                                     bool packageReletive,
                                                     ref long Count,
                                                     ref MdbContext _mdb,
                                                     ref SdbContext _sdb,
                                                     long userId = 0,
                                                     int size = 1,
                                                     int page = 0)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            long alertId = ProductHelperService.GetUserAlertId(userId: userId, sdb: ref _sdb);

            JavaScriptSerializer scr = new JavaScriptSerializer();


            List<FilterAndCategoryFeatureModel> AppliedFilter = ProductHelperService.ApplyFilter(categoryId: categoryId,
                                                                                                   filters: filters,
                                                                                                   sdb: ref _sdb);

            BsonDocument match = new BsonDocument();

            switch ((AlertSearchType)type)
            {
                case AlertSearchType.notVisit:
                    match = ProductHelperService.GetMongoFilterSearch(categoryId: categoryId,
                                                      filter: AppliedFilter,
                                                      userId: userId,
                                                      visited: false,
                                                      keyword: keyword,
                                                      alertId: alertId,
                                                      sdb: ref _sdb,
                                                      mdb: ref _mdb);
                    break;

                case AlertSearchType.visit:
                    match = ProductHelperService.GetMongoFilterSearch(categoryId: categoryId,
                                                                      filter: AppliedFilter,
                                                                      userId: userId,
                                                                      visited: true,
                                                                      keyword: keyword,
                                                                      alertId: alertId,
                                                                      sdb: ref _sdb,
                                                                      mdb: ref _mdb);
                    break;
                default:
                    match = ProductHelperService.GetMongoFilterSearch(categoryId: categoryId,
                                                                    keyword: keyword,
                                                                    filter: AppliedFilter,
                                                                    packageReletive: packageReletive,
                                                                    sdb: ref _sdb,
                                                                    mdb: ref _mdb);
                    break;

            }




            Count = _mdb.Products.Count(match);

            var MongoResult = _mdb.Products.Find(match)
                                            .Sort(ProductHelperService.GetProductSort((SortType)sort))
                                            .Skip(size * page)
                                            .Limit(size)
                                            .ToList();


            return ProductHelperService.ConfigOutPutResult(searchResult: MongoResult,
                                                            VisitedProduct: ProductHelperService.UserAlertVisit(userId, alertId, ref _mdb));
        }


        internal static AggregateResultModel AggregateSearchProduct(string keyword,
                                                                long categoryId,
                                                                List<CFProdoctFilterModel> filters,
                                                                ref MdbContext _mdb,
                                                                ref SdbContext _sdb)
        {
            Category category = _sdb.Categoris.Find(categoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            JavaScriptSerializer scr = new JavaScriptSerializer();

            List<FilterAndCategoryFeatureModel> AppliedFilter = ProductHelperService.ApplyFilter(categoryId: categoryId,
                                                                                                 filters: filters,
                                                                                                 sdb: ref _sdb);

            List<CategoryFeature> categoryFeatures = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                                        cf.AvailableInSearchBox &&
                                                                                        !cf.HasMultiSelectValue &&
                                                                                        !cf.HasCustomValue)
                                                         .OrderBy(c => c.OrderId).ToList();

            #region aggrigate


            // _mdb.db.RunCommand<BsonDocument>(J);

            BsonDocument Location = new BsonDocument();
            var match = ProductHelperService.GetMongoFilterAggregate(categoryId: categoryId,
                                                                     filter: AppliedFilter,
                                                                     keyword: keyword,
                                                                     Location: ref Location,
                                                                     sdb: ref _sdb,
                                                                     mdb: ref _mdb);

            List<CFAggregateModel> AggrigationData = new List<CFAggregateModel>();
            foreach (var item in categoryFeatures)
            {

                var group = new BsonDocument
                    {{"$group",new BsonDocument
                        {
                         { "_id", "$CF_"+item.Id.ToString() },
                         { "count",new BsonDocument{ { "$sum",1} } }
                        }
                      }};


                var pipeline = new[] { match, group };


                List<FeatuerValueAggModel> result = _mdb.Products.Aggregate<FeatuerValueAggModel>(pipeline).ToList();

                AggrigationData.Add(new CFAggregateModel { categoryFeatureId = item.Id, feachurValue = result });



            }

            List<CategoryFeature> categoryFeaturesMulti = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == categoryId &&
                                                                                            cf.AvailableInSearchBox &&
                                                                                            !cf.HasCustomValue &&
                                                                                            cf.HasMultiSelectValue)
                                                         .OrderBy(c => c.OrderId).ToList();

            categoryFeatures = categoryFeatures.Concat(categoryFeaturesMulti).ToList();

            foreach (var item in categoryFeaturesMulti)
            {

                var group = new BsonDocument {
                 {"$group",new BsonDocument
                    {  { "_id", "$CF_"+item.Id.ToString() },{ "count",new BsonDocument{ { "$sum",1} } }}
                  }
                };

                var unwind = new BsonDocument {
                 {"$unwind", "$CF_"+item.Id.ToString() }};

                var pipeline = new[] { match, unwind, group };



                List<object> result = _mdb.Products.Aggregate<object>(pipeline).ToList();

                CFAggregateModel AggModel = new CFAggregateModel { categoryFeatureId = item.Id };

                foreach (var t in result)
                {

                    string a = t.ToJson().Replace("(", "").Replace(")", "").Replace("NumberLong", "");
                    FeatuerValueAggModelLong mm = scr.Deserialize<FeatuerValueAggModelLong>(a);
                    AggModel.feachurValue.Add(new FeatuerValueAggModel { _id = mm._id.ToString(), count = mm.count });

                }

                AggrigationData.Add(AggModel);
            }

            #endregion

            long categoryFeaturCount = categoryFeatures.Count;

            List<CFAggregateViewModel> aggregate = new List<CFAggregateViewModel>();

            foreach (CFAggregateModel item in AggrigationData)
            {
                CFAggregateViewModel temp = new CFAggregateViewModel();

                CategoryFeature categoryfeature = categoryFeatures.First(cf => cf.Id == item.categoryFeatureId);

                if (!categoryfeature.HasCustomValue)
                {
                    temp.title = categoryfeature.Title;
                    temp.categoryFeatureId = categoryfeature.Id;

                    long countPerItem = 0;

                    foreach (var fvItem in item.feachurValue)
                    {
                        countPerItem = countPerItem + fvItem.count;
                        if (fvItem._id != null)
                        {
                            var featureValue = categoryfeature.FeatureValueList.FirstOrDefault(fv => fv.Id.ToString() == fvItem._id);

                            //
                            if (featureValue == null)
                            {
                                temp.featureValues.Add(new FeatuerValueAggregateViewModel
                                {
                                    count = fvItem.count,
                                    title = fvItem._id
                                });

                            }
                            else
                            {
                                temp.featureValues.Add(new FeatuerValueAggregateViewModel
                                {
                                    count = fvItem.count,
                                    featureValueId = featureValue.Id,
                                    title = featureValue.Title
                                });
                            }
                        }

                    }

                    if (!filters.Any(f => f.CategoryFeatureId == categoryfeature.Id) || categoryfeature.AvailableSearchMultiSelect)
                    {
                        if (categoryfeature.ParentList.Count == 0)
                        {
                            temp.totalCount = countPerItem;

                            aggregate.Add(temp);
                        }
                        else
                        {
                            int a = 0;
                            foreach (var tempItem in categoryfeature.ParentList)
                            {
                                if (categoryFeatures.Any(cf => cf.Id == tempItem.CategoryFeatureId))
                                {
                                    if (!filters.Any(f => f.CategoryFeatureId == tempItem.CategoryFeatureId))
                                        a++;
                                }
                            }

                            if (a == 0)
                            {

                                temp.totalCount = countPerItem;
                                aggregate.Add(temp);
                            }

                        }
                    }

                    if (categoryfeature.LoadInFirstTime)
                    {
                        if (!aggregate.Any(c => c.categoryFeatureId == temp.categoryFeatureId))
                            aggregate.Add(temp);
                    }
                }
            }

            aggregate.RemoveAll(m => m.featureValues.Count == 0);

            AggregateResultModel model = new AggregateResultModel();

            model.totalCount = aggregate.FirstOrDefault() != null ? aggregate.FirstOrDefault().totalCount : 0;

            model.aggregates = aggregate.OrderBy(m => m.categoryFeatureId).ToList();

            AppliedFilter = ProductHelperService.ApplyFilterForOutput(categoryId: categoryId, filters: filters, sdb: ref _sdb);

            model.selectedItems = AppliedFilter.Select(f => new Exon.Recab.Service.Model.ProdoctModel.SelectItemFilterSearchModel
            {
                categoryFeatureId = f.CategoryFeature.Id,
                customValue = f.Filter.CustomValue ?? "",
                selectedFeatureValues = f.Filter.FeatureValueId.Count > 0 ? f.Filter.FeatureValueId.Select(fv => new SelectedFeatureValue
                {
                    featureValueId = fv,
                    featureValueTitle = f.CategoryFeature.FeatureValueList.First(c => c.Id == fv).Title
                }).ToList()
                                                                            : new List<SelectedFeatureValue>(),
            }).ToList();

            return model;


        }


        internal static List<AdvertiseLocationSerarchViewModel> LocationSearchProduct(long userId,
                                                                    string keyword,
                                                                    long categoryId,
                                                                    double lat,
                                                                    double lng,
                                                                    int distance,
                                                                    List<CFProdoctFilterModel> filters,
                                                                    ref long count,
                                                                    ref MdbContext _mdb,
                                                                    ref SdbContext _sdb,
                                                                    int size = 1,
                                                                    int page = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            long alertId = ProductHelperService.GetUserAlertId(userId: userId, sdb: ref _sdb);

            JavaScriptSerializer scr = new JavaScriptSerializer();

            List<FilterAndCategoryFeatureModel> AppliedFilter = ProductHelperService.ApplyFilter(categoryId: categoryId,
                                                                                                 filters: filters,
                                                                                                 sdb: ref _sdb);

            BsonDocument Location = new BsonDocument();
            BsonDocument Match = ProductHelperService.GetMongoFilterLocaton(categoryId: categoryId,
                                                                     filter: AppliedFilter,
                                                                     keyword: keyword,
                                                                     lat: lat,
                                                                     lng: lng,
                                                                     distance: distance,
                                                                     Location: ref Location,
                                                                     sdb: ref _sdb,
                                                                     mdb: ref _mdb);
            var pipline = new BsonDocument[] { Location,
                                                                                Match ,
                                                                               new BsonDocument { { "$skip" , size*page } },
                                                                               new BsonDocument { { "$limit",size } }
                                                                              };
            count = _mdb.Products.Aggregate<BsonDocument>(new BsonDocument[] { Location, Match }).ToList().Count();
            List<BsonDocument> result = _mdb.Products.Aggregate<BsonDocument>(pipline).ToList();




            return ProductHelperService.ConfigLocationOutPutResult(result, ProductHelperService.UserAlertVisit(userId, alertId, ref _mdb));
        }

        internal static List<SearchResultItemViewModel> GetAllFavouriteProduct(long userId,
                                                                            long categoryId,
                                                                            ref long count,
                                                                            ref MdbContext _mdb,
                                                                            ref SdbContext _sdb,
                                                                            int size = 1,
                                                                            int skip = 0)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<long> prodouctIds = _sdb.FavouriteProduct.Where(pf => pf.UserId == user.Id &&
                                                                      pf.Product.Status == ProdoctStatus.فعال &&
                                                                      (pf.Product.DealershipId.HasValue ? pf.Product.Dealership.Status == DealershipStatus.فعال : true)).Select(pf => pf.ProductId).ToList();

            BsonDocument match = ProductHelperService.GetMongoFavouriteSearch(categoryId, prodouctIds);

            var MongoResult = _mdb.Products.Find(match).ToList();

            return ProductHelperService.ConfigOutPutResult(searchResult: MongoResult,
                                                          VisitedProduct: ProductHelperService.UserAlertVisit(userId, new long(), ref _mdb));


        }

        #endregion

        public static List<long> TagItems(int entityType,
                                          string keyword,
                                          long categoryId,
                                          ref SdbContext sdb,
                                          ref MdbContext mdb)
        {
            Category category = sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", category.Id.ToString() } });

            mongoFilter.Add(new BsonDocument { { "$text", new BsonDocument { { "$search", keyword } } } });


            var Result = mdb.Tag.Find(new BsonDocument { { "$and", mongoFilter } }).Project(new BsonDocument()).ToList();


            List<long> model = new List<long>();

            switch ((EntityType)entityType)
            {
                case EntityType.Article:

                    foreach (var item in Result)
                    {
                        Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                        var ArticleItems = (ICollection)data["ArticleItems"];

                        foreach (var id in ArticleItems)
                        {
                            model.Add(Convert.ToInt64(id));
                        }

                    }

                    break;

                case EntityType.Review:
                    foreach (var item in Result)
                    {
                        Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                        var ReviewItems = (ICollection)data["ReviewItems"];

                        foreach (var id in ReviewItems)
                        {
                            model.Add(Convert.ToInt64(id));
                        }

                    }
                    break;

                case EntityType.Product:
                    foreach (var item in Result)
                    {
                        Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                        var ProductItems = (ICollection)data["ProductItems"];

                        foreach (var id in ProductItems)
                        {
                            model.Add(Convert.ToInt64(id));
                        }

                    }
                    break;
                case EntityType.TodayPrice:

                    foreach (var item in Result)
                    {
                        Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);

                        var TodayPriceItems = (ICollection)data["TodayPriceItems"];

                        foreach (var id in TodayPriceItems)
                        {
                            model.Add(Convert.ToInt64(id));
                        }

                    }

                    break;

                default:
                    return new List<long>();

            }


            return model.Distinct().ToList();
        }
        #endregion

        #region Review

        internal static BsonDocument GetMongoFilterSearchRV(long categoryId,
                                                            List<ReviewFilterModelWithCategory> filter,
                                                            string keyword ,
                                                            ref SdbContext _sdb ,
                                                            ref MdbContext _mdb)
        {
            BsonArray mongoFilter = new BsonArray();

            if (keyword.Length >= 2)
            {

                List<long> tagItems = ProductHelperService.TagItems(entityType: (int)EntityType.Review, keyword: keyword, categoryId: categoryId, sdb: ref _sdb, mdb: ref _mdb);

                BsonArray arrayItem = new BsonArray();

                foreach (var fv in tagItems)
                {
                    arrayItem.Add(fv.ToString());
                }

                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });
            }

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });

            foreach (var item in filter)
            {
                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.ToString() } });

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        internal static void MongoReviewSave(Review review , ref SdbContext _sdb ,ref MdbContext _mdb)
        {

            BsonDocument newMongoReview = new BsonDocument {

                { "Id" , review.Id.ToString() },
                { "UserId",review.UserId.ToString() },
                { "CategoryId", review.CategoryId.ToString() },
                { "Title" , review.Title },
                { "Body" , review.Body},
                { "Rate", review.Rate },
                { "VisitCount", review.VisitCount } ,
                { "CreateTime" , review.CreateTime.ToLongTimeString()}
            };


            List<FeatureValueAssign> FeatureValueAssigns = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.Review &&
                                                                                                  fva.EntityId == review.Id).ToList();


            foreach (FeatureValueAssign item in FeatureValueAssigns)
            {
                if (item.CategoryFeature.HasCustomValue)
                {
                    newMongoReview.Add("CF_" + item.CategoryFeatureId.ToString(), "CUM_" + item.CustomValue ?? "");
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

                        newMongoReview.Add("CF_" + item.CategoryFeatureId.ToString(), bArray);
                    }
                    else
                    {
                        newMongoReview.Add("CF_" + item.CategoryFeatureId.ToString(), item.ListFeatureValue.FirstOrDefault() != null ? item.ListFeatureValue.FirstOrDefault().FeatureValueId.ToString() : "0");
                    }
                }
            }


            #region result

            List<ReviewCategoryFeatureViewModel> model = new List<ReviewCategoryFeatureViewModel>();

            List<CategoryFeature> categoryFeature = _sdb.CategoryFeatures.Where(c => c.CategoryId == review.CategoryId &&
                                                                                     c.AvailableInReview).ToList();

            List<long> featerValue = new List<long>();


            foreach (var item in FeatureValueAssigns.OrderBy(fva => fva.CategoryFeature.OrderId))
            {

                featerValue = featerValue.Concat(item.ListFeatureValue.Select(fv => fv.FeatureValueId).ToList()).ToList();
                model.Add(new ReviewCategoryFeatureViewModel
                {
                    categoryFeatureId = item.CategoryFeatureId,
                    title = item.CategoryFeature.Title,
                    featureValues = item.ListFeatureValue.Select(fv => new ReviewFeatureValueViewModel { title = fv.FeatureValue.Title, featureValueId = fv.FeatureValueId }).ToList()
                });
            }



            List<FeatureValue> DependentReviewItems = _sdb.FeatureValues.Where(fv => fv.CategoryFeature.CategoryId == review.CategoryId &&
                                                                                      fv.ParentList.Select(fvp => fvp.FeatureValueId)
                                                                                                   .Join(featerValue, p => p, f => f, (p, f) => (p))
                                                                                                   .Count() > 0).ToList();


            DependentReviewItems = DependentReviewItems.Where(i => !FeatureValueAssigns.Any(fva => fva.CategoryFeatureId == i.CategoryFeatureId))
                                                       .OrderBy(fv => fv.CategoryFeature.OrderId)
                                                       .ToList();

            foreach (var item in DependentReviewItems)
            {

                if (!model.Any(m => m.categoryFeatureId == item.CategoryFeatureId))
                {
                    List<ReviewFeatureValueViewModel> ReviewFeatureValue = new List<ReviewFeatureValueViewModel>();

                    ReviewFeatureValue.Add(new ReviewFeatureValueViewModel { featureValueId = item.Id, title = item.Title });

                    model.Add(new ReviewCategoryFeatureViewModel
                    {
                        categoryFeatureId = item.CategoryFeatureId,
                        title = item.CategoryFeature.Title,
                        featureValues = ReviewFeatureValue
                    });
                }
                else
                {
                    model.FirstOrDefault(m => m.categoryFeatureId == item.CategoryFeatureId).featureValues.Add(new ReviewFeatureValueViewModel
                    {
                        featureValueId = item.Id,
                        title = item.Title
                    });


                }
            }

            JavaScriptSerializer js = new JavaScriptSerializer();

            newMongoReview.Add("Result", Convert.ToBase64String(Encoding.UTF8.GetBytes(js.Serialize(model))));
            #endregion



            _mdb.Reviews.InsertOne(newMongoReview);

        }
        #endregion


    }


}
