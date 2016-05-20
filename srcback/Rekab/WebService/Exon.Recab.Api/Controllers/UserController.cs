using Exon.Recab.Api.Models.User;
using Exon.Recab.Service.Implement.User;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using System;
using System.Linq;
using Exon.Recab.Service.Implement.FeedBack;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Service.Implement.Payment;
using Exon.Recab.Api.Models.Advertise;
using Exon.Recab.Service.Implement.Email;

namespace Exon.Recab.Api.Controllers
{
    public class UserController : ApiController
    {

        #region init
        private readonly UserService _userService;
        private readonly DealershipService _DealershipService;
        private readonly FeedBackService _FeedBackService;
        private readonly PaymentService _PaymentService;
        public UserController()
        {
            _userService = new UserService();
            _DealershipService = new DealershipService();
            _FeedBackService = new FeedBackService();
            _PaymentService = new PaymentService();
            //_EmailService = new EmailService();
        }
        #endregion

        #region Security

        [HttpPost]
        public HttpResponseMessage SignIn(LoginViewModel model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _userService.SignIn(username: model.userName, password: model.password, isMobile: true).GetHttpResponse();

                case "api":
                    return _userService.SignIn(username: model.userName, password: model.password, isMobile: false).GetHttpResponse();

                default:
                    throw new RecabException(HttpStatusCode.NotFound);

            }

        }

        [HttpPost]
        public HttpResponseMessage SignUp(SingUpModel model)
        {
            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _userService.SignUp(username: model.email,
                                                password: model.password,
                                                fname: model.firstName,
                                                lname: model.lastName,
                                                mobile: model.mobile,
                                                type: (int)model.genderType,
                                                isMobile: true).GetHttpResponse();

                case "api":
                    return _userService.SignUp(username: model.email,
                                                 password: model.password,
                                                 fname: model.firstName,
                                                 lname: model.lastName,
                                                 mobile: model.mobile,
                                                 type: (int)model.genderType,
                                                 isMobile: false).GetHttpResponse();

                default:
                    break;


            }
            throw new Exception();

        }

        [HttpPost]
        public HttpResponseMessage GenerateVerificationCode(SendSmsModel model)
        {
            return _userService.SendSmsMobileVerification(mobileNum: model.mobile, type: (int)model.type).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage GenerateVerificationEmail(SendEmailModel model)
        {
            return _userService.SendVerificationEmail(email: model.email, type: (int)model.type).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage VerifyMobileNumber(VerifyMobileModel model)
        {
            return _userService.VerifyMobileCode(code: model.code, mobileNumber: model.mobileNum).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage VerifyEmailAddress(VerifyEmailModel model)
        {
            return _userService.VerifyEmailCode(email : model.email , code: model.code).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword(ChangePasswordModel model)
        {

            switch (Request.RequestUri.Segments[1].Replace("/", "").ToLower())
            {

                case "mobapi":
                    return _userService.ChangePassWord(userId: model.userId,
                                                oldpassword: model.oldPassword,
                                                newpassword: model.newPassword,
                                                repassword: model.rePassword,
                                                isMobile: true).GetHttpResponse();

                case "api":
                    return _userService.ChangePassWord(userId: model.userId,
                                                oldpassword: model.oldPassword,
                                                newpassword: model.newPassword,
                                                repassword: model.rePassword,
                                                isMobile: false).GetHttpResponse();

                default:
                    throw new RecabException(HttpStatusCode.NotFound);

            }


        }

        [HttpPost]
        public HttpResponseMessage ForgetPassword(ForgetPasswordModel model)
        {
            return _userService.ForgetPassword(userId: model.forgetUserId,

                                                newpassword: model.newPassword,
                                                repassword: model.rePassword).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage EditUser(EditUserModel model)
        {

            return _userService.EditUserInfo(userId: model.userId,
                                        fname: model.firstName,
                                        lname: model.lastName,
                                        email: model.email,
                                        type: (int)model.genderType).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage EditUserMobile(EditUserMobileModel model)
        {

            return _userService.EditUserMobile(userId: model.userId,
                                               code: model.code,
                                                mobile: model.mobile).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage UserDetail(UserDetailModel model)
        {
            return _userService.UserDetail(userId: model.userId).GetHttpResponse();
        }


        #endregion

        #region dealership

        #region add
        [HttpPost]
        public HttpResponseMessage AddDealership(AddDealershipModel model)
        {
            return _DealershipService.AddDelership(address: model.address,
                                             userId: model.userId,
                                             title: model.title,
                                             fax: model.fax,
                                             coordinateLat: model.lat,
                                             coordinateLong: model.lng,
                                             description: model.description,
                                             websiteUrl: model.websiteUrl,
                                             logoUrl: model.logoUrl,
                                             tell: model.tell,
                                             cityId: model.cityId,
                                             categoryId: model.categoryIds).GetHttpResponse();


        }

        #endregion

        #region   Report

        [HttpPost]
        public HttpResponseMessage ListDealership(UserDealershipFindModel model)
        {
            long count = 0;

            return _DealershipService.ListDelership(userId: model.userId,
                                                     status: (int)model.status,
                                                     categoryId: model.categoryId,
                                                     size: model.pageSize,
                                                     skip: model.pageIndex,
                                                     count: ref count).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage ListDealershipSimple(ListDealershipSimpleModel model)
        {
            long count = 0;
            var temp = _DealershipService.ListDelership(userId: model.userId,
                                                        status: 1,
                                                        categoryId: model.categoryId,
                                                        size: model.pageSize,
                                                        skip: model.pageIndex,
                                                        count: ref count);
            if (count > 0)
                return temp.Select(i => new
                {
                    dealershipId = i.id,
                    title = i.title
                }).ToList().GetHttpResponseWithCount(count);

            return temp.GetHttpResponseWithCount(0);
        }

        [HttpPost]
        public HttpResponseMessage DealershipDetail(DealershipDetailModel model)
        {
            return _DealershipService.DealershipDetail(delershipId: model.dealershipId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DealershipSearch(DealershipFindModel model)
        {
            long count = 0;
            return _DealershipService.SearchDelership(categoryId: model.categoryId,
                                                    cityId: model.cityId,
                                                    stateId: model.stateId,
                                                    title: model.title,
                                                    lng: model.lng,
                                                    lat: model.lat,
                                                    distance: model.distance,
                                                    size: model.pageSize,
                                                    skip: model.pageIndex,
                                                    count: ref count).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage DealershipAdvertise(DealershipAdvertiseModel model)
        {
            long count = 0;
            return _DealershipService.GetDealershipAdvertise(dealershipId: model.dealershipId,
                                                             categoryId: model.categoryId,
                                                             count: ref count,
                                                             size: model.pageSize,
                                                            skip: model.pageIndex).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage GetDealershipById(GetDealershipByIdModel model)
        {

            return _DealershipService.GetByID(model.dealershipid, userId: model.userId).GetHttpResponse();
        }


        #endregion

        #region edit
        [HttpPost]
        public HttpResponseMessage EditDealership(UserEditDealershipModel model)
        {
            return _DealershipService.UserEditDelership(userId: model.userId,
                                                        dealershipId: model.dealershipId,
                                                        title: model.title,
                                                        address: model.address,
                                                        tell: model.tell,
                                                        fax: model.fax,
                                                        coordinateLat: model.lat,
                                                        coordinateLong: model.lng,
                                                        description: model.description,
                                                        websiteUrl: model.websiteUrl,
                                                        logoUrl: model.logoUrl,
                                                        cityId: model.cityId,
                                                        categoryId: model.categoryIds).GetHttpResponse();
        }
        #endregion

        #endregion

        #region Package

        [HttpPost]
        public HttpResponseMessage ListPackage(UserListPakageModel model)
        {

            long count = 0;

            return _userService.ListUserPakage(userId: model.userId,
                                                 categoryId: model.categoryId,
                                                 isDealership: model.isDealership,
                                                 size: model.pageSize,
                                                 skip: model.pageIndex,
                                                 count: ref count).GetHttpResponseWithCount(count);

        }

        [HttpPost]
        public HttpResponseMessage UserAdminListPackage(UserAdminFindModel model)
        {

            long count = 0;

            return _userService.ListUserPakage(userId: model.cumUserId,
                                                 categoryId: model.categoryId,
                                                 isDealership: model.isDealership,
                                                 size: model.pageSize,
                                                 skip: model.skipPage,
                                                 count: ref count).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage AddUsePackageCredit(AddUserPackageCredit model)
        {
            return _userService.AddPackageCreditToUser(userId: model.userId, categoryPurchasePackageTypeId: model.packageTypeId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage PackageDetail(UserPackageDetailModel model)
        {
            return _userService.packageDetailConfig(model.userId, model.packageId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AdminPackageDetail(AdminPackageDetailModel model)
        {
            return _userService.packageDetailConfig(model.cumUserId, model.packageId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage AddCredit(AddCreditModel model)
        {
            return _PaymentService.InitPayment(Amount: model.amount, userId: model.userId, voucherCode: model.voucherCode).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage BuyPackage(BuyPackageModel model)
        {
            return _PaymentService.InitPayment(cpptId: model.cpptId,
                                               userId: model.userId,
                                               bancType: 1,
                                               voucherCode: model.voucherCode,
                                               productId: null).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage CreditDetail(UserCreditDetailModel model)
        {
            long count = 0;
            return _PaymentService.UserCreditDetail(fromPersianDate: model.fromPersianDate,
                                                    toPersianDate: model.toPersianDate,
                                                    userId: model.userId,
                                                    count: ref count, size: model.pageSize,
                                                    skip: model.pageIndex).GetHttpResponseWithCount(count);
        }
        #endregion

        #region Report
        [HttpPost]
        public HttpResponseMessage AdvertiseDetail(UserAdvertiseDetailModel model)
        {
            return _userService.ProductDetail(userId: model.userId, productId: model.advertiseId).GetHttpResponse();

        }

        [HttpPost]
        public HttpResponseMessage ListUserAdvertise(UserAllAdvertiseDetailModel model)
        {
            long count = 0;

            return _userService.ListProductForUserwithStatus(userId: model.userId,
                                                            count: ref count,
                                                            status: (int)model.status,
                                                            size: model.pageSize,
                                                            skip: model.pageIndex).GetHttpResponseWithCount(count);

        }


        [HttpPost]

        public HttpResponseMessage DealershipForLogin(UserDetailModel model)
        {

            return _userService.DealershipForLogin(userId: model.userId).GetHttpResponse();
        }
        #endregion

        #region Business

        [HttpPost]
        public HttpResponseMessage AddToFavorite(UserAdvertiseDetailModel model)
        {
            return _userService.AddToFavourite(userId: model.userId, prodouctId: model.advertiseId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage DeleteFavorite(DeleteFavoritFindModel model)
        {
            return _userService.DeleteFavourite(productId: model.advertiseId, userId: model.userId).GetHttpResponse();
        }


        [HttpPost]
        public HttpResponseMessage AddFeedback(AddFeedbackModel model)
        {

            return _FeedBackService.AddFeedback(userId: model.userId,
                                                productId: model.advertiseId,
                                                comment: model.comment,
                                                categoryFeatureTitle: model.categoryFeatureTitle).GetHttpResponse();
        }

        #endregion
    }
}
