using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.AsyncServerModel;
using Exon.Recab.Service.Model.EmailModel;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Model.SMSModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Exon.Recab.Service.Implement.Helper
{
    public static class PublicService
    {      
        public static bool ValidateCatagoryFeature(List<CategoryFeature> InsertList, List<ProductSelectItemModel> productItems)
        {
            int inputDistinctCount = productItems.Select(pi => pi.CategoryFeatureId).Distinct().Count();


            if (inputDistinctCount != productItems.Count)
                throw new RecabException((int)ExceptionType.SomeCategoryFeatureRedundant);

            int mustInsert = InsertList.Where(i => i.RequiredInADInsert).Count();


            if (productItems.Select(pi => pi.CategoryFeatureId).Distinct().Count() < mustInsert)
                throw new RecabException((int)ExceptionType.MissedInsertCategoryFeature);


            //validate item  and concat to cf
            var inputItems = InsertList.Join(productItems, i => i.Id, pi => pi.CategoryFeatureId, (i, pi) => new { CategoryFeature = i, ProductSelectItem = pi }).ToList();


            if (inputItems.Where(i => i.CategoryFeature.RequiredInADInsert).Count() != mustInsert)
                throw new RecabException((int)ExceptionType.MissedInsertCategoryFeature);

            // اگر تعداد آیتم های تایید شده با همه تعداد همه آیتم ها برابر نبود 
            if (inputItems.Count != inputDistinctCount)
                throw new RecabException((int)ExceptionType.InvalidInsertCategoryFeature);

            return true;
        }


        public static bool SendSms(string mobileNumber, string content)
        {
            AppSettingsReader settingsReader = new AppSettingsReader();
            string AsyncServerUrl = settingsReader.GetValue("AsyncServerURL", typeof(string)).ToString();

            JavaScriptSerializer js = new JavaScriptSerializer();

            SimpleSMSModel sms = new SimpleSMSModel
            {
                fromNumber = "30007654322346",
                content = content,
                toNumber = mobileNumber
            };


            SmsRequestModel model = new SmsRequestModel
            {
                data = js.Serialize(sms),
                type = SMSType.Simple
            };

            BaseRequestModel requestModel = new BaseRequestModel
            {
                data = js.Serialize(model),
                url = "SMS"
            };


            SocketSend(js.Serialize(requestModel));


            return true;
        }

        public static bool SendEmail(SimpleEmailModel model)
        {
            AppSettingsReader settingsReader = new AppSettingsReader();
            string AsyncServerUrl = settingsReader.GetValue("AsyncServerURL", typeof(string)).ToString();

            JavaScriptSerializer js = new JavaScriptSerializer();

            EmailRequestModel email = new EmailRequestModel
            {
                type = EmailType.MandrillSimple,
                data = js.Serialize(model)
            };

            BaseRequestModel requestModel = new BaseRequestModel
            {
                url = "Email",
                data = js.Serialize(email)
            };

            SocketSend(js.Serialize(requestModel));

            return true;
        }

        public static List<SelectItemModel> GetConfirmedItems(this List<SelectItemModel> selectedItems,
                                                               List<CategoryFeature> entityCategoryFeature,
                                                               List<CategoryFeature> reqiredCategoryFeature,
                                                               List<CategoryFeature> titleItems,
                                                               ref string title)
        {

            int inputDistinctCount = selectedItems.Select(pi => pi.CategoryFeatureId).Distinct().Count();

            if (selectedItems.Count() != inputDistinctCount)
                throw new RecabException((int)ExceptionType.SomeCategoryFeatureRedundant);

            #region CategoryFeature

            ///validate item  and concat to cf
            var ClientItems = entityCategoryFeature.Join(selectedItems,
                                                      i => i.Id,
                                                      pi => pi.CategoryFeatureId,
                                                      (i, pi) => new
                                                      {
                                                          CategoryFeature = i,
                                                          ProductSelectItem = pi
                                                      }).OrderBy(c => c.CategoryFeature.OrderId).ToList();

            /// اگر تعداد آیتم های تایید شده با همه تعداد همه آیتم ها برابر نبود 
            if (ClientItems.Count != inputDistinctCount)
                throw new RecabException((int)ExceptionType.InvalidInsertCategoryFeature);

            #endregion


            #region FeatureValue

            List<SelectItemModel> ConfirmedFeatureValues = new List<SelectItemModel>();

            List<long?> HideList = new List<long?>();

            foreach (var item in ClientItems)
            {
                #region  NO CUSTOM VALUE
                if (!item.CategoryFeature.HasCustomValue)
                {
                    ///اگر چیزی انتخاب نشده بود
                    if (item.ProductSelectItem.FeatureValueIds.Count == 0)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorNullId, item.CategoryFeature.Title);

                    ///اگر سینگل بود و چند موردانتخاب نشده بود
                    if (!item.CategoryFeature.HasMultiSelectValue && item.ProductSelectItem.FeatureValueIds.Count > 1)
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorWrongMulti, item.CategoryFeature.Title);

                    ///  اگر سینگل بود  واشتباه بود
                    if (!item.CategoryFeature.HasMultiSelectValue &&
                        item.ProductSelectItem.FeatureValueIds.Count == 1 &&
                        !item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == item.ProductSelectItem.FeatureValueIds.First()))

                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);


                    if (item.CategoryFeature.HasMultiSelectValue)
                    {
                        foreach (var temp in item.ProductSelectItem.FeatureValueIds)
                        {
                            /// اگر مالتی آیدی اشتباه انتخاب کرده بود
                            if (!item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == temp))
                                throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);
                        }
                    }


                }
                #endregion

                #region   CUSTOM VALUE
                else
                {
                    Regex regex = new Regex(item.CategoryFeature.Pattern, RegexOptions.IgnoreCase);

                    foreach (var seperet in item.ProductSelectItem.CustomValue.Split(','))
                    {
                        Match match = regex.Match(seperet ?? "");

                        ///فرمت دیتای وارد شده برابر نبود
                        if (!match.Success)
                            throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorRegex, item.CategoryFeature.Title);

                    }


                }
                #endregion

                #region DEPENDENCI

                ///بدون سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => entityCategoryFeature.Any(i => i.Id == cf.Id)).Count() == 0)
                {
                    ConfirmedFeatureValues.Add(new SelectItemModel
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        FeatureValueIds = !item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.FeatureValueIds.ToList() : new List<long>()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueIds)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }

                ///با سرشاخه
                if (item.CategoryFeature.ParentList.Where(cf => entityCategoryFeature.Any(i => i.Id == cf.Id)).Count() > 0)
                {
                    foreach (var par in item.CategoryFeature.ParentList.Where(cf => cf.CategoryFeature.AvailableInADS))
                    {
                        SelectItemModel parentItem = ConfirmedFeatureValues.FirstOrDefault(pfv => pfv.CategoryFeatureId == par.CategoryFeatureId);

                        if (parentItem == null || parentItem.FeatureValueIds.Count != 1)
                            throw new RecabException((int)ExceptionType.ParentCategoryNotFound);

                        FeatureValue parentFeatureValue = entityCategoryFeature.FirstOrDefault(cf => cf.Id == par.CategoryFeatureId)
                                                                            .FeatureValueList.FirstOrDefault(fv => fv.Id == parentItem.FeatureValueIds.FirstOrDefault());


                        foreach (var n in item.ProductSelectItem.FeatureValueIds)
                        {
                            if (!parentFeatureValue.ChildList.Any(child => child.FeatureValueId == n))
                                throw new RecabException();
                        }

                    }

                    ConfirmedFeatureValues.Add(new SelectItemModel
                    {
                        CategoryFeatureId = item.CategoryFeature.Id,
                        CustomValue = item.CategoryFeature.HasCustomValue ? item.ProductSelectItem.CustomValue : "",
                        FeatureValueIds = item.ProductSelectItem.FeatureValueIds.ToList()
                    });

                    foreach (var fvitem in item.ProductSelectItem.FeatureValueIds)
                    {

                        HideList = HideList.Concat(item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == fvitem).HideList.Select(h => h.CategoryFeatureHideId).ToList()).ToList();
                    }

                }
                #endregion

                if (titleItems.Any(t => t.Id == item.CategoryFeature.Id))
                {
                    if (item.CategoryFeature.HasCustomValue)
                    {
                        title = title + " " + item.ProductSelectItem.CustomValue;
                    }
                    else
                    {
                        var feature = item.CategoryFeature.FeatureValueList.FirstOrDefault(fv => fv.Id == item.ProductSelectItem.FeatureValueIds.FirstOrDefault());
                        title = title + " " + feature.Title;
                    }
                }

            }

            #region REQIRED
            List<CategoryFeature> RequiredCategoryFeature = reqiredCategoryFeature.Where(cf => !HideList.Any(h => h.Value == cf.Id)).ToList();

            if (RequiredCategoryFeature.Count != selectedItems.Join(RequiredCategoryFeature, p => p.CategoryFeatureId, r => r.Id, (p, r) => p).Count())
                throw new RecabException((int)ExceptionType.MissedInsertCategoryFeature);
            #endregion

            #endregion

            return ConfirmedFeatureValues;
        }

        public static List<CFResultViewModel> GetEntityAssingeItems(long id, EntityType type, ref SdbContext _sdb)
        {
            List<FeatureValueAssign> AssignedItems = _sdb.FeatureValueAssign.Where(FVA => FVA.EntityId == id && FVA.EntityType == type).ToList();

            List<CFResultViewModel> model = new List<CFResultViewModel>();

            foreach (var item in AssignedItems)
            {
                CFResultViewModel Result = new CFResultViewModel
                {
                    categoryFeatureId = item.CategoryFeatureId,
                    title = item.CategoryFeature.Title,
                    customValue = item.CustomValue ?? ""
                };

                foreach (var featureItem in item.ListFeatureValue)
                {
                    Result.featureValues.Add(new FVResultViewModel
                    {
                        featureValueId = featureItem.FeatureValueId,
                        title = featureItem.FeatureValue.Title
                    });
                }

                model.Add(Result);

            }

            return model;

        }


        private static string SocketSend(string data)
        {

            data = "HTTP/1.1 200 OK\r\n" +
                   "Server:Microsoft-IIS/8.5\r\n" +
                   "Content-Length:1\r\n" +
                    "\r\n" + data;

            IPEndPoint ServerEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.14"), 7579);

            var client = new Socket(addressFamily: AddressFamily.InterNetwork, socketType: SocketType.Stream, protocolType: ProtocolType.Tcp);

            client.Connect(IPAddress.Parse("192.168.1.14"), 7579);

            client.Send(System.Text.Encoding.UTF8.GetBytes(data));

            byte[] buffer = new byte[client.ReceiveBufferSize];

            int read = 0;
            while (true)
            {

                if (client.Connected)
                {
                    read = client.Receive(buffer);

                    if (read > 0)
                    {
                        string pureRequest = Encoding.UTF8.GetString(buffer);

                        client.Close();
                        return pureRequest;
                    }
                }
            }

            return "";
        }

    }

}
