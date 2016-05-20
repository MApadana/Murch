using Exon.Recab.Domain.SqlServer;
using System.Linq;
using System.Net;
using Exon.Recab.Domain.Entity;
using System;
using Exon.Recab.Service.Model.UserModel;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Domain.Constant.User;
using System.Collections.Generic;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Service.Resource;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Domain.Constant.Media;
using System.Security.Cryptography;
using System.Text;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Infrastructure.Utility.Security;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Public;
using Exon.Recab.Domain.Constant.SMS;
using MongoDB.Bson;
using Exon.Recab.Service.Implement.Email;
using Exon.Recab.Domain.MongoDb;
using MongoDB.Driver;
using Exon.Recab.Service.Implement.Helper;

namespace Exon.Recab.Service.Implement.User
{
    public class UserService
    {
        #region Init
        private SdbContext _sdb;
        private MdbContext _mdb;
        private readonly RoleManagementService _roleManagementService;
        private readonly RecabSystemConfig RecabSystemConfig;

        internal UserService(ref SdbContext sdb)
        {
            _sdb = sdb;
            _mdb = new MdbContext();
            _roleManagementService = new RoleManagementService(ref sdb);

        }

        public UserService()
        {
            _sdb = new SdbContext();
            _roleManagementService = new RoleManagementService(ref _sdb);
            _mdb = new MdbContext();

            RecabSystemConfig = _sdb.RecabSystemConfig.FirstOrDefault();
        }
        #endregion

        #region Add

        public object SignUp(string username,
                            string password,
                            string fname,
                            string lname,
                            string mobile,
                            int type,
                            bool isMobile ,
                            bool isdelership = false)
        {

            if (_sdb.Users.Any(u => u.Email == username || u.Mobile == mobile))
                throw new RecabException((int)ExceptionType.UserAlreadyExist);

            MD5 Hash = MD5.Create();

            Domain.Entity.User user = new Domain.Entity.User
            {
                Status = UserStatus.Deactive,
                LastLoginRequest = DateTime.UtcNow,
                LastSuccessLogin = DateTime.UtcNow,
                UnsuccessTryCount = 0,
                Email = username,
                Password = "test",
                FirstName = fname,
                LastName = lname,
                Mobile = mobile.StartsWith("0") ? "98" + mobile.Remove(0, 1) : "98" + mobile,
                MobileVerified = isMobile,
                EmailVerified = false,
                GenderType = (UserGender)type
            };

            _sdb.Users.Add(user);
            _sdb.SaveChanges();

            user.Password = Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(password + user.Id)));

            _sdb.SaveChanges();

            if (RecabSystemConfig.NewUesrVoucher > 0)
            {
                _sdb.Credits.Add(new Credit
                {
                    UserId = user.Id,
                    InsertTime = DateTime.UtcNow,
                    Description = "اعتبار هدیه برای ثبت نام",
                    Amount = RecabSystemConfig.NewUesrVoucher
                });

            }
            EmailService Emailservice = new EmailService(ref _sdb, ref _mdb);
            Emailservice.SendWellcome(user);


            return this.SignIn(username: user.Email, password: password, isMobile: isMobile, AddDealership: true);

        }


        public bool AddUser(string username,
                            string password,
                            string fname,
                            string lname,
                            string mobile,
                            int type)
        {
            if (_sdb.Users.Any(u => u.Email == username || u.Mobile == mobile))
                throw new RecabException((int)ExceptionType.UserAlreadyExist);

            MD5 Hash = MD5.Create();

            Domain.Entity.User user = new Domain.Entity.User
            {
                Status = UserStatus.Deactive,
                LastLoginRequest = DateTime.UtcNow,
                LastSuccessLogin = DateTime.UtcNow,
                UnsuccessTryCount = 0,
                Email = username,
                Password = "test",
                FirstName = fname,
                LastName = lname,
                Mobile = mobile,
                GenderType = (UserGender)type
            };

            _sdb.Users.Add(user);
            _sdb.SaveChanges();

            user.Password = Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(password + user.Id)));

            _sdb.SaveChanges();

            return true;

        }

        public bool SignUpByRole(string username,
                                 string password,
                                 string fname,
                                 string lname,
                                 string mobile,
                                 int type,
                                 long roleId)
        {
            if (_sdb.Users.Any(u => u.Email == username || u.Mobile == mobile))
                throw new RecabException((int)ExceptionType.UserAlreadyExist);

            MD5 Hash = MD5.Create();

            var user = new Domain.Entity.User
            {
                Status = UserStatus.Deactive,
                LastLoginRequest = DateTime.UtcNow,
                LastSuccessLogin = DateTime.UtcNow,
                UnsuccessTryCount = 0,
                Email = username,
                Password = Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(password + username))),
                FirstName = fname,
                LastName = lname,
                Mobile = mobile,
                GenderType = (UserGender)type
            };

            _sdb.Users.Add(user);
            _sdb.SaveChanges();

            return _roleManagementService.AddRoleToUser(new List<long> { roleId }, user.Id);
        }


        public bool AddPackageCreditToUser(long userId, long categoryPurchasePackageTypeId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            CategoryPurchasePackageType packageType = _sdb.CategoryPurchasePackageTypes.Find(categoryPurchasePackageTypeId);

            if (packageType == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFound);

            List<PurchaseConfig> listPurchaseConfig = _sdb.PurchaseConfig.Where(pc => pc.CategoryPurchasePackageTypeId == packageType.Id).ToList();

            PackageBaseConfig qoutaItem = _sdb.PackageBaseConfig.FirstOrDefault(pbc => pbc.Title == PackageConfig.Quota);

            if (listPurchaseConfig.Count == 0 || qoutaItem == null)
                throw new RecabException("database config error for qouta", HttpStatusCode.BadRequest);

            PurchaseConfig configQouta = listPurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfigId == qoutaItem.Id);

            if (configQouta == null)
                throw new RecabException("database config error for qouta", HttpStatusCode.BadRequest);


            user.UserPackageCredits.Add(new UserPackageCredit
            {
                CategoryPurchasePackageTypeId = packageType.Id,
                BaseQuota = Convert.ToInt64(configQouta.Value),
                UsedQuota = 0,
                InsertTime = DateTime.UtcNow,
                Status = UserCreditStatus.فعال
            });

            _sdb.SaveChanges();

            return true;

        }



        public bool AddToFavourite(long userId, long prodouctId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            Product Product = _sdb.Product.Find(prodouctId);

            if (Product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            if (_sdb.FavouriteProduct.Any(fp => fp.ProductId == prodouctId && fp.UserId == userId))
                throw new RecabException((int)ExceptionType.FavouriteProductExist);

            _sdb.FavouriteProduct.Add(new FavouriteProduct { UserId = user.Id, ProductId = Product.Id });

            _sdb.SaveChanges();

            return true;

        }


        public bool DeleteFavourite(long productId, long userId)
        {
            FavouriteProduct favouriteProduct = _sdb.FavouriteProduct.FirstOrDefault(fp => fp.ProductId == productId && fp.UserId == userId);

            if (favouriteProduct == null)
                throw new RecabException((int)ExceptionType.FavouriteNotFound);

            _sdb.FavouriteProduct.Remove(favouriteProduct);


            _sdb.SaveChanges();


            return true;
        }

        #endregion

        #region Report

        public List<UserPakageViewModel> ListUserPakage(long userId,
                                                        long categoryId,
                                                        bool isDealership,
                                                        ref long count,
                                                        int size = 1,
                                                        int skip = 0)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            var temp = _sdb.UserPackageCredits.Where(upc => upc.UserId == userId && upc.ExpireDate > DateTime.UtcNow && upc.UsedQuota > 0)
                                              .Join(_sdb.CategoryPurchasePackageTypes.Where(cppt => cppt.CategoryPurchaseType.PurchaseType.AvailableDealership == isDealership),
                                                    upc => upc.CategoryPurchasePackageTypeId,
                                                    cppt => cppt.Id,
                                                    (upc, cppt) => new
                                                    {
                                                        Id = upc.Id,
                                                        CategoryId = cppt.CategoryPurchaseType.CategoryId,
                                                        type1 = cppt.CategoryPurchaseType.PurchaseType.Title,
                                                        tyep2 = cppt.CategoryPurchaseType.Category.Title,
                                                        type3 = cppt.PackageType.Title,
                                                        count = upc.BaseQuota - upc.UsedQuota
                                                    }).ToList();

            temp = temp.Where(t => t.CategoryId == categoryId).ToList();

            if (temp.Count == 0)
                return new List<UserPakageViewModel>();


            return temp.Select(t => new UserPakageViewModel
            {
                pakageType = string.Format("{0}-{1}-{2}", t.type1, t.tyep2, t.type3),
                id = t.Id,
                count = t.count
            }).ToList();
        }

        public PackageDetailViewModel packageDetailConfig(long userId, long userPackageCreditId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            UserPackageCredit creditPackage = user.UserPackageCredits.Find(c => c.Id == userPackageCreditId);
            if (creditPackage == null)
                throw new RecabException((int)ExceptionType.UserPackageCreditNotFound);

            PackageDetailViewModel newDetail = new PackageDetailViewModel();
            newDetail.baseCount = creditPackage.BaseQuota;
            newDetail.usedCount = creditPackage.UsedQuota;
            newDetail.logoUrl = creditPackage.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.LogoUrl;

            foreach (var item in creditPackage.CategoryPurchasePackageType.PurchaseConfig)
            {

                newDetail.configs.Add(new PackageConfigDetailViewModel
                {
                    configTitle = item.PackageBaseConfig.Title,
                    configValue = item.Value
                });
            }
            return newDetail;
        }


        public UserProcuctDetailModel ProductDetail(long userId, long productId)
        {

            Product product = _sdb.Product.FirstOrDefault(p => p.Id == productId && p.UserId == userId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            UserProcuctDetailModel model = new UserProcuctDetailModel();

            model.confirmDate = product.ConfirmDate.HasValue ? product.ConfirmDate.Value.UTCToPersianDateLong() : "تایید نشده";

            model.insertDate = product.InsertDate.UTCToPersianDateLong();

            model.email = product.User.Email;

            model.description = product.Description;

            model.adminComment = product.AdminComment ?? "";

            model.tell = product.Tell ?? "";

            model.Id = product.Id;

            model.userName = product.User.FirstName + " " + product.User.LastName;

            if (_sdb.Media.Any(m => m.EntityType == EntityType.Product && m.EntityId == product.Id))
                model.mediaUrl = _sdb.Media.Where(m => m.EntityType == EntityType.Product && m.EntityId == product.Id).OrderBy(m => m.Order).Select(m => m.MediaURL).ToList();

            product.ProductFeatures = product.ProductFeatures.OrderBy(pf => pf.CategoryFeature.OrderId).ToList();
            model.categoryFeature = product.ProductFeatures.Select(pcf => new UserProductCFDetailViewModel
            {
                title = pcf.CategoryFeature.Title,
                categoryfeatureId = pcf.Id,
                featureValue = pcf.ListFeatureValue.Select(pfv => new UserProductFVDetailViewModel
                {
                    featureValueId = pfv.FeatureValueId,
                    title = pfv.FeatureValue.Title
                }).ToList(),
                customValue = pcf.CustomValue != "" ? pcf.CustomValue.ToString() : ""


            }).ToList();


            return model;


        }


        public List<UserProductViewModel> ListProductForUser(long userId,
                                                             ref long count,
                                                             int size = 1,
                                                             int skip = 0)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<Product> userProducts = _sdb.Product.Where(p => p.UserId == user.Id).ToList();

            count = userProducts.Count;
            if (count == 0)
                return new List<UserProductViewModel>();

            List<UserProductViewModel> model = new List<UserProductViewModel>();
            var temp = userProducts.OrderBy(c => c.InsertDate).Skip(size * skip).Take(size).ToList();

            foreach (var p in temp)
            {
                string ADTitle = "";

                var collection = p.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle);

                foreach (var item in collection)
                {
                    ADTitle = ADTitle + " " + item.ListFeatureValue.First().FeatureValue.Title;
                }

                model.Add(new UserProductViewModel
                {
                    advertiseId = p.Id,
                    description = p.Description,
                    imageUrl = _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product) != null ? _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product).MediaURL : "",
                    status = (int)p.Status,
                    dealershipId = p.DealershipId,
                    exchangeCategoryId = p.CategoryExchangeId,
                    title = ADTitle,
                    updateAble = p.UpdateCount > 0,
                    updateCount = p.UpdateCount,
                    mediaMaxCount = PackageMediaCount(p.Id),
                    insertDate = p.InsertDate.UTCToPersianDateLong(),
                    confirmDate = p.ConfirmDate.HasValue ? p.ConfirmDate.Value.UTCToPersianDateLong() : "تایید نشده",
                    packageTitle = p.UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.Title + "-" +
                                   p.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title,

                    packageId = p.UserPackageCreditId.Value,
                    categoryTitle = p.Category.Title,
                    webVisitCount = p.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title == Resource.PackageConfig.Gold ? p.WebVisitCount : -1,
                    mobileVisitCount = p.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title == Resource.PackageConfig.Gold ? p.AndroidVisitCount + p.IosVisitCount : -1

                });
            }

            return model;
        }

        public List<UserProductViewModel> ListProductForUserwithStatus(long userId,
                                                                       ref long count,
                                                                       int status,
                                                                       int size = 1,
                                                                       int skip = 0)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<Product> userProducts = _sdb.Product.Where(p => p.UserId == user.Id && p.Status == (ProdoctStatus)status).ToList();

            count = userProducts.Count;
            if (count == 0)
                return new List<UserProductViewModel>();

            List<UserProductViewModel> model = new List<UserProductViewModel>();
            var temp = userProducts.OrderBy(c => c.InsertDate).Skip(size * skip).Take(size).ToList();

            foreach (var p in temp)
            {
                string ADTitle = "";

                var collection = p.ProductFeatures.Where(cf => cf.CategoryFeature.AvailableInTitle);

                foreach (var item in collection)
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

                model.Add(new UserProductViewModel
                {
                    advertiseId = p.Id,
                    description = p.Description,
                    status = (int)p.Status,
                    dealershipId = p.DealershipId,
                    exchangeCategoryId = p.CategoryExchangeId,
                    title = ADTitle,
                    updateAble = p.UpdateCount > 0,
                    updateCount = p.UpdateCount,
                    mediaMaxCount = PackageMediaCount(p.Id),
                    insertDate = p.InsertDate.UTCToPersianDateLong(),
                    confirmDate = p.ConfirmDate.HasValue ? p.ConfirmDate.Value.UTCToPersianDateLong() : "تایید نشده",
                    packageTitle = p.UserPackageCredit.CategoryPurchasePackageType.CategoryPurchaseType.PurchaseType.Title + "-" +
                                   p.UserPackageCredit.CategoryPurchasePackageType.PackageType.Title,

                    packageId = p.UserPackageCreditId.Value,
                    categoryTitle = p.Category.Title,
                    categoryId = p.CategoryId,

                    imageUrl = _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product) != null ?
                               _sdb.Media.FirstOrDefault(m => m.EntityId == p.Id && m.EntityType == EntityType.Product).MediaURL : ""


                });

            }

            return model;
        }
        #endregion

        #region   business

        public object SignIn(string username, string password, bool isMobile, bool AddDealership = false)
        {
            Domain.Entity.User user = _sdb.Users.FirstOrDefault(c => c.Mobile == (username.StartsWith("0") ? "98" + username.Remove(0, 1) : "98" + username) || c.Email == username);

            LoginStatus status = LoginStatus.معمولی;

            if (user == null)
                throw new RecabException((int)ExceptionType.WrongUserNameORPassword);

            if (user.Email.ToLower() == username.ToLower())
            {

                if (!AddDealership)
                {
                    if (!user.EmailVerified)
                        throw new RecabException((int)ExceptionType.UserEmailNotActive);
                }

                else
                {
                    if (user.Dealerships.Count > 0)
                    {
                        if (user.UserTokens.Count == 1)
                            status = LoginStatus.اولین_بار_نمایشگاه;
                    }
                    else
                    {
                        if (user.UserTokens.Count == 1)
                            status = LoginStatus.اولین_بار_کاربر;
                    }

                }
            }
            else
            {
                if (user.Dealerships.Count > 0)
                {
                    if (user.UserTokens.Count == 1)
                        status = LoginStatus.اولین_بار_نمایشگاه;
                }
                else
                {
                    if (user.UserTokens.Count == 1)
                        status = LoginStatus.اولین_بار_کاربر;
                }
            }



            MD5 Hash = MD5.Create();

            if (user.Password != Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(password + user.Id))))
                throw new RecabException((int)ExceptionType.WrongUserNameORPassword);

            UserToken userToken = new UserToken
            {
                Token = CodeHelper.NewToken(),
                InsertTime = DateTime.UtcNow,
                TokenType = TokenType.General,
                ClientType = isMobile ? ClientType.Android : ClientType.WebUI,
                LastUsedTime = DateTime.UtcNow,
                Available = true ,
               
            };

            user.UserTokens.Add(userToken);

            _sdb.SaveChanges();

            if (isMobile)
                return new MobileLogInModel
                {
                    id = userToken.Token,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    credit = user.Credit.Sum(c => c.Amount),
                    status = status,
                    isDealership = user.Dealerships.Count() > 0 ,
                    activeDealership = user.Dealerships.Where(d=>d.Status == DealershipStatus.فعال) .Count()  >0
                };


            return new WebLoginModel
            {
                id = userToken.Token,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                credit = user.Credit.Sum(c => c.Amount),
                status = status ,
                isDealership = user.Dealerships.Count() > 0,
                activeDealership = user.Dealerships.Where(d => d.Status == DealershipStatus.فعال).Count()>0
            };

        }

        public bool SendSmsMobileVerification(string mobileNum, int type)
        {
            mobileNum = mobileNum.Replace("-", "");

            mobileNum = mobileNum.StartsWith("0") ? "98" + mobileNum.Remove(0, 1) : "98" + mobileNum;

            switch ((SMSType)type)
            {
                case SMSType.NewUser:
                    if (_sdb.Users.Any(c => c.Mobile == mobileNum))
                        throw new RecabException((int)ExceptionType.RedundantMobileNumber);
                    break;

                case SMSType.MobileVerify:
                    if (_sdb.Users.Any(c => c.Mobile == mobileNum))
                        throw new RecabException((int)ExceptionType.RedundantMobileNumber);
                    break;
                case SMSType.ForgetPassword:
                    if (!_sdb.Users.Any(c => c.Mobile == mobileNum))
                        throw new RecabException((int)ExceptionType.UserNotExist);
                    break;

                default:
                    throw new RecabException((int)ExceptionType.ModelStateInvalid);
            }


            SMS sms = new SMS
            {
                MobileNumber = mobileNum,
                Content = CodeHelper.NewVerification(),
                SendDate = DateTime.UtcNow,
                Type = (SMSType)type

            };
            _sdb.SMS.Add(sms);

            _sdb.SaveChanges();

            return PublicService.SendSms(mobileNumber: sms.MobileNumber, content: sms.Content);

            // return true;
        }

        public bool SendVerificationEmail(string email, int type)
        {
            Domain.Entity.User user = _sdb.Users.FirstOrDefault(c => c.Email == email);

            if (user == null)
                throw new RecabException((int)ExceptionType.RedundantUserNewsEmail);

            EmailService emailService = new EmailService(ref _sdb, ref _mdb);

            emailService.SendVerificationEmail(user: user, type: (int)type);
            return true;
        }

        public VerifyMobileCodeViewModel VerifyMobileCode(string code, string mobileNumber)
        {
            mobileNumber = mobileNumber.Replace("-", "");

            mobileNumber = mobileNumber.StartsWith("0") ? "98" + mobileNumber.Remove(0, 1) : "98" + mobileNumber;

            DateTime time = DateTime.UtcNow.AddMinutes(6);

            SMS sms = _sdb.SMS.FirstOrDefault(s => s.MobileNumber == mobileNumber && s.Content == code && s.SendDate < time);

            if (sms == null)
                throw new RecabException((int)ExceptionType.MobileCodeInValid);

            VerifyMobileCodeViewModel model = new VerifyMobileCodeViewModel
            {
                mobileVerified = true,
                userExist = _sdb.Users.Any(u => u.Mobile == mobileNumber)
            };

            switch (sms.Type)
            {
                case SMSType.NewUser:
                case SMSType.MobileVerify:
                    return model;

                case SMSType.ForgetPassword:

                    Domain.Entity.User user = _sdb.Users.FirstOrDefault(u => u.Mobile == sms.MobileNumber);

                    UserToken userToken = new UserToken
                    {
                        Token = CodeHelper.NewToken(),
                        InsertTime = DateTime.UtcNow,
                        TokenType = TokenType.ForgetPassword,
                        LastUsedTime = DateTime.UtcNow,
                        ClientType = ClientType.Android,
                        Available = true

                    };

                    user.UserTokens.Add(userToken);

                    _sdb.SaveChanges();

                    model.id = userToken.Token;

                    return model;

                default:
                    throw new RecabException((int)ExceptionType.InternalError);
            }


        }

        public VerifyEmailCodeViewModel VerifyEmailCode(string email, string code)
        {
            BsonArray Filter = new BsonArray();
            Filter.Add(new BsonDocument { { "Email", email.ToLower() } });
            Filter.Add(new BsonDocument { { "Code", code } });

            List<BsonDocument> Result = _mdb.Email.Find(filter: new BsonDocument { { "$and", Filter } }).ToList();

            if (Result.Count == 0)
                return new VerifyEmailCodeViewModel { emailVerified = false, userExist = false, id = "" };

            if (Result.Count > 1)
                return new VerifyEmailCodeViewModel { emailVerified = false, userExist = false, id = "" };

            BsonDocument Email = Result.FirstOrDefault();

            DateTime EmailTime = Convert.ToDateTime(Email.GetElement("InsertDate").Value.ToString());

            if (EmailTime.AddMinutes(60) < DateTime.UtcNow)
                return new VerifyEmailCodeViewModel { emailVerified = false, userExist = false, id = "" };

            switch ((SMSType)Convert.ToInt32(Email.GetElement("VerifyType").Value.ToString()))
            {

                case SMSType.MobileVerify:
                case SMSType.NewUser:

                    string g = Email.GetElement("Email").Value.ToString().ToLower();
                    Domain.Entity.User user = _sdb.Users.FirstOrDefault(u => u.Email.ToLower() == g);
                    if (user == null)
                        throw new RecabException((int)ExceptionType.InternalError);

                    if (user.Dealerships.Count() > 0)
                    {
                        user.UserRoles.Add(new UserRole { RoleId = 4, Status = RoleStatus.Active });
                    }
                    else
                    {
                        user.UserRoles.Add(new UserRole { RoleId = 3, Status = RoleStatus.Active });
                    }


                    UserToken iioken = new UserToken
                    {
                        Token = CodeHelper.NewToken(),
                        InsertTime = DateTime.UtcNow,
                        TokenType = TokenType.General,
                        LastUsedTime = DateTime.UtcNow,
                        ClientType = ClientType.Android,
                        Available = true

                    };

                    user.Credit.FirstOrDefault().Amount = RecabSystemConfig.NewDealershipVoucher;

                    user.UserTokens.Add(iioken);

                    _sdb.SaveChanges();

                    user.EmailVerified = true;
                    _sdb.SaveChanges();

                    return new VerifyEmailCodeViewModel
                    {
                        emailVerified = true,
                        userExist = true,
                        id = iioken.Token,
                        user = new WebLoginModel
                        {
                            id = iioken.Token,
                            firstName = user.FirstName,
                            lastName = user.LastName,
                            email = user.Email,
                            credit = user.Credit.Sum(c => c.Amount),
                            status = user.Dealerships.Count() > 0 ? LoginStatus.اولین_بار_نمایشگاه : LoginStatus.اولین_بار_کاربر,
                            isDealership = user.Dealerships.Count() > 0,
                            activeDealership = user.Dealerships.Where(d => d.Status == DealershipStatus.فعال).Count()  >0

                        }
                    };

                case SMSType.ForgetPassword:
                    Domain.Entity.User userforget = _sdb.Users.ToList().FirstOrDefault(u => u.Email.ToLower() == Email.GetElement("Email").Value.ToString().ToLower());
                    if (userforget == null)
                        throw new RecabException((int)ExceptionType.InternalError);

                    VerifyEmailCodeViewModel model = new VerifyEmailCodeViewModel
                    {
                        emailVerified = true,
                        userExist = true
                    };
                    UserToken userToken = new UserToken
                    {
                        Token = CodeHelper.NewToken(),
                        InsertTime = DateTime.UtcNow,
                        TokenType = TokenType.ForgetPassword,
                        LastUsedTime = DateTime.UtcNow,
                        ClientType = ClientType.Android,
                        Available = true

                    };

                    userforget.UserTokens.Add(userToken);

                    _sdb.SaveChanges();

                    model.id = userToken.Token;

                    return model;

                default:
                    throw new RecabException((int)ExceptionType.InternalError);


            }


        }

        public string PackageMediaCount(long productId)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            var conf = product.UserPackageCredit.CategoryPurchasePackageType.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.PictureCount);
            return conf != null ? conf.Value : "";
        }

        public object ChangePassWord(long userId, string oldpassword, string newpassword, string repassword, bool isMobile)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            MD5 Hash = MD5.Create();

            if (user.Password != Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(oldpassword + user.Id))))
                throw new RecabException((int)ExceptionType.WrongUserNameORPassword);


            if (newpassword != repassword)
                throw new RecabException((int)ExceptionType.ComparePassword);

            user.Password = Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(newpassword + user.Id)));

            foreach (var item in user.UserTokens)
            {
                item.Available = false;
            }

            _sdb.SaveChanges();

            return this.SignIn(user.Email, password: newpassword, isMobile: isMobile);
        }

        public bool ForgetPassword(long userId, string newpassword, string repassword)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.WrongUserNameORPassword);

            MD5 Hash = MD5.Create();

            if (newpassword != repassword)
                throw new RecabException((int)ExceptionType.ComparePassword);

            user.Password = Convert.ToBase64String(Hash.ComputeHash(Encoding.ASCII.GetBytes(newpassword + user.Id)));

            foreach (var item in user.UserTokens)
            {
                item.Available = false;
            }

            _sdb.SaveChanges();

            return true;
        }

        public UserDetailModel UserDetail(long userId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            return new UserDetailModel
            {
                email = user.Email,
                firstName = user.FirstName,
                lastName = user.LastName,
                genderType = (int)user.GenderType,
                mobile = user.Mobile,
                mobileVerified = user.MobileVerified,
                emailVerified = user.EmailVerified
            };
        }

        public bool EditUserInfo(long userId,
                                 string fname,
                                 string lname,
                                 string email,
                                 int type)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            user.FirstName = fname ?? user.FirstName;
            user.LastName = lname ?? user.LastName;
            user.GenderType = (UserGender)type;
            user.EmailVerified = email == user.Email;
            user.Email = email;

            _sdb.SaveChanges();


            return true;
        }

        public bool EditUserMobile(long userId,
                                   string mobile,
                                   string code)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            if (_sdb.Users.Any(u => u.Id != user.Id && (u.Mobile == mobile)))
                throw new RecabException((int)ExceptionType.RedundantMobileNumber);

            if (mobile != user.Mobile)
            {
                DateTime time = DateTime.UtcNow.AddMinutes(6);

                SMS sms = _sdb.SMS.FirstOrDefault(s => s.MobileNumber == mobile && s.Content == code && s.SendDate < time);

                if (sms == null)
                    throw new RecabException((int)ExceptionType.MobileCodeInValid);

                user.Mobile = mobile;
                user.MobileVerified = true;
                _sdb.SaveChanges();
            }

            return true;
        }

        public object DealershipForLogin(long userId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            return new 
            {
                isDealership = user.Dealerships.Count() > 0,
                activeDealership = user.Dealerships.Where(d => d.Status == DealershipStatus.فعال).Count() >0
            };
        

        }

        #endregion

    }
}
