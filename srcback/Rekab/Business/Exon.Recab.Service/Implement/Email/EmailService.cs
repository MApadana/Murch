using Exon.Recab.Domain.Constant.Email;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Utility.Security;
using Exon.Recab.Service.Implement.Helper;
using Exon.Recab.Service.Model.EmailModel;
using Exon.Recab.Service.Model.EmailModel.SendEmailModel;
using Exon.Recab.Service.Resource;
using MongoDB.Bson;
using Nustache.Core;
using System;
using System.Linq;


namespace Exon.Recab.Service.Implement.Email
{
    public class EmailService
    {
        private readonly SdbContext _sdb;

        private readonly MdbContext _mdb;

        internal EmailService(ref SdbContext sdb, ref MdbContext mdb)
        { 
            this._sdb = sdb;
            this._mdb = mdb;

        }

        public bool SendVerificationEmail(Domain.Entity.User user, int type)
        {
            string code = CodeHelper.NewVerification();

            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" , user.Email.ToLower()},
                { "Subject" , "تغيير کلمه عبور" },
                { "EmailType", ((int)EmailType.VerificationEmail).ToString() },
                { "VerifyType" , type.ToString()},
                { "Code",code } ,
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };

            SendVerificationEmailModel userData = new SendVerificationEmailModel
            {
                Code = code,
                Name = user.FirstName + " " + user.LastName

            };

            string body = Render.StringToString(EmailTemplate.SendVerficationCode, userData).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

            return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });

            //return true;
        }


        public bool SendAdsInfo(Product product)
        {
            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" ,product.User.Email.ToLower()},
                { "Subject" , "رسيد ثبت آگهي" },
                { "EmailType", ((int)EmailType.SendAdsInfo).ToString() },
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };

            var temp = product.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle).OrderBy(cf => cf.CategoryFeature.TitleOrder);

            string ADTitle = "";

            foreach (var item in temp)
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

            SendAdsInfoEmailModel sendAdsInfoEmailModel = new SendAdsInfoEmailModel
            {
                ADSTitle = ADTitle,
                Name = product.User.FirstName + " " + product.User.LastName,
                AdSLink = product.Id.ToString()
            };

            foreach (ProductFeatureValue item in product.ProductFeatures)
            {
                string fvTitle = "";
                foreach (ProductFeatureValueFeatureValueItem value in item.ListFeatureValue)
                {
                    fvTitle = fvTitle + " " + value.FeatureValue.Title;
                }
                sendAdsInfoEmailModel.CFItems.Add(new SendAdsInfoCategoryFeatureEmailModel
                {
                    CFTitle = item.CategoryFeature.Title,
                    FVTitle = (item.ListFeatureValue.Count == 0) ? item.CustomValue : fvTitle
                });
            }

            string body = Render.StringToString(EmailTemplate.AdvertiseSave, sendAdsInfoEmailModel).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

             return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });

            //return true;
        }


        public bool SendConfirmAds(Product product)
        {
            if (product.Status != Domain.Constant.Prodoct.ProdoctStatus.فعال)
                return false;

            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" ,product.User.Email.ToLower()},
                { "Subject" , "تاييد آگهي" },
                { "EmailType", ((int)EmailType.SendAdsConfirm).ToString() },
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };


            var temp = product.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle).OrderBy(cf => cf.CategoryFeature.TitleOrder);

            string ADTitle = "";

            foreach (var item in temp)
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

            SendConfirmAdsEmailModel model = new SendConfirmAdsEmailModel
            {
                Name = product.User.FirstName + " " + product.User.LastName,
                Title = ADTitle,
                ADSLink = product.Id
            };


            string body = Render.StringToString(EmailTemplate.AdvertiseConfirm, model).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

            return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });
            //return true;

        }


        public bool SendApprovedDealershipEmail(Dealership dealership)
        {         
            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" ,dealership.User.Email.ToLower()},
                { "Subject" , "تاييد نمايشگاه" },
                { "EmailType", ((int)EmailType.DealershipConfirm).ToString() },
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };

            string JobTitle = "";
            foreach (var item in dealership.DealershipCategory)
            {
                JobTitle = JobTitle + " " + item.Category.Title;
            }

            SendApprovedDealershipEmailModel model = new SendApprovedDealershipEmailModel
            {
                Name = dealership.User.FirstName + " " + dealership.User.LastName,
                Title = dealership.Title,
                Tell = dealership.Tell,
                Fax = dealership.Fax,
                Url = dealership.WebsiteUrl,
                Address = dealership.Address,
                Job = JobTitle,
                Image = dealership.LogoUrl

            };


            string body = Render.StringToString(EmailTemplate.DealershipConfirm, model).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

            return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });

          //  return true;
        }


        public bool SendWellcome(Domain.Entity.User user)
        {
            string code = CodeHelper.NewVerification();


            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" , user.Email.ToLower()},
                { "Subject" , "خوش آمديد" },
                { "EmailType", ((int)EmailType.WellCome).ToString() },
                { "VerifyType" , "0"},
                { "Code",code } ,
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };

            SendWellcomeEmailModel userData = new SendWellcomeEmailModel
            {
                Link = string.Format("http://127.0.0.1:26538/activation?email={0}&code={1}", user.Email.ToLower() , code),
                Mobile = user.Mobile,
                Email =user.Email.ToLower(),
                Name = (user.GenderType == UserGender.Female ? " سرکار خانم" : " جناب آقای") + " " + user.FirstName + " " + user.LastName
            };

            string body = Render.StringToString(EmailTemplate.Wellcome, userData).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

           return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });
            //return true;
        }


        public bool SendPurchaseDetail(UserPackageCredit UserPackageCredit)
        {
            BsonDocument Email = new BsonDocument {
                { "Id" , Guid.NewGuid().ToString() } ,
                { "Email" , UserPackageCredit.User.Email.ToLower()},
                { "Subject" , "جزييات بسته خريداري شده" },
                { "EmailType", ((int)EmailType.Package).ToString() },
                { "InsertDate" ,DateTime.UtcNow.ToString() }
            };

            string type1 = UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.Title;
            string tyep2 = UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.Category.Title;
            string type3 = UserPackageCredit.CategoryPurchasePackageType.PackageType.Title;

            SendPurchaseDetailEmailModel model = new SendPurchaseDetailEmailModel
            {
                Name = UserPackageCredit.User.FirstName + " " + UserPackageCredit.User.LastName,
                CPPTTitle = string.Format("{0} {1} {2}", type1, tyep2, type3)
            };

            foreach (var item in UserPackageCredit.CategoryPurchasePackageType.PurchaseConfig)
            {
                if (item.Value == "true")
                {
                    model.Configs.Add(new PurchaseConfigItemEmailModel
                    {
                        Title = item.PackageBaseConfig.Title,
                        Value = "دارد"
                    });
                }

                if (item.Value == "false")
                {
                    model.Configs.Add(new PurchaseConfigItemEmailModel
                    {
                        Title = item.PackageBaseConfig.Title,
                        Value = "ندارد"
                    });
                }

                if (item.Value != "true" && item.Value != "false")
                {
                    model.Configs.Add(new PurchaseConfigItemEmailModel
                    {
                        Title = item.PackageBaseConfig.Title,
                        Value = item.Value
                    });
                }

            }

            string body = Render.StringToString(EmailTemplate.PurchaseDetail, model).Replace("ی", "ي");

            Email.Add("Body", body);

            _mdb.Email.InsertOne(Email);

           // return true;
           return PublicService.SendEmail(new SimpleEmailModel { Id = Email.GetElement("Id").Value.ToString() });
        }  
    }
}
