using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Implement.Recommend;
using Exon.Recab.Service.Model.AdminModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Exon.Recab.Service.Implement.Admin
{
    public class AdminCategoryService
    {
        private SdbContext _sdb;
        private MdbContext _mdb;

        public AdminCategoryService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
        }

        #region ADD

        public bool AddNewCategory(string title, int todayPriceRange, int relativeCount, long? parentId)
        {
            Category newCategory = new Category();

            if (parentId.HasValue)
            {
                if (_sdb.Categoris.Any(c => c.Id == parentId.Value))
                    newCategory.ParentId = parentId.Value;

                else
                    throw new RecabException((int)ExceptionType.ParentCategoryNotFound);
            }

            newCategory.Title = title ?? "";
            newCategory.TodayPriceChartLastRange = todayPriceRange;
            newCategory.ReletiveCount = relativeCount;

            _sdb.Categoris.Add(newCategory);

            _sdb.SaveChanges();

            return true;
        }

        public bool AddFeatureToCategory(long categoryId,
                                          string title,
                                            string pattern,
                                            string ContanerName,
                                            string hideContaner,
                                            int orderId,
                                            bool advertise,
                                            bool alert,
                                            bool searchBoxAd,
                                            bool lightSearchAd,
                                            bool searchResultAd,
                                            bool adTitle,
                                            bool review,
                                            bool reviewTitle,
                                            bool reviewSearch,
                                            bool loadInFirstTime,
                                            bool feedbackAD,
                                            bool article,
                                            bool articleSearch,
                                            bool articleTitle,
                                            bool custom,
                                            bool requiredAD,
                                            bool requiredRv,
                                            bool requiredAT,
                                            bool multiSelect,
                                            bool exchangeAd,
                                            bool searchMulti,
                                            bool icon,
                                            bool fvIcon,
                                            bool todayPrice,
                                            bool todayPriceSearch,
                                            bool adCompaire,
                                            bool adTextSearch,
                                            bool aTTextSearch,
                                            bool RvTextSearch,
                                            bool isMap,
                                            bool isRange,
                                            bool valueSearch,
                                            bool requierdInTPInsert,
                                            bool alertInsert,
                                            long androidId,
                                            long elementId,
                                            int titleOrder,
                                            int reletiveOrder,
                                            string iconUrl,
                                            long? rangeId
                                         )
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            AndroidElement androidElement = _sdb.AndroidElements.Find(androidId);

            if (androidElement == null)
                throw new RecabException((int)ExceptionType.AndroidElementNotFound);

            Element element = _sdb.Elements.Find(elementId);

            if (category == null)
                throw new RecabException((int)ExceptionType.HtmlElementNotFound);

            if (rangeId.HasValue)
            {
                if (_sdb.CategoryFeatures.Find(rangeId.Value) == null)
                    throw new RecabException((int)ExceptionType.RangCategoryFeatureNotFound);
            }

            int maxCount = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id).Count();

            CategoryFeature newCategoryFeature = new CategoryFeature();

            newCategoryFeature.Title = title ?? newCategoryFeature.Title;

            newCategoryFeature.Pattern = pattern ?? newCategoryFeature.Pattern;

            newCategoryFeature.ContainerName = ContanerName ?? "";

            newCategoryFeature.HideContainer = hideContaner ?? "";

            newCategoryFeature.OrderId = orderId;


            newCategoryFeature.AndroidElementId = androidElement.Id;

            newCategoryFeature.ElementId = element.Id;

            newCategoryFeature.CategoryFeatureRangeId = rangeId;

            newCategoryFeature.AvailableInADS = advertise;

            newCategoryFeature.AvailableInReview = review;

            newCategoryFeature.AvailableInSearchBox = searchBoxAd;

            newCategoryFeature.AvailableInLigthSearch = lightSearchAd;

            newCategoryFeature.AvailableInSearchResult = searchResultAd;

            newCategoryFeature.LoadInFirstTime = loadInFirstTime;

            newCategoryFeature.AvailableInArticle = article;

            newCategoryFeature.HasCustomValue = custom;

            newCategoryFeature.HasMultiSelectValue = multiSelect;

            newCategoryFeature.AvailableInExchange = exchangeAd;

            newCategoryFeature.AvailableSearchMultiSelect = searchMulti;

            newCategoryFeature.RequiredInADInsert = requiredAD;

            newCategoryFeature.RequiredInRVInsert = requiredRv;

            newCategoryFeature.RequiredInATInsert = requiredAT;

            newCategoryFeature.AvailableInTitle = adTitle;

            newCategoryFeature.AvailableIcon = icon;

            newCategoryFeature.AvailableTodayPrice = todayPrice;

            newCategoryFeature.AvailableTPInSearch = todayPriceSearch;

            newCategoryFeature.TitleOrder = titleOrder;

            newCategoryFeature.AvailableInFeedback = feedbackAD;

            newCategoryFeature.AvailableInRVSearch = reviewSearch;

            newCategoryFeature.AvailableInRVTitle = reviewTitle;

            newCategoryFeature.AvailableInATSearch = articleSearch;

            newCategoryFeature.AvailableInATTitle = articleTitle;

            newCategoryFeature.AvailableInADCompaire = adCompaire;

            newCategoryFeature.AvailableFVIcon = fvIcon;

            newCategoryFeature.AvailableInADTextSearch = adTextSearch;

            newCategoryFeature.AvailableInATTextSearch = aTTextSearch;

            newCategoryFeature.AvailableInRVTextSearch = RvTextSearch;

            newCategoryFeature.IsMap = isMap;

            newCategoryFeature.IsRenge = isRange;

            newCategoryFeature.AvailableValueSearch = valueSearch;

            newCategoryFeature.AvailableADSAlert = alert;

            newCategoryFeature.RequierdInTPInsert = requierdInTPInsert;

            newCategoryFeature.RelativeADSOrder = reletiveOrder;

            newCategoryFeature.RequiredInADSAlertInsert = alertInsert;

            category.CategoryFeatures.Add(newCategoryFeature);

            _sdb.SaveChanges();

            return this.AddCategoryFeatureIcon(categoryFeatureId: newCategoryFeature.Id, iconUrl: iconUrl);
        }

        public bool AddFeatureValueToCategoryFeature(long categoryFeaturId,
                                                     string title,
                                                     string description,
                                                     string icon,
                                                     string showcontaner,
                                                     string hideContaner)
        {
            CategoryFeature categoryFeature = _sdb.CategoryFeatures.Find(categoryFeaturId);

            if (categoryFeature == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            FeatureValue newFeatureValue = new FeatureValue();

            if (categoryFeature.FeatureValueList.Any(fv => fv.Title + fv.Description == title + description))
                throw new RecabException((int)ExceptionType.FeatureValueExist);

            newFeatureValue.Title = title ?? "";
            newFeatureValue.Description = description ?? "";
            newFeatureValue.ShowContainer = showcontaner ?? "";
            newFeatureValue.HideContainer = hideContaner ?? "";

            categoryFeature.FeatureValueList.Add(newFeatureValue);

            _sdb.SaveChanges();

            if (icon != "NULL" && icon != "")
                _sdb.Media.Add(new Media
                {
                    EntityId = newFeatureValue.Id,
                    MediaType = MediaType.Picture,
                    MediaURL = icon,
                    EntityType = EntityType.FeatureValue
                });

            _sdb.SaveChanges();


            if (categoryFeature.AvailableInADTextSearch)
            {
                RecommendService _RecommendService = new RecommendService(mdb: ref _mdb, sdb: ref _sdb);
                _RecommendService.AddTageForFeatureValue(featureValueId: newFeatureValue.Id, tag: newFeatureValue.Title);
            }
            return true;

        }


        public bool AddExchangeCategoryToCategory(long categoryId, long categoryExchangeId)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            Category exchangeCategory = _sdb.Categoris.Find(categoryExchangeId);


            if (category == null || exchangeCategory == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            if (category.CategoryExchanges.Any(ex => ex.ExchangeCategoryId == exchangeCategory.Id))
                throw new RecabException((int)ExceptionType.ExchangeCategoryAlreadyExist);

            category.CategoryExchanges.Add(new CategoryExchange { ExchangeCategoryId = exchangeCategory.Id });

            _sdb.SaveChanges();

            return true;


        }


        public bool AddFeatureValueIcon(long featureValueId, string iconUrl)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            if (!featureValue.CategoryFeature.AvailableFVIcon)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotAvalableInFeatureValueIcon);


            if (iconUrl != "NULL" && iconUrl != "")
                _sdb.Media.Add(new Media
                {
                    EntityId = featureValue.Id,
                    MediaType = MediaType.Picture,
                    MediaURL = iconUrl,
                    EntityType = EntityType.FeatureValue
                });

            _sdb.SaveChanges();

            return true;
        }


        public bool AddCategoryFeatureIcon(long categoryFeatureId, string iconUrl)
        {
            if (iconUrl == "NULL")
                return true;

            CategoryFeature categoryFeature = _sdb.CategoryFeatures.Find(categoryFeatureId);

            if (categoryFeature == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (!categoryFeature.AvailableIcon)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotAvalableInCategoryFeatureIcon);

            _sdb.Media.Add(new Media
            {
                EntityId = categoryFeature.Id,
                MediaType = MediaType.Picture,
                MediaURL = iconUrl,
                EntityType = EntityType.CategoryFeature
            });

            _sdb.SaveChanges();

            return true;
        }
        #endregion

        #region Edit
        public CategoryViewModel EditCategory(long categoryId, string title, int todayPriceRange, int relativeCount, long? parentId)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            if (parentId.HasValue)
            {
                Category parent = _sdb.Categoris.Find(parentId.Value);

                if (parent == null)
                    throw new RecabException((int)ExceptionType.ParentCategoryNotFound);
            }

            category.Title = title ?? category.Title;
            category.ParentId = parentId.HasValue ? parentId : category.ParentId;
            category.TodayPriceChartLastRange = todayPriceRange;
            category.ReletiveCount = relativeCount;

            _sdb.SaveChanges();

            return new CategoryViewModel
            {
                id = category.Id,
                title = category.Title,
                parentId = category.ParentId

            };

        }

        public CategoryFeatureViewModel EditCategoryFeature(long categoryFeatureId,
                                                            string title,
                                                            string pattern,
                                                            string ContanerName,
                                                            string hideContaner,
                                                            int orderId,
                                                            bool advertise,
                                                            bool alert,
                                                            bool searchBoxAd,
                                                            bool lightSearchAd,
                                                            bool searchResultAd,
                                                            bool adTitle,
                                                            bool review,
                                                            bool reviewTitle,
                                                            bool reviewSearch,
                                                            bool loadInFirstTime,
                                                            bool feedbackAD,
                                                            bool article,
                                                            bool articleSearch,
                                                            bool articleTitle,
                                                            bool custom,
                                                            bool requiredAD,
                                                            bool requiredRv,
                                                            bool requiredAT,
                                                            bool multiSelect,
                                                            bool exchangeAd,
                                                            bool searchMulti,
                                                            bool icon,
                                                            bool fvIcon,
                                                            bool todayPrice,
                                                            bool todayPriceSearch,
                                                            bool adCompaire,
                                                            bool adTextSearch,
                                                            bool aTTextSearch,
                                                            bool RvTextSearch,
                                                            bool isMap,
                                                            bool isRange,
                                                            bool valueSearch,
                                                            bool requierdInTPInsert,
                                                            bool requierdAlertInsert,
                                                            int reletiveOrder,
                                                            long androidId,
                                                            long elementId,
                                                            int titleOrder,
                                                            string iconUrl,
                                                            long? rangeId
                                                            )
        {
            CategoryFeature categoryFeature = _sdb.CategoryFeatures.Find(categoryFeatureId);


            if (categoryFeature == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            AndroidElement androidElement = _sdb.AndroidElements.Find(androidId);

            if (androidElement == null)
                throw new RecabException((int)ExceptionType.AndroidElementNotFound);

            Element element = _sdb.Elements.Find(elementId);

            if (element == null)
                throw new RecabException((int)ExceptionType.HtmlElementNotFound);

            if (rangeId.HasValue)
            {
                if (_sdb.CategoryFeatures.Find(rangeId.Value) == null)
                    throw new RecabException((int)ExceptionType.RangCategoryFeatureNotFound);

            }
            categoryFeature.Title = title ?? categoryFeature.Title;

            categoryFeature.Pattern = pattern ?? categoryFeature.Pattern;

            categoryFeature.OrderId = orderId;

            categoryFeature.ContainerName = ContanerName ?? categoryFeature.ContainerName;
            categoryFeature.HideContainer = hideContaner ?? categoryFeature.HideContainer;

            categoryFeature.AndroidElementId = androidElement.Id;

            categoryFeature.ElementId = element.Id;

            categoryFeature.CategoryFeatureRangeId = rangeId;

            categoryFeature.AvailableInADS = advertise;

            categoryFeature.AvailableInReview = review;

            categoryFeature.AvailableInSearchBox = searchBoxAd;

            categoryFeature.AvailableInLigthSearch = lightSearchAd;

            categoryFeature.AvailableInSearchResult = searchResultAd;

            categoryFeature.LoadInFirstTime = loadInFirstTime;

            categoryFeature.AvailableInArticle = article;

            categoryFeature.HasCustomValue = custom;

            categoryFeature.HasMultiSelectValue = multiSelect;

            categoryFeature.AvailableInExchange = exchangeAd;

            categoryFeature.AvailableSearchMultiSelect = searchMulti;

            categoryFeature.RequiredInADInsert = requiredAD;

            categoryFeature.RequiredInRVInsert = requiredRv;

            categoryFeature.RequiredInATInsert = requiredAT;

            categoryFeature.AvailableInTitle = adTitle;

            categoryFeature.AvailableIcon = icon;

            categoryFeature.AvailableTodayPrice = todayPrice;

            categoryFeature.AvailableTPInSearch = todayPriceSearch;

            categoryFeature.TitleOrder = titleOrder;

            categoryFeature.AvailableInFeedback = feedbackAD;

            categoryFeature.AvailableInRVSearch = reviewSearch;

            categoryFeature.AvailableInRVTitle = reviewTitle;

            categoryFeature.AvailableInATSearch = articleSearch;

            categoryFeature.AvailableInATTitle = articleTitle;

            categoryFeature.AvailableInADCompaire = adCompaire;

            categoryFeature.AvailableInADTextSearch = adTextSearch;

            categoryFeature.AvailableInATTextSearch = aTTextSearch;

            categoryFeature.AvailableInRVTextSearch = RvTextSearch;

            categoryFeature.IsRenge = isRange;

            categoryFeature.IsMap = isMap;

            categoryFeature.AvailableValueSearch = valueSearch;

            categoryFeature.AvailableFVIcon = fvIcon;

            categoryFeature.AvailableADSAlert = alert;

            categoryFeature.RequierdInTPInsert = requierdInTPInsert;

            categoryFeature.RelativeADSOrder = reletiveOrder;

            categoryFeature.RequiredInADSAlertInsert = requierdAlertInsert;

            var media = _sdb.Media.FirstOrDefault(m => m.EntityId == categoryFeature.Id && m.EntityType == EntityType.CategoryFeature);
            if (media != null)
            {
                if (iconUrl == "NULL")
                    _sdb.Media.Remove(media);
                else
                    media.MediaURL = iconUrl;
            }
            else
            {
                this.AddCategoryFeatureIcon(categoryFeature.Id, iconUrl);
            }

            _sdb.SaveChanges();

            return new CategoryFeatureViewModel
            {
                categoryFeatureId = categoryFeature.Id,

                pub_Title = categoryFeature.Title ?? "",
                pub_LoadInFirstTime = categoryFeature.LoadInFirstTime,
                pub_HasCustomValue = categoryFeature.HasCustomValue,
                pub_Pattern = categoryFeature.Pattern ?? "",
                pub_SearchOrderId = categoryFeature.OrderId,
                pub_CategoryFeatureRangeId = categoryFeature.CategoryFeatureRangeId,
                pub_HasMultiSelectValue = categoryFeature.HasMultiSelectValue,
                pub_AndroidElementId = categoryFeature.AndroidElementId.HasValue ? categoryFeature.AndroidElementId.Value : 0,
                pub_HtmlElementId = categoryFeature.ElementId.HasValue ? categoryFeature.ElementId.Value : 0,
                pub_AvailableCFIcon = categoryFeature.AvailableIcon,
                pub_TitleOrder = categoryFeature.TitleOrder,
                pub_AvailableFVIcon = categoryFeature.AvailableFVIcon,
                pub_IconUrl = _sdb.Media.FirstOrDefault(m => m.EntityId == categoryFeature.Id && m.EntityType == EntityType.CategoryFeature) != null ? _sdb.Media.FirstOrDefault(m => m.EntityId == categoryFeature.Id && m.EntityType == EntityType.CategoryFeature).MediaURL : "NULL",
                pub_IsMap = categoryFeature.IsMap,
                pub_IsRang = categoryFeature.IsRenge,
                pub_HasValueSearch = categoryFeature.AvailableValueSearch,

                ads_AvailableInADS = categoryFeature.AvailableInADS,
                ads_AvailableInLigthSearch = categoryFeature.AvailableInLigthSearch,
                ads_AvailableInSearchBox = categoryFeature.AvailableInSearchBox,
                ads_AvailableInSearchResult = categoryFeature.AvailableInSearchResult,
                ads_AvailableInFeedBack = categoryFeature.AvailableInFeedback,
                ads_RequiredInInsert = categoryFeature.RequiredInADInsert,
                ads_AvailableInExchange = categoryFeature.AvailableInExchange,
                ads_AvailableInADCompare = categoryFeature.AvailableInADCompaire,
                ads_AvailableInSearchMulti = categoryFeature.AvailableSearchMultiSelect,
                ads_AvailableAdTitle = categoryFeature.AvailableInTitle,
                ads_AvailableInADTextSearch = categoryFeature.AvailableInADTextSearch,
                ads_AvailableInAlert = categoryFeature.AvailableADSAlert,
                ads_ReletiveAdsOrder = categoryFeature.RelativeADSOrder,
                ads_RequiredInAlertInsert = categoryFeature.RequiredInADSAlertInsert,

                tdp_AvailableInTodayPrice = categoryFeature.AvailableTodayPrice,
                tdp_AvailableInTPSearch = categoryFeature.AvailableTPInSearch,
                tdp_RequierdInTPInsert = categoryFeature.RequierdInTPInsert,

                rew_AvailableInReview = categoryFeature.AvailableInReview,
                rew_AvailableInRVSearch = categoryFeature.AvailableInRVSearch,
                rew_AvailableInRVTitle = categoryFeature.AvailableInRVTitle,
                rew_RequiredInRvInsert = categoryFeature.RequiredInRVInsert,
                rew_AvailableInRVTextSearch = categoryFeature.AvailableInRVTextSearch,

                art_AvailableInATTitle = categoryFeature.AvailableInATTitle,
                art_RequiredInATInsert = categoryFeature.RequiredInATInsert,
                art_AvailableInATsearch = categoryFeature.AvailableInATSearch,
                art_AvailableInArticle = categoryFeature.AvailableInArticle,
                art_AvailableInATTextSearch = categoryFeature.AvailableInATTextSearch,
            };
        }

        public FeatureValueViewModel EditFeatureValue(long featureValueId,
                                                      string title,
                                                      string description,
                                                      string icon,
                                                     string showcontaner,
                                                     string hideContaner)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotExist);

            if (featureValue.Title != title && featureValue.CategoryFeature.FeatureValueList.Any(fv => fv.Title + fv.Description == title + description))
                throw new RecabException((int)ExceptionType.FeatureValueExist);

            featureValue.Title = title ?? featureValue.Title;
            featureValue.Description = description ?? featureValue.Description;
            featureValue.ShowContainer = showcontaner;
            featureValue.HideContainer = hideContaner;

            if (icon != "NULL")
            {
                Media media = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == featureValue.Id);
                if (media != null)
                {
                    media.MediaURL = icon ?? "";
                }
                else
                {
                    _sdb.Media.Add(new Media
                    {
                        EntityId = featureValue.Id,
                        EntityType = EntityType.FeatureValue,
                        MediaType = MediaType.Picture,
                        MediaURL = icon
                    });
                }
            }
            else
            {
                Media media = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == featureValue.Id);
                if (media != null)
                {
                    _sdb.Media.Remove(media);
                }

            }

            _sdb.SaveChanges();

            return new FeatureValueViewModel
            {
                id = featureValue.Id,
                title = featureValue.Title ?? "",
                pub_IconUrl = icon
            };
        }

        #endregion

        #region List
        public List<CategoryViewModel> GetAllCategoris(string title, ref long count, int size = 20, int skip = 0)
        {

            var cat = _sdb.Categoris.Where(c => c.Title.Contains(title));
            count = cat.Count();

            var temp = cat.OrderBy(c => c.Id).Skip(size * skip)
                                  .Take(size);

            if (temp == null)
                return new List<CategoryViewModel>();
            else
            {
                var list = new List<CategoryViewModel>();
                foreach (var c in temp)
                {
                    list.Add(new CategoryViewModel()
                    {
                        id = c.Id,
                        title = c.Title ?? "",
                        parentId = c.ParentId,
                        todayPriceChartlastRange = c.TodayPriceChartLastRange,
                        relativeCount = c.ReletiveCount
                    });
                }
                return list;
            }
        }


        public List<SimpleCategoryViewModel> GetAllCategorisSimple(ref long count, int size = 20, int skip = 0)
        {
            List<Category> Categoris = _sdb.Categoris.ToList();
            count = Categoris.Count;

            var temp = Categoris.OrderBy(c => c.Id).Skip(size * skip)
                                  .Take(size);

            if (temp == null)
                return new List<SimpleCategoryViewModel>();
            else
            {
                var list = new List<SimpleCategoryViewModel>();
                foreach (var c in temp)
                {
                    list.Add(new SimpleCategoryViewModel
                    {
                        id = c.Id,
                        title = c.Title ?? ""
                    });
                }
                return list;
            }
        }


        public List<CategoryFeatureViewModel> GetAllCategoryFeatureFromCategory(string title,
                                                                                long parentCategoryId,
                                                                                ref int count,
                                                                                int size = 20,
                                                                                int skip = 0)
        {
            var Cf = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == parentCategoryId && cf.Title.Contains(title));

            count = Cf.Count();

            var temp = Cf.OrderBy(x => x.OrderId)
                        .Skip(size * skip)
                        .Take(size);
            return temp == null ? new List<CategoryFeatureViewModel>() :
                        temp.Select(cf => new CategoryFeatureViewModel
                        {
                            categoryFeatureId = cf.Id,

                            pub_Title = cf.Title ?? "",
                            pub_LoadInFirstTime = cf.LoadInFirstTime,
                            pub_HasCustomValue = cf.HasCustomValue,
                            pub_Pattern = cf.Pattern ?? "",
                            pub_SearchOrderId = cf.OrderId,
                            pub_CategoryFeatureRangeId = cf.CategoryFeatureRangeId,
                            pub_HasMultiSelectValue = cf.HasMultiSelectValue,
                            pub_AndroidElementId = cf.AndroidElementId.HasValue ? cf.AndroidElementId.Value : 0,
                            pub_HtmlElementId = cf.ElementId.HasValue ? cf.ElementId.Value : 0,
                            pub_AvailableCFIcon = cf.AvailableIcon,
                            pub_TitleOrder = cf.TitleOrder,
                            pub_AvailableFVIcon = cf.AvailableFVIcon,
                            pub_IconUrl = _sdb.Media.FirstOrDefault(m => m.EntityId == cf.Id && m.EntityType == EntityType.CategoryFeature) != null ? _sdb.Media.FirstOrDefault(m => m.EntityId == cf.Id && m.EntityType == EntityType.CategoryFeature).MediaURL : "NULL",
                            pub_IsMap = cf.IsMap,
                            pub_IsRang = cf.IsRenge,
                            pub_HasValueSearch = cf.AvailableValueSearch,

                            ads_AvailableInADS = cf.AvailableInADS,
                            ads_AvailableInLigthSearch = cf.AvailableInLigthSearch,
                            ads_AvailableInSearchBox = cf.AvailableInSearchBox,
                            ads_AvailableInSearchResult = cf.AvailableInSearchResult,
                            ads_AvailableInFeedBack = cf.AvailableInFeedback,
                            ads_RequiredInInsert = cf.RequiredInADInsert,
                            ads_AvailableInExchange = cf.AvailableInExchange,
                            ads_AvailableInADCompare = cf.AvailableInADCompaire,
                            ads_AvailableInSearchMulti = cf.AvailableSearchMultiSelect,
                            ads_AvailableAdTitle = cf.AvailableInTitle,
                            ads_AvailableInADTextSearch = cf.AvailableInADTextSearch,
                            ads_ContainerName = cf.ContainerName,
                            ads_HideContainer = cf.HideContainer,
                            ads_AvailableInAlert = cf.AvailableADSAlert,
                            ads_ReletiveAdsOrder = cf.RelativeADSOrder,
                            ads_RequiredInAlertInsert = cf.RequiredInADSAlertInsert,

                            tdp_AvailableInTodayPrice = cf.AvailableTodayPrice,
                            tdp_AvailableInTPSearch = cf.AvailableTPInSearch,
                            tdp_RequierdInTPInsert  = cf.RequierdInTPInsert,
                            rew_AvailableInReview = cf.AvailableInReview,
                            rew_AvailableInRVSearch = cf.AvailableInRVSearch,
                            rew_AvailableInRVTitle = cf.AvailableInRVTitle,
                            rew_RequiredInRvInsert = cf.RequiredInRVInsert,
                            rew_AvailableInRVTextSearch = cf.AvailableInRVTextSearch,

                            art_AvailableInATTitle = cf.AvailableInATTitle,
                            art_RequiredInATInsert = cf.RequiredInATInsert,
                            art_AvailableInATsearch = cf.AvailableInATSearch,
                            art_AvailableInArticle = cf.AvailableInArticle,
                            art_AvailableInATTextSearch = cf.AvailableInATTextSearch,


                        }).ToList();

        }

        public List<FeatureValueViewModel> GetAllFeatureValueFromCategoryFeature(string title,
                                                                                 long parentCategoryFeatureId,
                                                                                 ref int count,
                                                                                 int size = 20,
                                                                                 int skip = 0)
        {

            var Fv = _sdb.FeatureValues.Where(ct => ct.CategoryFeatureId == parentCategoryFeatureId && ct.Title.Contains(title));

            count = Fv.Count();

            var temp = Fv.OrderBy(x => x.Id)
                       .Skip(size * skip)
                       .Take(size);
            return temp == null ? new List<FeatureValueViewModel>() :
                       temp.Select(fv => new FeatureValueViewModel
                       {
                           id = fv.Id,
                           title = fv.Title,
                           cfTitle = fv.CategoryFeature.Title ?? "",
                           description = fv.Description,
                           showContainer = fv.ShowContainer,
                           hideContainer = fv.HideContainer,
                           pub_IconUrl = _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == fv.Id) != null ?
                                          _sdb.Media.FirstOrDefault(m => m.EntityType == EntityType.FeatureValue && m.EntityId == fv.Id).MediaURL : "NULL"


                       }).ToList();
        }

        public List<StateViewModel> GetAllStat()
        {
            return _sdb.State.Select(s => new StateViewModel { title = s.Name, id = s.Id }).ToList();
        }


        public List<CityViewModel> GetCityOfState(long stateId)
        {
            return _sdb.Cities.Where(c => c.StateId == stateId).Select(s => new CityViewModel
            {
                title = s.Name,
                id = s.Id//,
                //lng = s.Longitude,
                //lat = s.Latitude
            }).ToList();
        }


        public List<AndroidElementViewModel> GetListAndroidElement()
        {

            return _sdb.AndroidElements.Select(a => new AndroidElementViewModel { androidElementId = a.Id, name = a.Type }).ToList();

        }

        public List<HtmlElementViewModel> GetListHtmlElement()
        {

            return _sdb.Elements.Select(e => new HtmlElementViewModel { htmlElementId = e.Id, name = e.Title }).ToList();

        }

        public List<CategoryExchangesViewModel> GetAllExchangeCategoryFromCategory(long categoryId, ref int count, int size = 1, int skip = 0)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            count = category.CategoryExchanges.Count;

            if (count == 0)
                return new List<CategoryExchangesViewModel>();

            return category.CategoryExchanges.Join(_sdb.Categoris,
                                       cx => cx.ExchangeCategoryId,
                                       c => c.Id,
                                       (cx, c) => new CategoryExchangesViewModel
                                       {
                                           title = c.Title,
                                           categoryId = c.Id,
                                           categoryExchangeId = cx.Id


                                       }).OrderBy(c => c.categoryExchangeId)
                                         .Skip(size * skip)
                                         .Take(size)
                                       .ToList();

        }

        public List<SimpleCategoryViewModel> GetAllExchangeCategoryFromCategorySimple(long categoryId, ref int count, int size = 1, int skip = 0)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            count = category.CategoryExchanges.Count;

            if (count == 0)
                return new List<SimpleCategoryViewModel>();

            return category.CategoryExchanges.Join(_sdb.Categoris,
                                       cx => cx.ExchangeCategoryId,
                                       c => c.Id,
                                       (cx, c) => new SimpleCategoryViewModel
                                       {
                                           title = c.Title,
                                           id = cx.ExchangeCategoryId


                                       }).OrderBy(c => c.id)
                                         .Skip(size * skip)
                                         .Take(size)
                                       .ToList();

        }

        public List<CategoryViewModel> GetAllFavoriteUserCategory(long userId)
        {

            List<FavouriteProduct> fvProduct = _sdb.FavouriteProduct.Where(fp => fp.UserId == userId && fp.Product.Status == Domain.Constant.Prodoct.ProdoctStatus.فعال).ToList();

            if (fvProduct.Count == 0)
                return new List<CategoryViewModel>();

            var categoris = fvProduct.Select(pf => pf.Product.Category).Distinct();

            List<CategoryViewModel> model = new List<CategoryViewModel>();

            foreach (var item in categoris)
            {
                model.Add(new CategoryViewModel { title = item.Title, id = item.Id });
            }

            return model;
        }


        #endregion

        #region Delete
        public bool DeleteCategory(long id)
        {
            Category category = _sdb.Categoris.Find(id);

            if (category != null)
            {
                _sdb.Categoris.Remove(category);
                _sdb.SaveChanges();
                return true;
            }

            throw new Exception("Category not Found");
        }

        public bool DeleteCategoryFeature(long id)
        {
            CategoryFeature categoryFeature = _sdb.CategoryFeatures.Find(id);

            if (categoryFeature != null)
            {
                _sdb.CategoryFeatures.Remove(categoryFeature);
                _sdb.SaveChanges();
                return true;
            }

            throw new Exception("categoryFeature not Found");
        }

        public bool DeleteFeatureValue(long id)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(id);

            if (featureValue != null)
            {
                _sdb.FeatureValues.Remove(featureValue);
                _sdb.SaveChanges();
                return true;
            }

            throw new Exception("FeatureValue not Found");
        }

        public bool DeleteExchangeFromCategory(long categoryExchangeId)
        {
            CategoryExchange exchangeCategory = _sdb.CategoryExchange.Find(categoryExchangeId);

            if (exchangeCategory == null)
                throw new RecabException((int)ExceptionType.ExchangeCategoryNotFound);

            _sdb.CategoryExchange.Remove(exchangeCategory);

            _sdb.SaveChanges();

            return true;

        }

        #endregion

        #region categoryFeatureDependency

        #region ADD
        public bool AddParentToCategoryFeature(long id, long parentId)
        {
            if (id == parentId)
                throw new RecabException((int)ExceptionType.SameParentAndChild);

            CategoryFeature cf = _sdb.CategoryFeatures.Find(id);
            CategoryFeature parent = _sdb.CategoryFeatures.Find(parentId);

            if (cf == null || parent == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (cf.ParentList.Any(c => c.CategoryFeatureId == parent.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureParentAlreadyExist);

            if (parent.ChildeList.Any(c => c.CategoryFeatureId == cf.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureChildAlreadyExist);

            cf.ParentList.Add(new CategoryFeatureDependency { CategoryFeatureId = parent.Id });

            parent.ChildeList.Add(new CategoryFeatureDependency { CategoryFeatureId = cf.Id });

            _sdb.SaveChanges();

            return true;

        }

        public bool AddHideToCategoryFeature(long id, long hideId)
        {

            if (id == hideId)
                throw new RecabException((int)ExceptionType.SameParentAndChild);

            CategoryFeature cf = _sdb.CategoryFeatures.Find(id);

            CategoryFeature Hide = _sdb.CategoryFeatures.Find(hideId);

            if (cf == null || Hide == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (cf.HideList.Any(c => c.CategoryFeatureHideId == Hide.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureParentAlreadyExist);

            cf.HideList.Add(new CategoryFeatureDependency { CategoryFeatureHideId = Hide.Id, CategoryFeatureId = cf.Id });

            _sdb.SaveChanges();

            return true;

        }

        public bool AddCategoryFeatureDefault(long id, long cfDefaultId, bool enable, string customValue, long? featureValueId)
        {
            CategoryFeature cf = _sdb.CategoryFeatures.Find(id);
            CategoryFeature cfDefault = _sdb.CategoryFeatures.Find(cfDefaultId);

            if (cf == null || cfDefault == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (featureValueId.HasValue)
            {
                if (!cfDefault.FeatureValueList.Any(fv => fv.Id == featureValueId.Value))
                    throw new RecabException((int)ExceptionType.FeatureValueInvalid);
            }

            if (enable)
            {
                if (cf.EnableList.Any(e => e.EnableCategoryFeatureId == cfDefault.Id))
                    throw new RecabException((int)ExceptionType.EnableCFAlreadyExist);

                cf.EnableList.Add(new CategoryFeatureDefaultValue
                {
                    CategoryFeatureId = cf.Id,
                    EnableCategoryFeatureId = cfDefault.Id,
                    EnableFeatureValueId = featureValueId,
                    EnableFeatureValueCustomValue = customValue ?? ""
                });

            }
            else
            {
                if (cf.DisableList.Any(e => e.DisableCategoryFeatureId == cfDefault.Id))
                    throw new RecabException((int)ExceptionType.EnableCFAlreadyExist);

                cf.DisableList.Add(new CategoryFeatureDefaultValue
                {
                    CategoryFeatureId = cf.Id,
                    DisableCategoryFeatureId = cfDefault.Id,
                    DisableFeatureValueId = featureValueId,
                    DisableFeatureValueCustomValue = customValue ?? ""
                });

            }

            _sdb.SaveChanges();

            return true;
        }
        #endregion

        #region LIST
        public List<CategoryFeatureDependencyViewModel> GetChildrenOfCategoryFeature(string title, long categoryFeatureId, ref int count, int size = 1, int skip = 0)
        {

            CategoryFeature categoryFeature = _sdb.CategoryFeatures.Find(categoryFeatureId);

            if (categoryFeature == null)
                return new List<CategoryFeatureDependencyViewModel>();

            if (categoryFeature.ChildeList.ToList().Count == 0)
                return new List<CategoryFeatureDependencyViewModel>();

            var temp = categoryFeature.ChildeList.Join(_sdb.CategoryFeatures.Where(c => c.Title.Contains(title)).ToList(),
                                                    child => child.CategoryFeatureId,
                                                    cf => cf.Id,
                                                    (child, cf) => (cf));

            count = temp.Count();

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(cf => new CategoryFeatureDependencyViewModel
            {
                id = cf.Id,
                orderId = cf.OrderId,
                title = cf.Title
            }).ToList();

        }

        public List<CategoryFeatureDependencyViewModel> GetParentOfCategoryFeature(string title, long categoryFeatureId, ref int count, int size = 1, int skip = 0)
        {
            CategoryFeature cFeature = _sdb.CategoryFeatures.Find(categoryFeatureId);

            if (cFeature == null)
                return new List<CategoryFeatureDependencyViewModel>();

            if (cFeature.ParentList.ToList().Count == 0)
                return new List<CategoryFeatureDependencyViewModel>();

            var temp = cFeature.ParentList.Join(_sdb.CategoryFeatures.Where(c => c.Title.Contains(title)).ToList(),
                                                   par => par.CategoryFeatureId,
                                                   cf => cf.Id,
                                                   (par, cf) => (cf));

            count = temp.Count();
            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(cf => new CategoryFeatureDependencyViewModel
            {
                id = cf.Id,
                orderId = cf.OrderId,
                title = cf.Title
            }).ToList();

        }

        public List<CategoryFeatureDependencyViewModel> GetHideOfCategoryFeature(string title, long categoryFeatureId, ref int count, int size = 1, int skip = 0)
        {
            CategoryFeature categoryfeature = _sdb.CategoryFeatures.Find(categoryFeatureId);

            if (categoryfeature == null)
                return new List<CategoryFeatureDependencyViewModel>();

            if (categoryfeature.HideList.ToList().Count == 0)
                return new List<CategoryFeatureDependencyViewModel>();

            var temp = categoryfeature.HideList.Where(h => h.CategoryFeatureHide.Title.Contains(title)).ToList();

            count = temp.Count();
            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(cf => new CategoryFeatureDependencyViewModel
            {
                id = cf.CategoryFeatureHideId.Value,
                orderId = cf.CategoryFeatureHide.OrderId,
                title = cf.CategoryFeatureHide.Title
            }).ToList();

        }

        public List<GetCategoryFeatureDefaultViewModel> GetCategoryFeatureDefault(long id, bool enable, ref int count, int size = 1, int skip = 0)
        {
            CategoryFeature cf = _sdb.CategoryFeatures.Find(id);
            if (cf == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (enable)
            {
                count = cf.EnableList.Count;

                return cf.EnableList.OrderBy(e => e.Id).Skip(size * skip).Take(size).Select(e => new GetCategoryFeatureDefaultViewModel
                {
                    categoryFeatureId = e.EnableCategoryFeatureId,
                    categoryFeatureTitle = _sdb.CategoryFeatures.Find(e.EnableCategoryFeatureId).Title,
                    featureValueId = e.EnableFeatureValueId,
                    featureValueTitle = e.EnableFeatureValueId.HasValue ? _sdb.CategoryFeatures.Find(e.EnableCategoryFeatureId.Value).FeatureValueList.Find(f => f.Id == e.EnableFeatureValueId.Value).Title : "",
                    customValue = e.EnableFeatureValueCustomValue
                }).ToList();
            }
            else
            {
                count = cf.DisableList.Count;

                return cf.DisableList.OrderBy(e => e.Id).Skip(size * skip).Take(size).Select(e => new GetCategoryFeatureDefaultViewModel
                {
                    categoryFeatureId = e.DisableCategoryFeatureId,
                    categoryFeatureTitle = _sdb.CategoryFeatures.Find(e.DisableCategoryFeatureId).Title,
                    featureValueId = e.DisableFeatureValueId,
                    featureValueTitle = e.DisableFeatureValueId.HasValue ? _sdb.CategoryFeatures.Find(e.DisableCategoryFeatureId.Value).FeatureValueList.Find(f => f.Id == e.DisableFeatureValueId.Value).Title : "",
                    customValue = e.DisableFeatureValueCustomValue
                }).ToList();
            }

        }

        #endregion

        #region Delete
        public bool DeleteCategoryFeatureDependency(long id, long parentId)
        {
            CategoryFeature child = _sdb.CategoryFeatures.Find(id);
            CategoryFeature parent = _sdb.CategoryFeatures.Find(parentId);
            if (child == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (!child.ParentList.Any(p => p.CategoryFeatureId == parent.Id))
                throw new RecabException((int)ExceptionType.ParentCategoryNotFound);

            if (!parent.ChildeList.Any(c => c.CategoryFeatureId == child.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureChildNotFound);

            parent.ChildeList.RemoveAll(c => c.CategoryFeatureId == child.Id);

            child.ParentList.RemoveAll(c => c.CategoryFeatureId == parent.Id);

            _sdb.SaveChanges();

            return true;
        }

        public bool DeleteCategoryFeatureHide(long id, long parentId)
        {
            CategoryFeature CategoryFeature = _sdb.CategoryFeatures.Find(id);

            CategoryFeature hide = _sdb.CategoryFeatures.Find(parentId);

            if (CategoryFeature == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (!CategoryFeature.HideList.Any(p => p.CategoryFeatureHideId == hide.Id))
                throw new RecabException((int)ExceptionType.ParentCategoryNotFound);

            CategoryFeature.HideList.RemoveAll(c => c.CategoryFeatureHideId == hide.Id);

            _sdb.SaveChanges();

            return true;
        }

        public bool DeleteCategoryFeatureDefaultValue(long id, long cfDefaultId, bool enable)
        {
            CategoryFeature cf = _sdb.CategoryFeatures.Find(id);
            CategoryFeature cfDefault = _sdb.CategoryFeatures.Find(cfDefaultId);

            if (cf == null || cfDefault == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (enable)
            {
                cf.EnableList.RemoveAll(e => e.EnableCategoryFeatureId == cfDefault.Id);
            }
            else
            {
                cf.DisableList.RemoveAll(e => e.DisableCategoryFeatureId == cfDefault.Id);
            }

            _sdb.SaveChanges();
            return true;

        }
        #endregion

        #endregion

        #region FeatureValueDependency

        #region ADD
        public bool AddParentToFeatureValue(long id, long parentId)
        {
            if (id == parentId)
                throw new RecabException((int)ExceptionType.SameParentAndChild);

            FeatureValue fv = _sdb.FeatureValues.Find(id);

            if (fv == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            FeatureValue parent = _sdb.FeatureValues.Find(parentId);

            if (parent == null)
                throw new RecabException((int)ExceptionType.FeatureValueParentNotFound);

            if (fv.ParentList.Any(c => c.FeatureValueId == parent.Id))
                throw new RecabException((int)ExceptionType.FeatureValueParentAlreadyExist);

            if (parent.ChildList.Any(c => c.FeatureValueId == fv.Id))
                throw new RecabException((int)ExceptionType.FeatureValueChildAlreadyExist);

            fv.ParentList.Add(new FeatureValueDependency { FeatureValueId = parent.Id });

            parent.ChildList.Add(new FeatureValueDependency { FeatureValueId = fv.Id });


            _sdb.SaveChanges();

            return true;

        }

        public bool AddHideToFeatureValue(long id, long cfHideId)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(id);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            CategoryFeature hide = _sdb.CategoryFeatures.Find(cfHideId);

            if (hide == null)
                throw new RecabException((int)ExceptionType.FeatureValueParentNotFound);

            if (featureValue.HideList.Any(c => c.CategoryFeatureHideId == hide.Id))
                throw new RecabException((int)ExceptionType.FeatureValueParentAlreadyExist);

            featureValue.HideList.Add(new FeatureValueDependency { CategoryFeatureHideId = hide.Id, FeatureValueId = featureValue.Id });


            _sdb.SaveChanges();

            return true;

        }

        public bool AddShowToFeatureValue(long id, long cfshowId)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(id);

            if (featureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            CategoryFeature show = _sdb.CategoryFeatures.Find(cfshowId);

            if (show == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (featureValue.ShowList.Any(c => c.CategoryFeatureShowId == show.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureParentAlreadyExist);

            featureValue.ShowList.Add(new FeatureValueDependency { CategoryFeatureShowId = show.Id, FeatureValueId = featureValue.Id });


            _sdb.SaveChanges();

            return true;

        }

        public bool AddFeatureValueDefault(long id, long cfDefaultId, bool enable, string customValue, long? featureValueId)
        {
            FeatureValue fv = _sdb.FeatureValues.Find(id);
            CategoryFeature cfDefault = _sdb.CategoryFeatures.Find(cfDefaultId);

            if (fv == null || cfDefault == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (featureValueId.HasValue)
            {
                if (!cfDefault.FeatureValueList.Any(fV => fV.Id == featureValueId.Value))
                    throw new RecabException((int)ExceptionType.FeatureValueInvalid);
            }

            if (enable)
            {
                if (fv.EnableList.Any(e => e.EnableCategoryFeatureId == cfDefault.Id))
                    throw new RecabException((int)ExceptionType.EnableCFAlreadyExist);


                fv.EnableList.Add(new FeatureValueDefaultValue
                {
                    FeatureValueId = fv.Id,
                    EnableCategoryFeatureId = cfDefault.Id,
                    EnableFeatureValueCustomValue = customValue ?? "",
                    EnableValueId = featureValueId

                });
            }
            else
            {
                if (fv.DisableList.Any(e => e.DisableCategoryFeatureId == cfDefault.Id))
                    throw new RecabException((int)ExceptionType.EnableCFAlreadyExist);


                fv.DisableList.Add(new FeatureValueDefaultValue
                {
                    FeatureValueId = fv.Id,
                    DisableCategoryFeatureId = cfDefault.Id,
                    DisableFeatureValueCustomValue = customValue ?? "",
                    DisableValueId = featureValueId

                });

            }

            _sdb.SaveChanges();
            return true;


        }
        #endregion

        #region LIST

        public List<FeatureValueDependencyViewModel> GetChildrenOfFeatureValue(string title, long featureValueId, ref int count, int size = 1, int skip = 0)
        {

            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                return new List<FeatureValueDependencyViewModel>();

            if (featureValue.ChildList.ToList().Count == 0)
                return new List<FeatureValueDependencyViewModel>();

            var temp = featureValue.ChildList.Join(_sdb.FeatureValues.Where(cf => cf.Title.Contains(title)).ToList(),
                                                    child => child.FeatureValueId,
                                                    fv => fv.Id,
                                                    (child, fv) => (fv));

            count = temp.Count();

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(fv => new FeatureValueDependencyViewModel
            {
                id = fv.Id,
                title = fv.Title + (fv.Description != null ? "-" + fv.Description : ""),
                cfTitle = fv.CategoryFeature.Title ?? ""
            }).ToList();

        }

        public List<FeatureValueDependencyViewModel> GetParentOfFeatureValue(string title, long FeatureValueId, ref int count, int size = 1, int skip = 0)
        {
            FeatureValue featureValue = _sdb.FeatureValues.Find(FeatureValueId);

            if (featureValue == null)
                return new List<FeatureValueDependencyViewModel>();

            if (featureValue.ParentList.ToList().Count == 0)
                return new List<FeatureValueDependencyViewModel>();

            var temp = featureValue.ParentList.Join(_sdb.FeatureValues.Where(fv => fv.Title.Contains(title)).ToList(),
                                                   par => par.FeatureValueId,
                                                   fv => fv.Id,
                                                   (par, fv) => (fv));

            count = temp.Count();

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(fv => new FeatureValueDependencyViewModel
            {
                id = fv.Id,
                title = fv.Title + (fv.Description != null ? "-" + fv.Description : ""),
                cfTitle = fv.CategoryFeature.Title ?? ""
            }).ToList();

        }

        public List<FeatureValueDependencyViewModel> GetHideOfFeatureValue(string title, long featureValueId, ref int count, int size = 1, int skip = 0)
        {

            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                return new List<FeatureValueDependencyViewModel>();

            if (featureValue.HideList.ToList().Count == 0)
                return new List<FeatureValueDependencyViewModel>();

            var temp = featureValue.HideList.Where(h => h.CategoryFeatureHide.Title.Contains(title)).ToList();

            count = temp.Count();

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(s => new FeatureValueDependencyViewModel
            {
                id = s.CategoryFeatureHideId.Value,
                title = s.CategoryFeatureHide.Title,
                cfTitle = s.CategoryFeatureHide.Title ?? ""
            }).ToList();

        }

        public List<FeatureValueDependencyViewModel> GetShowOfFeatureValue(string title, long featureValueId, ref int count, int size = 1, int skip = 0)
        {

            FeatureValue featureValue = _sdb.FeatureValues.Find(featureValueId);

            if (featureValue == null)
                return new List<FeatureValueDependencyViewModel>();

            if (featureValue.ShowList.ToList().Count == 0)
                return new List<FeatureValueDependencyViewModel>();

            var temp = featureValue.ShowList.Where(s => s.CategoryFeatureShow.Title.Contains(title)).ToList();

            count = temp.Count();

            return temp.OrderBy(t => t.Id).Skip(size * skip).Take(size).Select(s => new FeatureValueDependencyViewModel
            {
                id = s.CategoryFeatureShowId.Value,
                title = s.CategoryFeatureShow.Title,
                cfTitle = s.CategoryFeatureShow.Title ?? ""
            }).ToList();

        }

        public List<GetCategoryFeatureDefaultViewModel> GetFeatureValueDefault(long id, bool enable, ref int count, int size = 1, int skip = 0)
        {
            FeatureValue cf = _sdb.FeatureValues.Find(id);
            if (cf == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (enable)
            {
                count = cf.EnableList.Count;

                return cf.EnableList.OrderBy(e => e.Id).Skip(size * skip).Take(size).Select(e => new GetCategoryFeatureDefaultViewModel
                {
                    categoryFeatureId = e.EnableCategoryFeatureId,
                    categoryFeatureTitle = _sdb.CategoryFeatures.Find(e.EnableCategoryFeatureId).Title,
                    featureValueId = e.EnableValueId,
                    //  featureValueTitle = e.EnableValueId.HasValue ? _sdb.CategoryFeatures.Find(e.EnableCategoryFeatureId.Value).FeatureValueList.Find(f => f.Id == e.EnableValueId.Value).Title : "",
                    customValue = e.EnableFeatureValueCustomValue
                }).ToList();
            }
            else
            {
                count = cf.DisableList.Count;

                return cf.DisableList.OrderBy(e => e.Id).Skip(size * skip).Take(size).Select(e => new GetCategoryFeatureDefaultViewModel
                {
                    categoryFeatureId = e.DisableCategoryFeatureId,
                    categoryFeatureTitle = _sdb.CategoryFeatures.Find(e.DisableCategoryFeatureId).Title,
                    featureValueId = e.DisableValueId,
                    //  featureValueTitle = e.DisableValueId.HasValue ? _sdb.CategoryFeatures.Find(e.EnableCategoryFeatureId.Value).FeatureValueList.Find(f => f.Id == e.DisableValueId.Value).Title : "",
                    customValue = e.DisableFeatureValueCustomValue
                }).ToList();
            }

        }

        #endregion

        #region Delete
        public bool DeleteFeatureValueDependency(long id, long parentId)
        {

            FeatureValue child = _sdb.FeatureValues.Find(id);
            if (child == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            FeatureValue parent = _sdb.FeatureValues.Find(parentId);
            if (parent == null)
                throw new RecabException((int)ExceptionType.FeatureValueParentNotFound);

            if (!child.ParentList.Any(p => p.FeatureValueId == parent.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureChildNotFound);

            if (!parent.ChildList.Any(c => c.FeatureValueId == child.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureChildNotFound);


            parent.ChildList.RemoveAll(cfv => cfv.FeatureValueId == child.Id);

            child.ParentList.RemoveAll(pfv => pfv.FeatureValueId == parent.Id);

            _sdb.SaveChanges();

            return true;
        }

        public bool DeleteFeatureValueShow(long id, long cfshowId)
        {
            FeatureValue FeatureValue = _sdb.FeatureValues.Find(id);
            if (FeatureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            CategoryFeature cfshow = _sdb.CategoryFeatures.Find(cfshowId);
            if (cfshow == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (!FeatureValue.ShowList.Any(p => p.CategoryFeatureShowId == cfshow.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            FeatureValue.ShowList.RemoveAll(cfv => cfv.CategoryFeatureShowId == cfshow.Id);

            _sdb.SaveChanges();

            return true;
        }

        public bool DeleteFeatureValueHide(long id, long cfHideId)
        {
            FeatureValue FeatureValue = _sdb.FeatureValues.Find(id);
            if (FeatureValue == null)
                throw new RecabException((int)ExceptionType.FeatureValueNotFound);

            CategoryFeature cfHide = _sdb.CategoryFeatures.Find(cfHideId);
            if (cfHide == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (!FeatureValue.HideList.Any(p => p.CategoryFeatureHideId == cfHide.Id))
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            FeatureValue.HideList.RemoveAll(cfv => cfv.CategoryFeatureHideId == cfHide.Id);

            _sdb.SaveChanges();

            return true;
        }

        public bool DeleteFeatureValueDefaultValue(long id, long cfDefaultId, bool enable)
        {
            FeatureValue cf = _sdb.FeatureValues.Find(id);

            CategoryFeature cfDefault = _sdb.CategoryFeatures.Find(cfDefaultId);

            if (cf == null || cfDefault == null)
                throw new RecabException((int)ExceptionType.CategoryFeatureNotFound);

            if (enable)
            {
                cf.EnableList.RemoveAll(e => e.EnableCategoryFeatureId == cfDefault.Id);
            }
            else
            {
                cf.DisableList.RemoveAll(e => e.DisableCategoryFeatureId == cfDefault.Id);
            }

            _sdb.SaveChanges();
            return true;

        }
        #endregion

        #endregion


    }
}
