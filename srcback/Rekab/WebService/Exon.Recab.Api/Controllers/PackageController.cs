using Exon.Recab.Api.Models.Package;
using Exon.Recab.Service.Implement.Package;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using Exon.Recab.Service.Model.PackageModel;
using Exon.Recab.Api.Models.AdminManageModel;
using Exon.Recab.Api.Infrastructure.Filter;

namespace Exon.Recab.Api.Controllers
{
    public class PackageController : ApiController
    {
        private readonly PackageService _packageService;

        private readonly VoucherService _VoucherService;

        public PackageController()
        {
            this._packageService = new PackageService();
            this._VoucherService = new VoucherService();
        }

        #region ADD

        [HttpPost]
        public HttpResponseMessage AddPurchaseType(AddPurchaseTypeModel model)
        {

            return _packageService.AddPurchaseType(model.title, free: model.isFree, dealership: model.isDealership , logoUrl : model.logoUrl).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddPackageType(AddPackageTypeModel model)
        {

            return _packageService.AddPackageType(model.title).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddCategoryPurchaseType(AddCategoryPurchaseTypeModel model)
        {

            return _packageService.AddCategoryToPurchaseType(categoryId: model.categoryId, PurchesTypeId: model.purchesTypeId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddCategoryPurchasePackageType(AddCategoryPurchasePackageTypeModel model)
        {

            return _packageService.AddPackageTypeToCategoryPurchaseType(categoryPurchaseTypeId: model.categoryPurchaseTypeId,
                                                                        packageTypeId: model.packageTypeId,
                                                                        order: model.order).GetHttpResponse();
        }

        #endregion


        #region PurchaseType

        [HttpPost]
        public HttpResponseMessage ListPurchaseType(SimpleFindModel model)
        {
            long count = 0;
            return _packageService.ListPurchaseType(count: ref count, size: model.pageSize, skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        #endregion


        #region CategoryPurchaseType

        [HttpPost]
        public HttpResponseMessage ListCategoryPurchaseType(PurchaseTypeFindModel model)
        {
            long count = 0;
            return _packageService.ListCategoryForPurchaseType(purchaseTypeId: model.purchaseTypeId,
                                                                count: ref count, size: model.pageSize,
                                                                skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        #endregion


        #region PackageType
        [HttpPost]
        public HttpResponseMessage ListPackageType(SimpleFindModel model)
        {
            long count = 0;
            return _packageService.ListPackageTypes(count: ref count, size: model.pageSize, skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        #endregion


        #region CategoryPurchasePackageType

        [HttpPost]
        public HttpResponseMessage ListCategoryPurchasePackageType(CategoryPurchaseTypeFindModel model)
        {
            long count = 0;
            return _packageService.ListCategoryPurchasePackageType(categoryPurchaseTypeId: model.categoryPurchaseTypeId,
                                                                count: ref count,
                                                                size: model.pageSize,
                                                                skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        #endregion


        #region   Edit

        [HttpPost]
        public HttpResponseMessage EditCategoryPurchasePackageTypeConfig(EditCategoryPurchasePackageTypeConfigModel model)
        {
            List<CpptEditConfigItemModel> configItems = new List<CpptEditConfigItemModel>();

            foreach (var item in model.configItems)
            {
                configItems.Add(new CpptEditConfigItemModel
                {
                    ConfigTitle = item.configTitle,
                    Value = item.value
                });
            }


            return _packageService.EditCpptConfig(configItems: configItems,
                                                    cpptId: model.categoryPurchasePackageTypeId).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage EditPurchaseType(EditPurchaseTypeModel model )
        {
            return _packageService.EditPurchaseType(purchaseTypeId: model.purchaseTypeId,
                                                    title: model.title,
                                                    logoUrl: model.logoUrl).GetHttpResponse();
        }
        #endregion


        #region Report

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage ListPackageBuy(PackageForBuyModel model)
        {
            return _packageService.ListBuyPackage(userId: model.userId, categoryId: model.categoryId, isDealership: model.isDealership).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListPurchasPackageTypeDetail(PurchasPackageTypeDetail model)
        {
            return _packageService.ListPurchasPackageTypeDetail(model.categoryPurchaseId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage CreateVoucher(AddVoucherModel model)
        {


            return _VoucherService.AddVoucherConfigAndVocher(userId: model.userId,
                                                            title: model.title,
                                                             description: model.description,
                                                             count: model.count,
                                                             fromDate: model.fromDate,
                                                             fromTime: model.fromTime,
                                                             toDate: model.toDate,
                                                             toTime: model.toTime,
                                                             value: model.value,
                                                             type: (int)model.type).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ListVoucherConfig(VoucherConfigSearchModel model)
        {
            long count = 0;


            return _VoucherService.ListVocherConfig(status: (int)model.status,
                                                    count: ref count,
                                                    fromDate: model.fromDate,
                                                    toDate: model.toDate,
                                                    size: model.pageSize,
                                                    skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ListVoucherByVoucherConfig(VoucherFindModel model)
        {
            long count = 0;

            return _VoucherService.ListVoucher(voucherConfigId: model.voucherConfigId,
                                               responseCode: model.responceCode,
                                               count: ref count,
                                               size: model.pageSize,
                                               skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage SearchVoucher(SearchVoucherModel model)
        {
            long count = 0;

            return _VoucherService.VoucherSearch(voucherConfigTitle: model.title,
                                                 fromPersianDate: model.fromPersianDate,
                                                 toPersianDate: model.toPersianDate,
                                                 responseCode: model.responseCode,
                                                 status: (int)model.status,
                                                 count: ref count,
                                                 size: model.pageSize,
                                                 skip: model.pageIndex).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage CategoryPurchasePackageTypeConfig(CategoryPurchasePackageTypeConfigModel model)
        {
            return _packageService.ListConfigForCPPT(cpptId: model.categoryPurchasePackageTypeId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage VoucherValidate(VoucherValidateModel model)
        {
            return _VoucherService.ValidateVoucher(code: model.code, cpptId: model.categoryPurchasePackageTypeId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage CreditVoucherValidate(CreditVoucherValidateModel model)
        {
            return _VoucherService.ValidateVoucherForCredit(code: model.code).GetHttpResponse();
        }

        [HttpPost]
        [CacheEnable("1:0:0")]
        public HttpResponseMessage TariffList(BillViewModel model)
        {

            return _packageService.ListPurchaseTypeWithDetail(categoryId: model.categoryId).GetHttpResponse();

        }

        #endregion


        #region delete
        [HttpPost]
        public HttpResponseMessage DeletePurchaseType(DeleteModel model)
        {
            return _packageService.DeletePurchaseType(purchaseTypeId: model.id).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryPurchaseType(DeleteModel model)
        {
            return _packageService.DeleteCategoryPurchaseType(categoryPurchaseTypeId: model.id).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteCategoryPurchasePackageType(DeleteModel model)
        {
            return _packageService.DeleteCategoryPurchasePackageType(cpptId: model.id).GetHttpResponse();
        }

        #endregion
    }
}
