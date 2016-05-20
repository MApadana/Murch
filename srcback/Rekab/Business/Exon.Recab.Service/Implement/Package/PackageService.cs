using Exon.Recab.Domain.SqlServer;
using System.Linq;
using System.Collections.Generic;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Model.PackageModel;
using Exon.Recab.Domain.Entity.PackageModule;

using Exon.Recab.Domain.Constant.CS.Exception;

namespace Exon.Recab.Service.Implement.Package
{
    public class PackageService
    {
        private readonly SdbContext _sdb;

        public PackageService()
        {
            _sdb = new SdbContext();
        }

        #region   Add

        public bool AddPurchaseType(string title, bool free, bool dealership,string logoUrl)
        {
            _sdb.PurchaseType.Add(new PurchaseType { Title = title, IsFree = free, AvailableDealership = dealership , LogoUrl = logoUrl ??"t"});

            _sdb.SaveChanges();

            return true;

        }

        public bool AddCategoryToPurchaseType(long categoryId, long PurchesTypeId)
        {
            PurchaseType PurchaseType = _sdb.PurchaseType.Find(PurchesTypeId);

            if (PurchaseType == null)
                throw new RecabException((int)ExceptionType.PurchaseTypeNotFound);

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            if (PurchaseType.CategoryPurchaseTypes.FirstOrDefault(cpt => cpt.CategoryId == category.Id) != null)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeAlreadyExist);

            PurchaseType.CategoryPurchaseTypes.Add(new CategoryPurchaseType { CategoryId = category.Id });

            _sdb.SaveChanges();

            return true;
        }


        public bool AddPackageTypeToCategoryPurchaseType(long categoryPurchaseTypeId, long packageTypeId, int order)
        {
            CategoryPurchaseType CategoryPurchaseType = _sdb.CategoryPurchaseTypes.Find(categoryPurchaseTypeId);

            if (CategoryPurchaseType == null)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeNotFound);

            PackageType PackageType = _sdb.PackageTypes.Find(packageTypeId);

            if (PackageType == null)
                throw new RecabException((int)ExceptionType.PackageTypeNotFound);

            if (CategoryPurchaseType.CategoryPurchasePackageTypes.FirstOrDefault(cppt => cppt.PackageTypeId == packageTypeId) != null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeAlreadyExist);

            CategoryPurchasePackageType newCategoryPurchasePackageType = new CategoryPurchasePackageType
            {
                PackageTypeId = packageTypeId,
                OrderId = order
            };

            CategoryPurchaseType.CategoryPurchasePackageTypes.Add(newCategoryPurchasePackageType);


            _sdb.SaveChanges();

            List<PackageBaseConfig> listBaseConfig = _sdb.PackageBaseConfig.ToList();

            foreach (var item in listBaseConfig)
            {
                newCategoryPurchasePackageType.PurchaseConfig.Add(new PurchaseConfig
                {
                    CategoryPurchasePackageTypeId = newCategoryPurchasePackageType.Id,
                    PackageBaseConfigId = item.Id,
                    Value = "NULL"
                });
            }

            _sdb.SaveChanges();

            return true;

        }

        public bool AddPackageType(string title)
        {
            _sdb.PackageTypes.Add(new PackageType { Title = title });


            _sdb.SaveChanges();

            return true;

        }

        #endregion

        #region Report

        public List<PurchaseTypeViewModel> ListPurchaseType(ref long count, int size = 1, int skip = 0)
        {
            List<PurchaseType> PurchaseType = _sdb.PurchaseType.ToList();
            count = PurchaseType.Count;

            return PurchaseType.OrderBy(pt => pt.Id).Skip(size * skip).Take(size)
                                .Select(pt => new PurchaseTypeViewModel
                                {
                                    purchaseTypeId = pt.Id,
                                    title = pt.Title,
                                    isFree = pt.IsFree,
                                    isDealership = pt.AvailableDealership,
                                    logoUrl = pt.LogoUrl
                                }).ToList();
        }


        public List<PackageTypeViewModel> ListPackageTypes(ref long count, int size = 1, int skip = 0)
        {
            List<PackageType> PackageType = _sdb.PackageTypes.ToList();
            count = PackageType.Count;

            return PackageType.OrderBy(pt => pt.Id).Skip(size * skip).Take(size)
                                .Select(pt => new PackageTypeViewModel { packageTypeId = pt.Id, title = pt.Title }).ToList();
        }

        public bool EditPurchaseType(long purchaseTypeId, string title, string logoUrl)
        {
            PurchaseType PurchaseType = _sdb.PurchaseType.Find(purchaseTypeId);
            if (PurchaseType == null)
                throw new RecabException((int)ExceptionType.PurchaseTypeNotFound);

            PurchaseType.Title = title;
            PurchaseType.LogoUrl = logoUrl;

            _sdb.SaveChanges();

            return true;



        }

        public List<CategoryPurchaseTypeViewModel> ListCategoryForPurchaseType(long purchaseTypeId, ref long count, int size = 1, int skip = 0)
        {
            PurchaseType PurchaseType = _sdb.PurchaseType.Find(purchaseTypeId);

            if (PurchaseType == null)
                throw new RecabException((int)ExceptionType.PurchaseTypeNotFound);

            count = PurchaseType.CategoryPurchaseTypes.Count;


            return PurchaseType.CategoryPurchaseTypes.OrderBy(cpt => cpt.Id).Skip(size * skip).Take(size)
                   .Select(cpt => new CategoryPurchaseTypeViewModel
                   {
                       title = cpt.Category.Title,
                       categoryPurchaseTypeId = cpt.Id

                   }).ToList();
        }

        public List<CategoryPurchasePackageTypeViewModel> ListCategoryPurchasePackageType(long categoryPurchaseTypeId, ref long count, int size = 1, int skip = 0)
        {
            CategoryPurchaseType CategoryPurchaseType = _sdb.CategoryPurchaseTypes.Find(categoryPurchaseTypeId);

            if (CategoryPurchaseType == null)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeNotFound);

            count = CategoryPurchaseType.CategoryPurchasePackageTypes.Count;


            return CategoryPurchaseType.CategoryPurchasePackageTypes.OrderBy(cppt => cppt.Id).Skip(size * skip).Take(size)
                   .Select(cppt => new CategoryPurchasePackageTypeViewModel
                   {
                       categoryPurchasePachageTypeId = cppt.Id,
                       title = cppt.PackageType.Title,
                       order = cppt.OrderId
                   }).ToList();

        }

        public List<CategoryPurchaseViewModel> ListBuyPackage(long userId, long categoryId, bool isDealership)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            var temp = _sdb.CategoryPurchaseTypes.Where(cpt => cpt.CategoryId == category.Id && !cpt.PurchaseType.IsFree && cpt.PurchaseType.AvailableDealership == isDealership)
                                                 .Select(cpt => new CategoryPurchaseViewModel
                                                 {
                                                     title = cpt.PurchaseType.Title,
                                                     categoryPurchaseId = cpt.Id
                                                 }).ToList();

            return temp.Count == 0 ? new List<CategoryPurchaseViewModel>() : temp;

        }

        public CPPTDetailInitConfigViewModel ListConfigForCPPT(long cpptId)
        {
            CategoryPurchasePackageType CPPT = _sdb.CategoryPurchasePackageTypes.FirstOrDefault(cppt => cppt.Id == cpptId);
            if (CPPT == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFound);

            CPPTDetailInitConfigViewModel model = new CPPTDetailInitConfigViewModel();

            model.purchasePackageTypeId = CPPT.Id;
            model.title = CPPT.PackageType.Title;

            foreach (var item in CPPT.PurchaseConfig)
            {

                model.configItems.Add(new PackageInitConfigItemViewModel
                {
                    configTitle = item.PackageBaseConfig.Title,
                    configValue = item.Value,
                    configType = item.PackageBaseConfig.ValueType,
                    BaseValue = item.PackageBaseConfig.PackageBaseConfigValue.Select(pbcv => pbcv.Value).ToList()
                });
            }

            return model;

        }

        public List<CPPTDetailConfigViewModel> ListPurchasPackageTypeDetail(long categoryPurchaseTypeId)
        {
            CategoryPurchaseType categoryPurchaseType = _sdb.CategoryPurchaseTypes.Find(categoryPurchaseTypeId);

            if (categoryPurchaseType == null)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeNotFound);

            List<CategoryPurchasePackageType> ListPurchasePackage = _sdb.CategoryPurchasePackageTypes.Where(cppt => cppt.CategoryPurchaseTypeId == categoryPurchaseType.Id).ToList();
            //تمام رنگ های بسته تکی خودرو


            List<CPPTDetailConfigViewModel> model = new List<CPPTDetailConfigViewModel>();

            foreach (var item in ListPurchasePackage)
            {

                CPPTDetailConfigViewModel newPurchasePackage = new CPPTDetailConfigViewModel();

                newPurchasePackage.purchasePackageTypeId = item.Id;
                newPurchasePackage.title = item.PackageType.Title;
                newPurchasePackage.logoUrl = item.CategoryPurchaseType.PurchaseType.LogoUrl;

                foreach (var temp in item.PurchaseConfig)
                {

                    newPurchasePackage.packageConfigItems.Add(new PackageConfigItemViewModel
                    {
                        configTitle = temp.PackageBaseConfig.Title,
                        configValue = temp.Value,
                        configType =temp.PackageBaseConfig.ValueType
                    });
                }

                model.Add(newPurchasePackage);
            }

            return model;

        }


        public List<PurchaseTypeWithDetailViewModel> ListPurchaseTypeWithDetail(long categoryId)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<PurchaseTypeWithDetailViewModel> model = new List<PurchaseTypeWithDetailViewModel>();

            var temp = _sdb.CategoryPurchaseTypes.Where(cpt => cpt.CategoryId == category.Id && !cpt.PurchaseType.IsFree).ToList();

            foreach (var item in temp)
            {
                model.Add(new PurchaseTypeWithDetailViewModel
                {
                    isDealership = item.PurchaseType.AvailableDealership,
                    isFree = item.PurchaseType.IsFree,
                    title = item.PurchaseType.Title,
                    logoUrl= item.PurchaseType.LogoUrl,
                    items = item.CategoryPurchasePackageTypes.Select(cppt => new CPPTDetailConfigViewModel
                    {
                        title = cppt.PackageType.Title,
                        purchasePackageTypeId = cppt.Id,
                        packageConfigItems = cppt.PurchaseConfig.Select(pc => new PackageConfigItemViewModel
                        {
                            configTitle = pc.PackageBaseConfig.Title,
                            configType = pc.PackageBaseConfig.ValueType,
                            configValue = pc.Value
                        }).ToList()
                    }).ToList()
                });
            }

            return model;
        }
        #endregion

        #region Edit
        public bool EditCpptConfig(List<CpptEditConfigItemModel> configItems, long cpptId)
        {

            CategoryPurchasePackageType CPPT = _sdb.CategoryPurchasePackageTypes.FirstOrDefault(cppt => cppt.Id == cpptId);
            if (CPPT == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFound);


            int distincCount = configItems.Select(c => c.ConfigTitle).Distinct().Count();

            if (distincCount != CPPT.PurchaseConfig.Count)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);

            foreach (var item in configItems)
            {
                PurchaseConfig PurchaseConfig = CPPT.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == item.ConfigTitle);

                if (PurchaseConfig == null)
                    throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);

                PurchaseConfig.Value = item.Value;
            }


            _sdb.SaveChanges();


            return true;

        }

        #endregion

        #region delete
        public bool DeletePurchaseType(long purchaseTypeId)
        {
            PurchaseType purchaseType = _sdb.PurchaseType.Find(purchaseTypeId);

            if (purchaseType == null)
                throw new RecabException((int)ExceptionType.PurchaseTypeNotFound);

            if (purchaseType.CategoryPurchaseTypes.Count != 0)
                throw new RecabException((int)ExceptionType.PurchaseTypeExistInCPT);

            _sdb.PurchaseType.Remove(purchaseType);

            _sdb.SaveChanges();

            return true;

        }

        public bool DeleteCategoryPurchaseType(long categoryPurchaseTypeId)
        {
            CategoryPurchaseType CategoryPurchaseType = _sdb.CategoryPurchaseTypes.Find(categoryPurchaseTypeId);

            if (CategoryPurchaseType == null)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeNotFound);

            if (CategoryPurchaseType.CategoryPurchasePackageTypes.Count != 0)
                throw new RecabException((int)ExceptionType.CategoryPurchaseTypeHasUsedItem);


            _sdb.CategoryPurchaseTypes.Remove(CategoryPurchaseType);

            _sdb.SaveChanges();

            return true;

        }


        public bool DeleteCategoryPurchasePackageType(long cpptId)
        {
            CategoryPurchasePackageType cppt = _sdb.CategoryPurchasePackageTypes.Find(cpptId);

            if (cppt == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFound);

            if (_sdb.UserPackageCredits.Any(upc => upc.CategoryPurchasePackageTypeId == cppt.Id))
                throw new RecabException((int)ExceptionType.CPPTExistInUserCredit);


            _sdb.PurchaseConfig.RemoveRange(cppt.PurchaseConfig);

            _sdb.CategoryPurchasePackageTypes.Remove(cppt);

            _sdb.SaveChanges();

            return true;

        }

        #endregion
    }
}
