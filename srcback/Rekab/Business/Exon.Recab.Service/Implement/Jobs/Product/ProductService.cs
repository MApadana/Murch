using Exon.Recab.Domain.Constant.Email;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Service.Implement.Alert;
using Exon.Recab.Service.Implement.Helper;
using Exon.Recab.Service.Model.EmailModel;
using Exon.Recab.Service.Model.EmailModel.SendEmailModel;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.ProdoctModel.AlertModel;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Resource;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Exon.Recab.Service.Implement.Jobs.Product
{
    public class ProductService
    {
        private MdbContext _mdb;

        private SdbContext _sdb;

        public ProductService()
        {
            _mdb = new MdbContext();
            _sdb = new SdbContext();
        }

        public void RaiseDown()
        {

            var collection = _mdb.Products.Find(filter: new BsonDocument { { "Priority", new BsonDocument { { "$ne", 0 } } } }).ToList();


            foreach (var item in collection)
            {
                Dictionary<string, object> data = BsonSerializer.Deserialize<Dictionary<string, object>>(item);
                int priority = Convert.ToInt32(data["Priority"]);

                if (priority != 0)
                {
                    int hour = Convert.ToInt32(data["RaiseHourTime"]);
                    DateTime Date = Convert.ToDateTime(data["RaiseDate"]);

                    if (Date.AddHours(hour) < DateTime.UtcNow)
                    {

                        var filter = Builders<BsonDocument>.Filter.Eq("Id", data["Id"]);

                        var update = Builders<BsonDocument>.Update
                            .Set("Priority", 0)
                            .CurrentDate("lastModified");

                        _mdb.Products.UpdateMany(filter: filter, update: update);

                    }
                }

            }



        }


        public void AlertSendEmail()
        {
            List<AlertProduct> Alerts = _sdb.AlertProduct.Where(ap => ap.Status == Domain.Constant.Prodoct.AlertProductStatus.فعال).ToList();


            foreach (var item in Alerts)
            {
                SendAlertEmailModel model = new SendAlertEmailModel();

                if (this.AlertDetail(item, ref model))
                {

                    BsonDocument Email = new BsonDocument {{ "Id" , Guid.NewGuid().ToString() } ,
                                                           { "Email" ,item.User.Email.ToLower()},
                                                           { "Subject" , "رسيد ثبت آگهي" },
                                                           { "EmailType", ((int)EmailType.SendAdsInfo).ToString() },
                                                           { "InsertDate" ,DateTime.UtcNow.ToString() }};

                    string body = Render.StringToString(EmailTemplate.AlertEmail, model).Replace("ی", "ي");

                    Email.Add("Body", body);

                    _mdb.Email.InsertOne(Email);

                    PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });

                }

            }



        }


        private bool AlertDetail(AlertProduct alertproduct, ref SendAlertEmailModel model)
        {
            model = new SendAlertEmailModel
            {
                Name = alertproduct.User.FirstName + " " + alertproduct.User.LastName
            };


            List<CFResultViewModel> AssingeItem = PublicService.GetEntityAssingeItems(id: alertproduct.Id,
                                                                                          type: EntityType.Alert,
                                                                                          _sdb: ref _sdb);


            var categoryFeature = AssingeItem.Select(pcf => new AlertProductCategoryFeatureDetailViewModel
            {
                categoryFeatureId = pcf.categoryFeatureId,
                selectedFeatureValues = pcf.featureValues.Select(i => i.featureValueId).ToList(),
                customValue = pcf.customValue

            }).ToList();


            long count = 0;
            long visit = 0;

            AlertService _alertService = new AlertService(ref _sdb, ref _mdb);

            var Result = _alertService.SearchAlertProduct(userId: alertproduct.UserId,
                                            categoryId: alertproduct.CategoryId,
                                            type: 0,
                                            size: 2,
                                            page: 0,
                                            Count: ref count,
                                            visitCount: ref visit,
                                            filters: categoryFeature.Select(cf => new CFProdoctFilterModel
                                            {
                                                CategoryFeatureId = cf.categoryFeatureId,
                                                CustomValue = cf.customValue,
                                                FeatureValueId = cf.selectedFeatureValues
                                            }).ToList());

            if (count == 0)
                return false;

            foreach (var item in Result)
            {
                model.AlertItems.Add(new AlertItemEmailModel
                {
                    Title = item.text1 + " " + item.text2 + " " + item.text3,
                    Url = item.imageUrl
                });
            }


            foreach (var item in AssingeItem)
            {
                string fvTitle = "";
                foreach (var value in item.featureValues)
                {
                    fvTitle = fvTitle + " " + value.title;
                }

                model.CFItems.Add(new SendAdsInfoCategoryFeatureEmailModel
                {
                    CFTitle = item.title,
                    FVTitle = item.customValue != "" ? item.customValue : fvTitle
                });
            }

            return true;

        }


    }
}
