using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.ProdoctModel;
using Exon.Recab.Service.Model.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.PolicySystemConfig;
using Exon.Recab.Domain.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Service.Implement.Helper;
using System.Device.Location;
using Exon.Recab.Service.Implement.Advertise;

namespace Exon.Recab.Service.Implement.User
{
    public class DealershipService
    {
        #region init
        private SdbContext _sdb;
        private MdbContext _mdb;
        private readonly RecabSystemConfig RecabSystemConfig;
        private readonly UserPolicyService _UserPolicyService;

        internal DealershipService(ref SdbContext sdb, ref MdbContext mdb)
        {
            _sdb = sdb;
            _mdb = mdb;
            _UserPolicyService = new UserPolicyService(ref sdb);

        }

        public DealershipService()
        {
            _sdb = new SdbContext();
            _UserPolicyService = new UserPolicyService(ref _sdb);
            _mdb = new MdbContext();
            RecabSystemConfig = _sdb.RecabSystemConfig.FirstOrDefault();
        }
        #endregion

        #region ADD
        public bool AddDelership(long userId,
                             string title,
                             string address,
                             string tell,
                             string fax,
                             double coordinateLat,
                             double coordinateLong,
                             string description,
                             string websiteUrl,
                             string logoUrl,
                             long cityId,
                             List<long> categoryId)
        {

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            if (categoryId.Count == 0)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            City city = _sdb.Cities.Find(cityId);

            if (city == null)
                throw new RecabException((int)ExceptionType.CityNotFound);

            List<Category> categoris = _sdb.Categoris.ToList();

            foreach (var item in categoryId)
            {
                if (!categoris.Any(c => c.Id == item))
                    throw new RecabException((int)ExceptionType.CategoryNotFound);
            }

            Dealership newDealership = new Dealership
            {
                Title = title ?? "",
                Address = address ?? "",
                Tell = tell ?? "",
                Fax = fax ?? "",
                CoordinateLat = coordinateLat,
                CoordinateLong = coordinateLong,
                Description = description ?? "",
                WebsiteUrl = websiteUrl ?? "",
                LogoUrl = logoUrl ?? "",
                CityId = city.Id,
                Status = DealershipStatus.غیر_فعال,
                InsertDate = DateTime.UtcNow

            };

            foreach (var item in categoryId)
                newDealership.DealershipCategory.Add(new DealershipCategory { CategoryId = item });

            user.Dealerships.Add(newDealership);

            _sdb.SaveChanges();

            _UserPolicyService.EnforcementChangeStatusDealership(ref newDealership);
            return true;

        }

        #endregion

        #region Report
        public List<DelershipAdminViewModel> ListDelershipAdmin(string title,
                                                                long categoryId,
                                                                int status,
                                                                ref long count,
                                                                long? cityId,
                                                                long? stateId,
                                                                int size = 1,
                                                                int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<DelershipAdminViewModel> model = new List<DelershipAdminViewModel>();

            List<Dealership> dl = _sdb.Dealerships.Where(d => d.DealershipCategory.Any(dc => dc.CategoryId == category.Id) && d.Status == (DealershipStatus)status)
                                                  .OrderBy(c => c.Id)
                                                  .Skip(size * skip)
                                                  .Take(size)
                                                  .ToList();

            count = dl.Count;
            if (count == 0)
                return new List<DelershipAdminViewModel>();

            if (cityId.HasValue)
                dl = dl.Where(d => d.CityId == cityId.Value).ToList();

            if (stateId.HasValue)
            {
                State state = _sdb.State.Find(stateId);
                if (state != null)
                    dl = dl.Where(d => d.City.StateId == stateId.Value).ToList();
            }

            if (title != "")
            {
                dl = dl.Where(d => d.Title.Contains(title)).ToList();
            }


            foreach (var item in dl)
            {
                model.Add(new DelershipAdminViewModel
                {
                    dealearshipId = item.Id,
                    address = item.Address,
                    description = item.Description,
                    logoUrl = item.LogoUrl,
                    title = item.Title,
                    fax = item.Fax,
                    tell = item.Tell,
                    status = (int)item.Status,
                    titleStatus = item.Status.ToString(),
                    confirmDate = item.ConfirmDate.HasValue ? item.ConfirmDate.Value.UTCToPersianDateLong() : "تایید نشده",
                    insertDate = item.InsertDate.UTCToPersianDateLong()

                });

            }

            return model;
        }

        public List<DelershipViewModel> ListDelership(long userId, ref long count, int? status, long? categoryId, int size = 1, int skip = 0)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            if (categoryId.HasValue)
            {
                Category category = _sdb.Categoris.Find(categoryId.Value);
                if (category == null)
                    throw new RecabException((int)ExceptionType.CategoryNotFound);
            }

            count = user.Dealerships.Count();

            if (count == 0)
                return new List<DelershipViewModel>();


            List<DelershipViewModel> model = new List<DelershipViewModel>();

            List<Dealership> dl = _sdb.Dealerships.Where(o => o.Status != DealershipStatus.مسدود).Where(d => d.UserId == user.Id && (status.HasValue ? d.Status == (DealershipStatus)status : true)).OrderBy(c => c.CityId).ToList();

            foreach (var item in dl)
            {
                if (categoryId.HasValue)
                {
                    if (item.DealershipCategory.Any(dc => dc.CategoryId == categoryId.Value))
                        model.Add(new DelershipViewModel
                        {
                            id = item.Id,
                            address = item.Address,
                            lat = item.CoordinateLat,
                            lng = item.CoordinateLong,
                            description = item.Description,
                            logoUrl = item.LogoUrl,
                            title = item.Title,
                            fax = item.Fax,
                            tell = item.Tell,
                            categoryId = item.DealershipCategory.Select(dc => dc.CategoryId).ToArray()
                        });
                }
                else
                {
                    model.Add(new DelershipViewModel
                    {
                        id = item.Id,
                        address = item.Address,
                        lat = item.CoordinateLat,
                        lng = item.CoordinateLong,
                        description = item.Description,
                        logoUrl = item.LogoUrl,
                        title = item.Title,
                        fax = item.Fax,
                        tell = item.Tell,
                        categoryId = item.DealershipCategory.Select(dc => dc.CategoryId).ToArray()
                    });
                }

            }

            count = model.Count();

            return model.Skip(size * skip).Take(size).ToList();

        }

        public DealershipDetailViewModel DealershipDetail(long delershipId)
        {
            Dealership Dealership = _sdb.Dealerships.Find(delershipId);

            if (Dealership == null && Dealership.Status == DealershipStatus.مسدود)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            DealershipDetailViewModel model = new DealershipDetailViewModel
            {
                id = Dealership.Id,
                tell = Dealership.Tell,
                address = Dealership.Address,
                lat = Dealership.CoordinateLat,
                lng = Dealership.CoordinateLong,
                description = Dealership.Description,
                fax = Dealership.Fax,
                website = Dealership.WebsiteUrl,
                logoUrl = Dealership.LogoUrl,
                categories = Dealership.DealershipCategory.Select(c => c.Category.Title).ToList(),
                title = Dealership.Title,
                products = Dealership.ProductList.Select(p => new SearchResultItemViewModel
                {
                    advertiseId = p.Id
                }).ToList()

            };

            return model;

        }

        public List<SearchResultItemViewModel> GetDealershipAdvertise(long dealershipId, long categoryId, ref long count, int size, int skip)
        {
            Dealership dealership = _sdb.Dealerships.Find(dealershipId);

            if (dealership == null || dealership.Status != DealershipStatus.فعال)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            count = _mdb.Products.Count(filter: new BsonDocument { { "DealershipId",dealership.Id.ToString() },
                                                            { "Status",((int)ProdoctStatus.فعال).ToString()}
                                                        });

            var mongoResult = _mdb.Products.Find(filter: new BsonDocument { { "DealershipId",dealership.Id.ToString() },
                                                            { "Status",((int)ProdoctStatus.فعال).ToString()}
                                                        }).Skip(skip * size).Limit(size).ToList();


            return ProductHelperService.ConfigOutPutResult(searchResult: mongoResult, VisitedProduct: new List<long>());
        }

        public List<DelershipSearchViewModel> SearchDelership(string title,
                                                             long? cityId,
                                                             long? stateId,
                                                             long categoryId,
                                                             double? lng,
                                                             double? lat,
                                                             double distance,
                                                             ref long count,
                                                             int size = 1,
                                                             int skip = 0)
        {

            var temp = _sdb.Dealerships.Where(o => o.Status == DealershipStatus.فعال)
                                        .Where(d => title != null ? d.Title.Contains(title) : true)
                                        .ToList();
            Category category = _sdb.Categoris.Find(categoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            temp = temp.Where(d => d.DealershipCategory.Any(dc => dc.CategoryId == categoryId)).ToList();

            if (cityId.HasValue)
                temp = temp.Where(d => d.CityId == cityId.Value).ToList();

            if (stateId.HasValue)
            {
                State state = _sdb.State.Find(stateId);
                if (state != null)
                    temp = temp.Where(d => d.City.StateId == stateId.Value).ToList();
            }




            if (lng.HasValue && lat.HasValue && distance > 0)
            {

                var tempLocation = temp.Where(d => (new GeoCoordinate(latitude: d.CoordinateLat, longitude: d.CoordinateLong))
                                     .GetDistanceTo(new GeoCoordinate(latitude: lat.Value, longitude: lng.Value)) < distance)
                           .Select(d => new
                           {
                               dealership = d,
                               distance = (new GeoCoordinate(latitude: d.CoordinateLat, longitude: d.CoordinateLong)).GetDistanceTo(new GeoCoordinate(latitude: lat.Value, longitude: lng.Value))
                           })
                           .ToList();

                count = tempLocation.Count;

                return tempLocation.OrderBy(t => t.dealership.Id).Skip(size * skip).Take(size).Select(d => new DelershipSearchViewModel
                {
                    dealershipId = d.dealership.Id,
                    urlLogo = d.dealership.LogoUrl,
                    title = d.dealership.Title,
                    tell = d.dealership.Tell,
                    address = d.dealership.Address,
                    categoryTitle = DealarshipCategoryTitle(d.dealership.DealershipCategory),
                    lat = d.dealership.CoordinateLat,
                    lng = d.dealership.CoordinateLong,
                    distance = Math.Round(d.distance / 1000, digits: 1).ToString()
                }).ToList();

            }

            count = temp.Count;

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(d => new DelershipSearchViewModel
            {
                dealershipId = d.Id,
                urlLogo = d.LogoUrl,
                title = d.Title,
                tell = d.Tell,
                address = d.Address,
                categoryTitle = DealarshipCategoryTitle(d.DealershipCategory),
                lat = d.CoordinateLat,
                lng = d.CoordinateLong,
                distance = ""
            }).ToList();

        }

        public DealershipEditViewModel GetByID(long dealershipId)
        {
            Dealership dealership = _sdb.Dealerships.Find(dealershipId);

            if (dealership == null)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            return new DealershipEditViewModel
            {
                dealershipId = dealership.Id,
                logoUrl = dealership.LogoUrl,
                websiteUrl = dealership.WebsiteUrl,
                title = dealership.Title,
                tell = dealership.Tell,
                address = dealership.Address,
                categoryitems = DealarshipCategory(dealership.DealershipCategory),
                lat = dealership.CoordinateLat,
                lng = dealership.CoordinateLong,
                cityId = dealership.City.Id,
                cityTitle = dealership.City.Name,
                stateTitle = dealership.City.State.Name,
                stateId = dealership.City.StateId,
                description = dealership.Description,
                fax = dealership.Fax,
                cumUserId = dealership.UserId,
                status = dealership.Status

            };



        }

        public DealershipEditViewModel GetByID(long dealershipId, long userId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            Dealership dealership = _sdb.Dealerships.Find(dealershipId);

            if (dealership == null || dealership.UserId != userId)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            return new DealershipEditViewModel
            {
                dealershipId = dealership.Id,
                logoUrl = dealership.LogoUrl,
                websiteUrl = dealership.WebsiteUrl,
                title = dealership.Title,
                tell = dealership.Tell,
                address = dealership.Address,
                categoryitems = DealarshipCategory(dealership.DealershipCategory),
                lat = dealership.CoordinateLat,
                lng = dealership.CoordinateLong,
                cityId = dealership.City.Id,
                cityTitle = dealership.City.Name,
                stateTitle = dealership.City.State.Name,
                stateId = dealership.City.StateId,
                description = dealership.Description,
                fax = dealership.Fax,
                status = dealership.Status

            };



        }

        private string DealarshipCategoryTitle(List<DealershipCategory> model)
        {
            string temp = "";

            foreach (var item in model)
            {
                if (temp == "")
                { temp = item.Category.Title; }
                else
                { temp = temp + "," + item.Category.Title; }

            }
            return temp;
        }

        private List<long> DealarshipCategory(List<DealershipCategory> model)
        {
            List<long> result = new List<long>();

            foreach (var item in model)
            {
                result.Add(item.CategoryId);

            }

            return result;

        }

        #endregion

        #region  Edit
        public bool ChangeStatus(long dealershipId, int status)
        {
            Dealership dealership = _sdb.Dealerships.Find(dealershipId);

            if (dealership == null)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            if (dealership.Status != DealershipStatus.فعال && status == (int)DealershipStatus.فعال)
            {
                Email.EmailService _emalService = new Email.EmailService(ref _sdb, ref _mdb);
                _emalService.SendApprovedDealershipEmail(dealership);

                //if (RecabSystemConfig.NewDealershipVoucher > 0 && !dealership.User.Credit.Any(c => c.Description == "اعتبار هدیه برای ثبت نام نمایشگاه"))
                //{
                //    _sdb.Credits.Add(new Credit
                //    {
                //        UserId = dealership.UserId,
                //        InsertTime = DateTime.UtcNow,
                //        Description = "اعتبار هدیه برای ثبت نام نمایشگاه",
                //        Amount = RecabSystemConfig.NewDealershipVoucher
                //    });

                //}
            }

            dealership.Status = (DealershipStatus)status;

            


            _sdb.SaveChanges();



            return this.ChangeDealershipProductStatus(dealership);

        }

        public bool UserEditDelership(long userId,
                                   long dealershipId,
                                   string title,
                                   string address,
                                   string tell,
                                   string fax,
                                   double coordinateLat,
                                   double coordinateLong,
                                   string description,
                                   string websiteUrl,
                                   string logoUrl,
                                   long cityId,
                                   List<long> categoryId)
        {

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            Dealership dealership = _sdb.Dealerships.Find(dealershipId);
            if (dealership == null || dealership.UserId != user.Id)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            City city = _sdb.Cities.Find(cityId);

            if (city == null)
                throw new RecabException((int)ExceptionType.CityNotFound);

            List<Category> categoris = _sdb.Categoris.ToList();

            foreach (var item in categoryId)
            {
                if (!categoris.Any(c => c.Id == item))
                    throw new RecabException((int)ExceptionType.CategoryNotFound);
            }

            _sdb.DealershipCategory.RemoveRange(dealership.DealershipCategory);


            dealership.Title = title ?? "";
            dealership.Address = address ?? "";
            dealership.Tell = tell ?? "";
            dealership.Fax = fax ?? "";
            dealership.CoordinateLat = coordinateLat;
            dealership.CoordinateLong = coordinateLong;
            dealership.Description = description ?? "";
            dealership.WebsiteUrl = websiteUrl ?? "";
            dealership.LogoUrl = logoUrl ?? "";
            dealership.CityId = city.Id;
            dealership.Status = dealership.Status == DealershipStatus.فعال ? DealershipStatus.منتظربروزرسانی : dealership.Status;

            foreach (var item in categoryId)
                dealership.DealershipCategory.Add(new DealershipCategory { CategoryId = item });


            _sdb.SaveChanges();

            return this.ChangeDealershipProductStatus(dealership);

        }

        public bool AdminEditDelership(long cumUserId,
                                       long dealershipId,
                                       string title,
                                       string address,
                                       string tell,
                                       string fax,
                                       double coordinateLat,
                                       double coordinateLong,
                                       string description,
                                       string websiteUrl,
                                       string logoUrl,
                                       long cityId,
                                       int status,
                                       List<long> categoryId)
        {

            Domain.Entity.User user = _sdb.Users.Find(cumUserId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotExist);


            Dealership dealership = _sdb.Dealerships.FirstOrDefault(d => d.UserId == cumUserId && d.Id == dealershipId);
            if (dealership == null)
                throw new RecabException((int)ExceptionType.DealershipNotFound);

            City city = _sdb.Cities.Find(cityId);

            if (city == null)
                throw new RecabException((int)ExceptionType.CityNotFound);

            List<Category> categoris = _sdb.Categoris.ToList();

            foreach (var item in categoryId)
            {
                if (!categoris.Any(c => c.Id == item))
                    throw new RecabException((int)ExceptionType.CategoryNotFound);
            }

            _sdb.DealershipCategory.RemoveRange(dealership.DealershipCategory);

            if (dealership.Status != DealershipStatus.فعال && status == (int)DealershipStatus.فعال)
            {
                Email.EmailService _emalService = new Email.EmailService(ref _sdb, ref _mdb);
                _emalService.SendApprovedDealershipEmail(dealership);
            }

            dealership.UserId = user.Id;
            dealership.Title = title ?? "";
            dealership.Address = address ?? "";
            dealership.Tell = tell ?? "";
            dealership.Fax = fax ?? "";
            dealership.CoordinateLat = coordinateLat;
            dealership.CoordinateLong = coordinateLong;
            dealership.Description = description ?? "";
            dealership.WebsiteUrl = websiteUrl ?? "";
            dealership.LogoUrl = logoUrl ?? "";
            dealership.CityId = city.Id;
            dealership.Status = (DealershipStatus)status;

            foreach (var item in categoryId)
                dealership.DealershipCategory.Add(new DealershipCategory { CategoryId = item });


            _sdb.SaveChanges();



            return this.ChangeDealershipProductStatus(dealership);

        }


        #endregion

        #region  status

        public bool ChangeDealershipProductStatus(Dealership dealership)
        {

            if (dealership.ProductList.Count == 0)
                return true;

            BsonArray mongoFilter = new BsonArray();

            BsonArray arrayItem = new BsonArray();

            foreach (var p in dealership.ProductList)
            {
                arrayItem.Add(p.Id.ToString());
            }


            if (dealership.Status != DealershipStatus.فعال)
            {
                mongoFilter.Add(new BsonDocument { { "Id", new BsonDocument { { "$in", arrayItem } } } });

                var update = Builders<BsonDocument>.Update
                  .Set("DealershipStatus", ((int)dealership.Status).ToString())
                  .CurrentDate("lastModified");

                _mdb.Products.DeleteMany(new BsonDocument { { "$and", mongoFilter } });
            }
            else
            {

                AdvertiseService _AdvertiseService = new AdvertiseService(ref _sdb, ref _mdb);

                foreach (var item in dealership.ProductList)
                {
                    _AdvertiseService.MongoProductUpdate(item);
                }

            }
            return true;

        }

        #endregion

    }
}
