using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Service.Model.ConfigModel;
using Exon.Recab.Service.Constant;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Service.Implement.User;
using Exon.Recab.Domain.Entity.AlertModule;
using Exon.Recab.Service.Model.ProdoctModel.AlertModel;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Service.Resource;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Service.Model.ProdoctModel;

namespace Exon.Recab.Service.Implement.Config
{
    public class BaseConfigService
    {
        #region init
        private SdbContext _sdb;

        private MdbContext _mdb;

        public BaseConfigService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
        }
        #endregion

        #region  featureValue
        public List<CategoryFeatureValuesResultModel> GetBaseFeatureValue(long categoryId,
                                                                          bool isSimple,
                                                                          int type,
                                                                          List<CategoryFeatureFilterModel> categoryFeatureFilter)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                return null;

            FeatureConfigType Type = (FeatureConfigType)type;

            List<CategoryFeatureValuesResultModel> model = new List<CategoryFeatureValuesResultModel>();


            if (categoryFeatureFilter.Count == 0)
            {
                List<CategoryFeature> categoryFeatureList = new List<CategoryFeature>();

                #region count ==0
                switch (Type)
                {

                    case FeatureConfigType.Advertise:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                      cf.AvailableInSearchBox &&
                                                                                      cf.AvailableInADS &&
                                                                                      cf.LoadInFirstTime &&
                                                                                      !cf.IsRenge
                                                                                      ).ToList();

                        break;

                    case FeatureConfigType.Article:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                      cf.AvailableInSearchBox &&
                                                                                      cf.AvailableInArticle &&
                                                                                      cf.LoadInFirstTime &&
                                                                                      !cf.IsRenge).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                       cf.AvailableInSearchBox &&
                                                                                       cf.AvailableInRVSearch &&
                                                                                       cf.LoadInFirstTime &&
                                                                                      !cf.IsRenge).ToList();
                        break;
                    case FeatureConfigType.TodayPrice:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableTodayPrice &&
                                                                               c.AvailableTPInSearch &&
                                                                               !c.IsRenge).ToList();
                        break;
                    case FeatureConfigType.Alert:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableADSAlert &&
                                                                               c.LoadInFirstTime &&
                                                                               !c.IsRenge).ToList();
                        break;

                }

                if (isSimple)
                {
                    categoryFeatureList = categoryFeatureList.Where(cf => cf.AvailableInLigthSearch).ToList();
                }



                foreach (CategoryFeature item in categoryFeatureList)
                {
                    model.Add(new CategoryFeatureValuesResultModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        values = item.FeatureValueList.Select(fv => new BaseValuesModel
                        {
                            featureValueId = fv.Id,
                            title = fv.Title
                        }).ToList()
                    });

                }
                #endregion
            }

            else
            {
                #region filter
                List<CategoryFeature> categoryFeatureList = new List<CategoryFeature>();

                switch (Type)
                {

                    case FeatureConfigType.Advertise:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInADS &&
                                                                                !cf.HasMultiSelectValue &&
                                                                                !cf.HasCustomValue &&
                                                                                      !cf.IsRenge).ToList();

                        break;

                    case FeatureConfigType.Article:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInArticle &&
                                                                                !cf.HasMultiSelectValue &&
                                                                                !cf.HasCustomValue &&
                                                                                      !cf.IsRenge).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInReview &&
                                                                                      !cf.IsRenge).ToList();

                        break;
                    case FeatureConfigType.TodayPrice:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableTodayPrice &&
                                                                                      !c.IsRenge).ToList();
                        break;

                    case FeatureConfigType.Alert:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableADSAlert &&
                                                                                !cf.HasMultiSelectValue &&
                                                                                !cf.HasCustomValue &&
                                                                                      !cf.IsRenge).ToList();
                        break;

                }

                if (isSimple)
                {
                    categoryFeatureList = categoryFeatureList.Where(cf => cf.AvailableInLigthSearch).ToList();
                }
                #endregion


                List<FilterSelectTempModel> FilterList = new List<FilterSelectTempModel>();

                foreach (CategoryFeatureFilterModel item in categoryFeatureFilter)
                {
                    CategoryFeature CF = categoryFeatureList.Find(c => c.Id == item.CategoryFeatureId);

                    if (CF != null && item.FeatureValueId.Count > 0)
                    {
                        FeatureValue FV = CF.FeatureValueList.Find(fv => fv.Id == item.FeatureValueId.First());
                        if (FV != null)

                            FilterList.Add(new FilterSelectTempModel { CF = CF, SelectedFV = FV });
                    }
                }


                FilterList = FilterList.OrderBy(c => c.CF.OrderId).ToList();

                foreach (var item in FilterList)
                {
                    foreach (var child in item.CF.ChildeList)
                    {

                        var childCategoryFeature = categoryFeatureList.Find(c => c.Id == child.CategoryFeatureId);

                        if (childCategoryFeature != null)
                        {
                            var tttt = childCategoryFeature.FeatureValueList.Where(cfv => cfv.ParentList.Any(p => p.FeatureValueId == item.SelectedFV.Id));


                            model.Add(new CategoryFeatureValuesResultModel
                            {
                                categoryFeatureId = childCategoryFeature.Id,
                                title = childCategoryFeature.Title,
                                values = tttt.Select(fv => new BaseValuesModel
                                {
                                    featureValueId = fv.Id,
                                    title = fv.Title
                                }).ToList()
                            });
                        }
                    }
                }
            }

            foreach (var item in categoryFeatureFilter)
            {
                if (model.Any(m => m.categoryFeatureId == item.CategoryFeatureId))
                {
                    model.RemoveAll(c => c.categoryFeatureId == item.CategoryFeatureId);
                }

            }


            return model;
        }


        public List<CategoryFeatureValuesResultModel> ManageFeatureValue(long categoryId,
                                                                         int type,
                                                                         List<CategoryFeatureFilterModel> categoryFeatureFilter)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                return null;

            FeatureConfigType Type = (FeatureConfigType)type;

            List<CategoryFeatureValuesResultModel> model = new List<CategoryFeatureValuesResultModel>();


            if (categoryFeatureFilter.Count == 0)
            {
                List<CategoryFeature> categoryFeatureList = new List<CategoryFeature>();

                #region count ==0
                switch (Type)
                {

                    case FeatureConfigType.Advertise:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                      cf.AvailableInSearchBox &&
                                                                                      cf.AvailableInADS &&
                                                                                      cf.LoadInFirstTime &&
                                                                                      cf.ParentList.Count == 0 &&
                                                                                      !cf.IsRenge
                                                                                      ).ToList();

                        break;

                    case FeatureConfigType.Article:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                                      c.AvailableInSearchBox &&
                                                                                      c.AvailableInArticle &&
                                                                                      c.LoadInFirstTime &&
                                                                                      c.ParentList.Count() == 0 &&
                                                                                      !c.IsRenge).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                       cf.AvailableInSearchBox &&
                                                                                       cf.AvailableInRVSearch &&
                                                                                       cf.LoadInFirstTime &&
                                                                                       cf.ParentList.Count() == 0 &&
                                                                                      !cf.IsRenge).ToList();
                        break;
                    case FeatureConfigType.TodayPrice:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableTodayPrice &&
                                                                               c.AvailableTPInSearch &&
                                                                                      !c.IsRenge).ToList();
                        break;

                    case FeatureConfigType.Alert:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableADSAlert &&
                                                                               c.LoadInFirstTime &&
                                                                               c.ParentList.Count() == 0 &&
                                                                                      !c.IsRenge).ToList();
                        break;

                }


                foreach (CategoryFeature item in categoryFeatureList)
                {
                    model.Add(new CategoryFeatureValuesResultModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        values = item.FeatureValueList.Select(fv => new BaseValuesModel
                        {
                            featureValueId = fv.Id,
                            title = fv.Title,
                            hideList = fv.HideList.Select(h => h.CategoryFeatureHideId.Value).ToList(),
                            showList = fv.ShowList.Select(s => s.CategoryFeatureShowId.Value).ToList(),
                            disableList = fv.DisableList.Select(d => new CFDefaultValueViewModel
                            {
                                categoryFeatureId = d.DisableCategoryFeatureId.Value,
                                featureValueId = d.DisableValueId.HasValue ? d.DisableValueId.Value : 0,
                                customValue = d.DisableFeatureValueCustomValue ?? ""
                            }).ToList(),
                            enableList = fv.EnableList.Select(d => new CFDefaultValueViewModel
                            {
                                categoryFeatureId = d.EnableCategoryFeatureId.Value,
                                featureValueId = d.EnableValueId.HasValue ? d.EnableValueId.Value : 0,
                                customValue = d.EnableFeatureValueCustomValue ?? ""
                            }).ToList(),
                            showContainer = fv.ShowContainer ?? "",
                            hideContainer = fv.HideContainer ?? ""
                        }).ToList()
                    });

                }
                #endregion
            }

            else
            {
                #region filter
                List<CategoryFeature> categoryFeatureList = new List<CategoryFeature>();

                switch (Type)
                {

                    case FeatureConfigType.Advertise:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInADS &&
                                                                                !cf.HasMultiSelectValue &&
                                                                                !cf.HasCustomValue &&
                                                                                      !cf.IsRenge).ToList();

                        break;

                    case FeatureConfigType.Article:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInArticle &&
                                                                                !cf.HasMultiSelectValue &&
                                                                                !cf.HasCustomValue &&
                                                                                      !cf.IsRenge).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInReview &&
                                                                                      !cf.IsRenge).ToList();

                        break;
                    case FeatureConfigType.TodayPrice:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                               c.AvailableTodayPrice &&
                                                                                      !c.IsRenge).ToList();
                        break;

                    case FeatureConfigType.Alert:

                        categoryFeatureList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableADSAlert &&
                                                                                      !cf.IsRenge).ToList();
                        break;

                }


                #endregion


                List<FilterSelectTempModel> FilterList = new List<FilterSelectTempModel>();

                foreach (CategoryFeatureFilterModel item in categoryFeatureFilter)
                {
                    CategoryFeature CF = categoryFeatureList.Find(c => c.Id == item.CategoryFeatureId);

                    if (CF != null && item.FeatureValueId.Count > 0)
                    {
                        FeatureValue FV = CF.FeatureValueList.Find(fv => fv.Id == item.FeatureValueId.First());
                        if (FV != null)

                            FilterList.Add(new FilterSelectTempModel { CF = CF, SelectedFV = FV });
                    }
                }


                FilterList = FilterList.OrderBy(c => c.CF.OrderId).ToList();

                foreach (var item in FilterList)
                {
                    foreach (var child in item.CF.ChildeList)
                    {

                        var childCategoryFeature = categoryFeatureList.FirstOrDefault(c => c.Id == child.CategoryFeatureId);

                        if (childCategoryFeature != null)
                        {
                            var valueItems = childCategoryFeature.FeatureValueList.Where(cfv => cfv.ParentList.Any(p => p.FeatureValueId == item.SelectedFV.Id));


                            model.Add(new CategoryFeatureValuesResultModel
                            {
                                categoryFeatureId = childCategoryFeature.Id,
                                title = childCategoryFeature.Title,
                                values = valueItems.Select(fv => new BaseValuesModel
                                {
                                    featureValueId = fv.Id,
                                    title = fv.Title,
                                    hideList = fv.HideList.Select(h => h.CategoryFeatureHideId.Value).ToList(),
                                    showList = fv.ShowList.Select(s => s.CategoryFeatureShowId.Value).ToList(),
                                    showContainer = fv.ShowContainer ?? "",
                                    hideContainer = fv.HideContainer ?? "",
                                    disableList = fv.DisableList.Select(d => new CFDefaultValueViewModel
                                    {
                                        categoryFeatureId = d.DisableCategoryFeatureId.Value,
                                        featureValueId = d.DisableValueId.HasValue ? d.DisableValueId.Value : 0,
                                        customValue = d.DisableFeatureValueCustomValue
                                    }).ToList(),
                                    enableList = fv.EnableList.Select(d => new CFDefaultValueViewModel
                                    {
                                        categoryFeatureId = d.EnableCategoryFeatureId.Value,
                                        featureValueId = d.EnableValueId.HasValue ? d.EnableValueId.Value : 0,
                                        customValue = d.EnableFeatureValueCustomValue
                                    }).ToList(),

                                }).ToList()
                            });
                        }
                    }
                }
            }

            foreach (var item in categoryFeatureFilter)
            {
                if (model.Any(m => m.categoryFeatureId == item.CategoryFeatureId))
                {
                    model.RemoveAll(c => c.categoryFeatureId == item.CategoryFeatureId);
                }

            }


            return model;

        }

        public List<CategoryFeatureValuesResultModel> SearchFeatureValue(long categoryId,
                                                                        int type,
                                                                        bool isSimple,
                                                                        List<CategoryFeatureFilterModel> categoryFeatureFilter)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                return null;

            FeatureConfigType Type = (FeatureConfigType)type;

            List<CategoryFeatureValuesResultModel> model = new List<CategoryFeatureValuesResultModel>();


            if (categoryFeatureFilter.Count == 0)
            {
                List<CategoryFeature> categoryFeatuerList = new List<CategoryFeature>();

                #region count ==0
                switch (Type)
                {

                    case FeatureConfigType.Advertise:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInSearchBox &&
                                                                                cf.AvailableInADS &&
                                                                                cf.ParentList.Count == 0 &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();

                        break;

                    case FeatureConfigType.Article:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                      cf.AvailableInSearchBox &&
                                                                                      cf.AvailableInArticle &&
                                                                                      cf.ParentList.Count == 0 &&
                                                                                      !cf.IsRenge &&
                                                                                      !cf.HasCustomValue).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInSearchBox &&
                                                                                cf.AvailableInReview &&
                                                                                cf.ParentList.Count == 0 &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();
                        break;
                    case FeatureConfigType.TodayPrice:
                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableTodayPrice &&
                                                                                cf.ParentList.Count == 0 &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();

                        break;

                }

                if (isSimple)
                {
                    categoryFeatuerList = categoryFeatuerList.Where(cf => cf.AvailableInLigthSearch).ToList();
                }


                foreach (CategoryFeature item in categoryFeatuerList)
                {
                    model.Add(new CategoryFeatureValuesResultModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        values = item.FeatureValueList.Select(fv => new BaseValuesModel
                        {
                            featureValueId = fv.Id,
                            title = fv.Title,
                            hideList = fv.HideList.Select(h => h.CategoryFeatureHideId.Value).ToList(),
                            showList = fv.ShowList.Select(s => s.CategoryFeatureShowId.Value).ToList(),
                            disableList = fv.DisableList.Select(d => new CFDefaultValueViewModel
                            {
                                categoryFeatureId = d.DisableCategoryFeatureId.Value,
                                featureValueId = d.DisableValueId.HasValue ? d.DisableValueId.Value : 0,
                                customValue = d.DisableFeatureValueCustomValue ?? ""
                            }).ToList(),
                            enableList = fv.EnableList.Select(d => new CFDefaultValueViewModel
                            {
                                categoryFeatureId = d.EnableCategoryFeatureId.Value,
                                featureValueId = d.EnableValueId.HasValue ? d.EnableValueId.Value : 0,
                                customValue = d.EnableFeatureValueCustomValue ?? ""
                            }).ToList(),
                            showContainer = fv.ShowContainer ?? "",
                            hideContainer = fv.HideContainer ?? ""
                        }).ToList()
                    });

                }
                #endregion
            }

            else
            {
                #region filter
                List<CategoryFeature> categoryFeatuerList = new List<CategoryFeature>();

                switch (Type)
                {
                    case FeatureConfigType.Advertise:

                        categoryFeatuerList = categoryFeatuerList.Concat(_sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                 cf.AvailableInSearchBox &&
                                                                                 cf.AvailableInADS &&
                                                                                 !cf.IsRenge &&
                                                                                 !cf.HasCustomValue).ToList()).ToList();
                        break;

                    case FeatureConfigType.Article:

                        categoryFeatuerList = categoryFeatuerList.Concat(_sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInSearchBox &&
                                                                                cf.AvailableInArticle &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList()).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatuerList = categoryFeatuerList.Concat(_sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInReview &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList()).ToList();

                        break;


                    case FeatureConfigType.TodayPrice:
                        categoryFeatuerList = categoryFeatuerList.Concat(_sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableTodayPrice &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList()).ToList();

                        break;
                }

                if (isSimple)
                {
                    categoryFeatuerList = categoryFeatuerList.Where(cf => cf.AvailableInLigthSearch).ToList();
                }


                #endregion


                List<FilterSelectTempModel> FilterList = new List<FilterSelectTempModel>();

                foreach (CategoryFeatureFilterModel item in categoryFeatureFilter)
                {
                    CategoryFeature CF = categoryFeatuerList.Find(c => c.Id == item.CategoryFeatureId);

                    if (CF != null && item.FeatureValueId.Count > 0)
                    {
                        FeatureValue FV = CF.FeatureValueList.Find(fv => fv.Id == item.FeatureValueId.First());
                        if (FV != null)

                            FilterList.Add(new FilterSelectTempModel { CF = CF, SelectedFV = FV });
                    }
                }


                FilterList = FilterList.OrderBy(c => c.CF.OrderId).ToList();

                foreach (var item in FilterList)
                {
                    foreach (var child in item.CF.ChildeList)
                    {

                        var childCategoryFeature = categoryFeatuerList.FirstOrDefault(c => c.Id == child.CategoryFeatureId);

                        if (childCategoryFeature != null)
                        {
                            var valueItems = childCategoryFeature.FeatureValueList.Where(cfv => cfv.ParentList.Any(p => p.FeatureValueId == item.SelectedFV.Id));


                            model.Add(new CategoryFeatureValuesResultModel
                            {
                                categoryFeatureId = childCategoryFeature.Id,
                                title = childCategoryFeature.Title,
                                values = valueItems.Select(fv => new BaseValuesModel
                                {
                                    featureValueId = fv.Id,
                                    title = fv.Title,
                                    hideList = fv.HideList.Select(h => h.CategoryFeatureHideId.Value).ToList(),
                                    showList = fv.ShowList.Select(s => s.CategoryFeatureShowId.Value).ToList(),
                                    showContainer = fv.ShowContainer ?? "",
                                    hideContainer = fv.HideContainer ?? "",
                                    disableList = fv.DisableList.Select(d => new CFDefaultValueViewModel
                                    {
                                        categoryFeatureId = d.DisableCategoryFeatureId.Value,
                                        featureValueId = d.DisableValueId.HasValue ? d.DisableValueId.Value : 0,
                                        customValue = d.DisableFeatureValueCustomValue
                                    }).ToList(),
                                    enableList = fv.EnableList.Select(d => new CFDefaultValueViewModel
                                    {
                                        categoryFeatureId = d.EnableCategoryFeatureId.Value,
                                        featureValueId = d.EnableValueId.HasValue ? d.EnableValueId.Value : 0,
                                        customValue = d.EnableFeatureValueCustomValue
                                    }).ToList(),

                                }).ToList()
                            });
                        }
                    }
                }
            }

            foreach (var item in categoryFeatureFilter)
            {
                if (model.Any(m => m.categoryFeatureId == item.CategoryFeatureId))
                {
                    model.RemoveAll(c => c.categoryFeatureId == item.CategoryFeatureId);
                }

            }


            return model;

        }

        public object GetInitSearchFeatureValue(long categoryId,
                                                bool isSimple,
                                                int type,
                                                List<CategoryFeatureFilterModel> selectedItems,
                                                bool isMobile = false)
        {

            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                return null;

            FeatureConfigType Type = (FeatureConfigType)type;

            List<CategoryFeatureValuesResultModel> model = new List<CategoryFeatureValuesResultModel>();

            List<CategoryFeature> categoryFeatuerList = new List<CategoryFeature>();

            List<SelectItemFilterSearchModel> filterItem = new List<SelectItemFilterSearchModel>();

            #region count ==0

            switch (Type)
            {

                case FeatureConfigType.Advertise:

                    categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                            cf.AvailableInSearchBox &&
                                                                            cf.AvailableInADS &&
                                                                            cf.ParentList.Count == 0 &&
                                                                            !cf.IsRenge &&
                                                                            !cf.HasCustomValue).ToList();

                    break;

                case FeatureConfigType.Article:

                    categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                  cf.AvailableInSearchBox &&
                                                                                  cf.AvailableInArticle &&
                                                                                  cf.ParentList.Count == 0 &&
                                                                                  !cf.IsRenge &&
                                                                                  !cf.HasCustomValue).ToList();

                    break;
                case FeatureConfigType.Review:

                    categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                            cf.AvailableInSearchBox &&
                                                                            cf.AvailableInReview &&
                                                                            cf.ParentList.Count == 0 &&
                                                                            !cf.IsRenge &&
                                                                            !cf.HasCustomValue).ToList();
                    break;
                case FeatureConfigType.TodayPrice:
                    categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                            cf.AvailableTodayPrice &&
                                                                            cf.ParentList.Count == 0 &&
                                                                            !cf.IsRenge &&
                                                                            !cf.HasCustomValue).ToList();

                    break;

            }

            if (isSimple)
            {
                categoryFeatuerList = categoryFeatuerList.Where(cf => cf.AvailableInLigthSearch).ToList();
            }


            foreach (CategoryFeature item in categoryFeatuerList)
            {
                model.Add(new CategoryFeatureValuesResultModel
                {
                    categoryFeatureId = item.Id,
                    title = item.Title,
                    values = item.FeatureValueList.Select(fv => new BaseValuesModel
                    {
                        featureValueId = fv.Id,
                        title = fv.Title
                    }).ToList()
                });

            }

            #endregion

            if (selectedItems.Count > 0)
            {
                switch (Type)
                {
                    case FeatureConfigType.Advertise:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                 cf.AvailableInSearchBox &&
                                                                                 cf.AvailableInADS &&
                                                                                 !cf.IsRenge &&
                                                                                 !cf.HasCustomValue).ToList();
                        break;

                    case FeatureConfigType.Article:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInSearchBox &&
                                                                                cf.AvailableInArticle &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();

                        break;
                    case FeatureConfigType.Review:

                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableInReview &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();

                        break;


                    case FeatureConfigType.TodayPrice:
                        categoryFeatuerList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == category.Id &&
                                                                                cf.AvailableTodayPrice &&
                                                                                !cf.IsRenge &&
                                                                                !cf.HasCustomValue).ToList();

                        break;
                }

                if (isSimple)
                {
                    categoryFeatuerList = categoryFeatuerList.Where(cf => cf.AvailableInLigthSearch).ToList();
                }

                List<CategoryFeature> FilterList = new List<CategoryFeature>();

                foreach (CategoryFeatureFilterModel item in selectedItems)
                {

                    if (categoryFeatuerList.Any(c => c.Id == item.CategoryFeatureId))
                    {
                        FilterList.Add(categoryFeatuerList.SingleOrDefault(c => c.Id == item.CategoryFeatureId));
                    }
                }

                List<FeatureValue> ListFeatureValue = new List<FeatureValue>();

                foreach (var item in categoryFeatuerList)
                {
                    ListFeatureValue = ListFeatureValue.Concat(item.FeatureValueList).ToList();
                }

                List<FeatureValue> finalFv = new List<FeatureValue>();

                FilterList = FilterList.OrderBy(c => c.OrderId).ToList();

                foreach (var item in FilterList)
                {
                    foreach (var child in item.ChildeList)
                    {
                        var temp = ListFeatureValue.Where(fv => fv.CategoryFeatureId == child.CategoryFeatureId).ToList();

                        foreach (var selectfv in temp)
                        {
                            foreach (var findeitem in selectedItems.First(c => c.CategoryFeatureId == item.Id).FeatureValueId)
                                if (selectfv.ParentList.Any(p => p.FeatureValueId == findeitem))
                                {
                                    finalFv.Add(selectfv);

                                    break;
                                }
                        }

                    }
                }

                foreach (CategoryFeature item in categoryFeatuerList)
                {
                    model.Add(new CategoryFeatureValuesResultModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        values = finalFv.Where(fv => fv.CategoryFeatureId == item.Id).Select(fv => new BaseValuesModel
                        {
                            featureValueId = fv.Id,
                            title = fv.Title
                        }).ToList()
                    });

                }
            }
            
            return model.Where(m => m.values.Count != 0).ToList();
        }
        #endregion

        #region Config
        public object GetConfigForSearch(long categoryId, int type, bool isSimple, bool IsMobile)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            FeatureConfigType Type = (FeatureConfigType)type;

            if (category == null)
                return null;

            List<CategoryFeature> categoryFeatureList = new List<CategoryFeature>();


            switch (Type)
            {
                case FeatureConfigType.Advertise:

                    categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                           c.AvailableInSearchBox &&
                                                                           c.AvailableInADS &&
                                                                                      !c.IsRenge).ToList();

                    break;

                case FeatureConfigType.Article:

                    categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                           c.AvailableInSearchBox &&
                                                                           c.AvailableInArticle &&
                                                                                      !c.IsRenge).ToList();

                    break;
                case FeatureConfigType.Review:

                    categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                            c.AvailableInRVSearch &&
                                                                                      !c.IsRenge).ToList();

                    break;

                case FeatureConfigType.TodayPrice:

                    categoryFeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                           c.AvailableTodayPrice &&
                                                                           c.AvailableTPInSearch &&
                                                                                      !c.IsRenge).ToList();

                    break;

            }

            if (isSimple)
            {
                categoryFeatureList = categoryFeatureList.Where(c => c.AvailableInLigthSearch).ToList();

                foreach (var item in categoryFeatureList)
                {
                    item.ChildeList.RemoveAll(c => !categoryFeatureList.Any(e => e.Id == c.CategoryFeatureId));

                }
            }

            if (IsMobile)
            {

                List<AndroidCFConfigViewModel> model = new List<AndroidCFConfigViewModel>();

                foreach (CategoryFeature item in categoryFeatureList)
                {
                    AndroidCFConfigViewModel temp = new AndroidCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title
                    };

                    string androidType = item.AndroidElementId.HasValue ? _sdb.AndroidElements.Find(item.AndroidElementId).Type : "";

                    temp.androidType = androidType;

                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    model.Add(temp);
                }

                return model;

            }
            else
            {
                List<WebCFConfigViewModel> model = new List<WebCFConfigViewModel>();

                foreach (CategoryFeature item in categoryFeatureList)
                {
                    WebCFConfigViewModel temp = new WebCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title
                    };

                    Element WebType = item.ElementId.HasValue ? _sdb.Elements.Find(item.ElementId) : new Element
                    {
                        Title = "",
                        DefaulteClass = "",
                        HtmlId = "",
                        HtmlName = ""
                    };

                    temp.defaulteClass = WebType.DefaulteClass;
                    temp.elementTitle = WebType.Title;
                    temp.htmlId = WebType.HtmlId;
                    temp.htmlName = WebType.HtmlName;
                    temp.hasMultiSelectValue = item.HasMultiSelectValue;
                    temp.hasValueSearch = item.AvailableValueSearch;
                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    model.Add(temp);
                }

                return model;

            }

        }

        public object GetConfigForSearchAggregation(long categoryId, int type, bool IsMobile)
        {
            Category category = _sdb.Categoris.Find(categoryId);
            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            FeatureConfigType Type = (FeatureConfigType)type;

            if (category == null)
                return null;

            List<CategoryFeature> categoryfeatureList = new List<CategoryFeature>();


            switch (Type)
            {
                case FeatureConfigType.Advertise:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => (c.CategoryId == category.Id &&
                                                                           c.AvailableInSearchBox &&
                                                                           c.AvailableInADS &&
                                                                           !c.HasCustomValue
                                                                           ) || (c.CategoryId == category.Id && c.Element.Title == "Exchange")).ToList();

                    break;

                case FeatureConfigType.Article:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                           c.AvailableInSearchBox &&
                                                                           c.AvailableInArticle &&
                                                                           !c.HasCustomValue
                                                                           ).ToList();

                    break;
                case FeatureConfigType.Review:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                            c.AvailableInSearchBox &&
                                                                            c.AvailableInReview &&
                                                                            !c.HasCustomValue
                                                                            ).ToList();

                    break;

            }



            if (IsMobile)
            {

                List<AndroidCFConfigViewModel> model = new List<AndroidCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    AndroidCFConfigViewModel temp = new AndroidCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        hasMultiSelectValue = item.AvailableSearchMultiSelect
                    };

                    string androidType = item.AndroidElementId.HasValue ? _sdb.AndroidElements.Find(item.AndroidElementId).Type : "";

                    temp.androidType = androidType;

                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    model.Add(temp);
                }

                return model;

            }
            else
            {
                List<WebCFConfigViewModel> model = new List<WebCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    WebCFConfigViewModel temp = new WebCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title
                    };

                    Element WebType = item.ElementId.HasValue ? _sdb.Elements.Find(item.ElementId) : new Element
                    {
                        Title = "",
                        DefaulteClass = "",
                        HtmlId = "",
                        HtmlName = ""
                    };

                    temp.defaulteClass = WebType.DefaulteClass;
                    temp.elementTitle = WebType.Title;
                    temp.htmlId = WebType.HtmlId;
                    temp.htmlName = WebType.HtmlName;
                    temp.hasMultiSelectValue = item.AvailableSearchMultiSelect;
                    temp.hasValueSearch = item.AvailableValueSearch;
                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    model.Add(temp);
                }

                return model;

            }

        }

        public object GetConfigForManagment(long categoryId, int type, bool IsMobile)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            FeatureConfigType Type = (FeatureConfigType)type;

            if (category == null)
                return null;


            List<CategoryFeature> categoryfeatureList = new List<CategoryFeature>();

            switch (Type)
            {
                case FeatureConfigType.Advertise:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id && c.AvailableInADS &&
                                                                                      !c.IsRenge).ToList();

                    break;

                case FeatureConfigType.Article:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id && c.AvailableInArticle &&
                                                                                      !c.IsRenge).ToList();

                    break;
                case FeatureConfigType.Review:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                                      c.AvailableInReview &&
                                                                                      c.RequiredInRVInsert &&
                                                                                      !c.IsRenge).ToList();
                    break;
                case FeatureConfigType.TodayPrice:

                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id && c.AvailableTodayPrice &&
                                                                                      !c.IsRenge).ToList();
                    break;

                case FeatureConfigType.Alert:
                    categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                                   c.AvailableADSAlert &&
                                                                                      !c.IsRenge).ToList();
                    break;

            }

            categoryfeatureList = categoryfeatureList.OrderBy(cf => cf.OrderId).ToList();

            if (IsMobile)
            {

                List<AndroidCFConfigViewModel> model = new List<AndroidCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    AndroidCFConfigViewModel temp = new AndroidCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        regex = item.Pattern ?? "",
                        containerName = item.ContainerName ?? "",
                        hideContainer = item.HideContainer ?? ""

                    };

                    string androidType = item.AndroidElementId.HasValue ? _sdb.AndroidElements.Find(item.AndroidElementId).Type : "";

                    temp.androidType = androidType;
                    temp.required = RequierdType(Type, item);

                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    foreach (var g in item.HideList)
                    {
                        temp.hideList.Add(g.CategoryFeatureHideId.Value);
                    }

                    foreach (var g in item.EnableList)
                    {
                        temp.enableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.EnableCategoryFeatureId.Value,
                            featureValueId = g.EnableFeatureValueId.HasValue ? g.EnableFeatureValueId.Value : 0,
                            customValue = g.EnableFeatureValueCustomValue ?? ""
                        });
                    }

                    foreach (var g in item.DisableList)
                    {
                        temp.disableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.DisableCategoryFeatureId.Value,
                            featureValueId = g.DisableFeatureValueId.HasValue ? g.DisableCategoryFeatureId.Value : 0,
                            customValue = g.DisableFeatureValueCustomValue ?? ""
                        });
                    }



                    model.Add(temp);
                }

                return model;

            }
            else
            {
                List<WebCFConfigViewModel> model = new List<WebCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    WebCFConfigViewModel temp = new WebCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        isRequired = RequierdType(Type, item),
                        regex = item.Pattern ?? "",
                        containerName = item.ContainerName ?? "",
                        hideContainer = item.HideContainer ?? ""
                    };

                    Element WebType = item.ElementId.HasValue ? _sdb.Elements.Find(item.ElementId) : new Element
                    {
                        Title = "",
                        DefaulteClass = "",
                        HtmlId = "",
                        HtmlName = ""

                    };

                    temp.defaulteClass = WebType.DefaulteClass;
                    temp.elementTitle = WebType.Title;
                    temp.htmlId = WebType.HtmlId;
                    temp.htmlName = WebType.HtmlName;
                    temp.hasValueSearch = item.AvailableValueSearch;

                    foreach (var g in item.ChildeList)
                    {
                        temp.children.Add(g.CategoryFeatureId);
                    }

                    foreach (var g in item.HideList)
                    {
                        temp.hideList.Add(g.CategoryFeatureHideId.Value);
                    }

                    foreach (var g in item.EnableList)
                    {
                        temp.enableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.EnableCategoryFeatureId.Value,
                            featureValueId = g.EnableFeatureValueId.HasValue ? g.EnableFeatureValueId.Value : 0,
                            customValue = g.EnableFeatureValueCustomValue ?? ""
                        });
                    }

                    foreach (var g in item.DisableList)
                    {
                        temp.disableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.DisableCategoryFeatureId.Value,
                            featureValueId = g.DisableFeatureValueId.HasValue ? g.DisableCategoryFeatureId.Value : 0,
                            customValue = g.DisableFeatureValueCustomValue ?? ""
                        });
                    }

                    model.Add(temp);
                }

                return model;

            }

        }

        public object GetExchangeConfigForManagment(long categoryId, int type, bool IsMobile)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            FeatureConfigType Type = (FeatureConfigType)type;

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);


            List<CategoryFeature> categoryfeatureList = new List<CategoryFeature>();



            categoryfeatureList = _sdb.CategoryFeatures.Where(c => c.CategoryId == category.Id &&
                                                                    c.AvailableInADS &&
                                                                    c.AvailableInExchange &&
                                                                    !c.IsRenge).OrderBy(c => c.OrderId).ToList();


            if (IsMobile)
            {

                List<AndroidCFConfigViewModel> model = new List<AndroidCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    AndroidCFConfigViewModel temp = new AndroidCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title,
                        regex = item.Pattern ?? ""
                    };

                    string androidType = item.AndroidElementId.HasValue ? _sdb.AndroidElements.Find(item.AndroidElementId).Type : "";

                    temp.androidType = androidType;
                    temp.required = false;
                    temp.containerName = item.ContainerName;
                    temp.hideContainer = item.HideContainer;

                    foreach (var g in item.ChildeList)
                    {
                        if (categoryfeatureList.FirstOrDefault(c => c.Id == g.CategoryFeatureId) != null &&
                            categoryfeatureList.FirstOrDefault(c => c.Id == g.CategoryFeatureId).AvailableInExchange)
                            temp.children.Add(g.CategoryFeatureId);
                    }

                    foreach (var g in item.HideList)
                    {
                        temp.hideList.Add(g.CategoryFeatureHideId.Value);
                    }

                    foreach (var g in item.EnableList)
                    {
                        temp.enableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.EnableCategoryFeatureId.Value,
                            featureValueId = g.EnableFeatureValueId.HasValue ? g.EnableFeatureValueId.Value : 0,
                            customValue = g.EnableFeatureValueCustomValue ?? ""
                        });
                    }

                    foreach (var g in item.DisableList)
                    {
                        temp.disableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.DisableCategoryFeatureId.Value,
                            featureValueId = g.DisableFeatureValueId.HasValue ? g.DisableCategoryFeatureId.Value : 0,
                            customValue = g.DisableFeatureValueCustomValue ?? ""
                        });
                    }

                    model.Add(temp);
                }

                return model;

            }
            else
            {
                List<WebCFConfigViewModel> model = new List<WebCFConfigViewModel>();

                foreach (CategoryFeature item in categoryfeatureList)
                {
                    WebCFConfigViewModel temp = new WebCFConfigViewModel
                    {
                        categoryFeatureId = item.Id,
                        title = item.Title
                    };

                    Element WebType = item.ElementId.HasValue ? _sdb.Elements.Find(item.ElementId) : new Element
                    {
                        Title = "",
                        DefaulteClass = "",
                        HtmlId = "",
                        HtmlName = ""

                    };

                    temp.defaulteClass = WebType.DefaulteClass;
                    temp.elementTitle = WebType.Title;
                    temp.htmlId = WebType.HtmlId;
                    temp.htmlName = WebType.HtmlName;
                    temp.hasValueSearch = item.AvailableValueSearch;

                    foreach (var g in item.ChildeList)
                    {
                        if (categoryfeatureList.FirstOrDefault(c => c.Id == g.CategoryFeatureId) != null &&
                             categoryfeatureList.FirstOrDefault(c => c.Id == g.CategoryFeatureId).AvailableInExchange)
                            temp.children.Add(g.CategoryFeatureId);
                    }
                    foreach (var g in item.HideList)
                    {
                        temp.hideList.Add(g.CategoryFeatureHideId.Value);
                    }

                    foreach (var g in item.EnableList)
                    {
                        temp.enableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.EnableCategoryFeatureId.Value,
                            featureValueId = g.EnableFeatureValueId.HasValue ? g.EnableFeatureValueId.Value : 0,
                            customValue = g.EnableFeatureValueCustomValue ?? ""
                        });
                    }

                    foreach (var g in item.DisableList)
                    {
                        temp.disableList.Add(new CFDefaultValueViewModel
                        {
                            categoryFeatureId = g.DisableCategoryFeatureId.Value,
                            featureValueId = g.DisableFeatureValueId.HasValue ? g.DisableCategoryFeatureId.Value : 0,
                            customValue = g.DisableFeatureValueCustomValue ?? ""
                        });
                    }

                    model.Add(temp);
                }

                return model;

            }

        }
        #endregion

        #region initEdit
        public InitEditAdvertiseViewModel InitEditAdvertise(long userId, long advertiseId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            Product product = _sdb.Product.Find(advertiseId);

            if (product == null || product.UserId != user.Id)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            InitEditAdvertiseViewModel model = new InitEditAdvertiseViewModel
            {

                insertDate = product.InsertDate,
                tell = product.Tell,
                description = product.Description ?? "",
                selectedDealershipId = product.DealershipId,
                exchangeCategoryId = product.CategoryExchangeId,
                selectedPackageTypeId = product.UserPackageCreditId.Value,
                categoryId = product.CategoryId,
                maxMediaCount = this.PackageMediaCount(product.Id)
            };

            var media = _sdb.Media.Where(m => m.EntityType == Domain.Constant.Media.EntityType.Product && m.EntityId == product.Id).ToList();

            if (media.Count > 0)
            {
                model.media = media.Select(m => new ProductMediaEditViewModel
                {
                    url = m.MediaURL,
                    mediaId = m.Id,
                    orderId = m.Order
                }).ToList();
            }

            List<CategoryFeatureFilterModel> categoryFeatureFilter = new List<CategoryFeatureFilterModel>();

            List<CategoryFeature> CategoryFeature = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == product.CategoryId &&
                                                                                        cf.AvailableInADS).ToList();

            foreach (var item in CategoryFeature)
            {

                List<CategoryFeatureValuesResultModel> selectedItem = this.ManageFeatureValue(categoryId: product.CategoryId,
                                                                                                type: 0,
                                                                                                categoryFeatureFilter: categoryFeatureFilter);

                foreach (var CFVItem in selectedItem)
                {
                    if (item.Id == CFVItem.categoryFeatureId)
                    {
                        model.categoryFeatureValues.Add(CFVItem);
                    }
                    else
                    {
                        if (item.ChildeList.Any(ch => ch.CategoryFeatureId == CFVItem.categoryFeatureId))
                            model.categoryFeatureValues.Add(CFVItem);

                        if (CategoryFeature.First(cf => cf.Id == CFVItem.categoryFeatureId).ParentList.Count == 0)
                            model.categoryFeatureValues.Add(CFVItem);
                    }
                }

                if (product.ProductFeatures.Any(cf => cf.CategoryFeatureId == item.Id) && !item.HasCustomValue)
                {
                    categoryFeatureFilter.Add(new CategoryFeatureFilterModel
                    {
                        CategoryFeatureId = item.Id,
                        FeatureValueId = product.ProductFeatures.First(cf => cf.CategoryFeatureId == item.Id).ListFeatureValue.Select(fv => fv.FeatureValueId).ToList()
                    });
                }

            }

            foreach (var item in product.ProductFeatures)
            {
                var selectItem = new ProductselectedItemViewModel
                {
                    categoryFeatureId = item.CategoryFeatureId,
                };

                if (item.CategoryFeature.HasCustomValue)
                {
                    selectItem.customValue = item.CustomValue ?? "";
                }
                else
                {
                    selectItem.featureValueIds = item.ListFeatureValue.Select(fv => fv.FeatureValueId).ToList();
                }

                model.selectedItems.Add(selectItem);

            }

            if (product.DealershipId.HasValue)
            {
                long count = 0;

                DealershipService _DealershipService = new DealershipService(sdb: ref _sdb, mdb: ref _mdb);

                model.dealerships = _DealershipService.ListDelership(userId: product.UserId,
                                                                      count: ref count,
                                                                       size: int.MaxValue,
                                                                 categoryId: product.CategoryId,
                                                                       skip: 0,
                                                                     status: 1)
                                                .Select(d => new DealershipViewModel
                                                {
                                                    dealershipId = d.id,
                                                    title = d.title
                                                }).ToList();
            }

            UserService _UserService = new UserService(ref _sdb);
            model.packageDetail = _UserService.packageDetailConfig(userId: user.Id, userPackageCreditId: product.UserPackageCreditId.Value);
            model.selectedItems = model.selectedItems.OrderBy(si => si.categoryFeatureId).ToList();
            model.categoryFeatureValues = model.categoryFeatureValues.OrderBy(cf => cf.categoryFeatureId).ToList();
            return model;
        }

        public InitEditAlertViewModel InitEditAlert(long userId, long alertId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            AlertProduct AlertProduct = _sdb.AlertProduct.Find(alertId);

            if (AlertProduct == null || AlertProduct.UserId != user.Id)
                throw new RecabException((int)ExceptionType.AlertNotFound);

            InitEditAlertViewModel model = new InitEditAlertViewModel
            {
                status = AlertProduct.Status.ToString(),
                insertDate = AlertProduct.InsertDate.UTCToPersianDateLong(),
                expireDate = AlertProduct.ExpireDate.UTCToPersianDateLong(),
                title = AlertProduct.Title,
                categoryId = AlertProduct.CategoryId,
                sendEmail = AlertProduct.SendEmail,
                sendSMS = AlertProduct.SendSMS  ,
                sendPush = AlertProduct.SendPush
            };

            List<CategoryFeatureFilterModel> categoryFeatureFilter = new List<CategoryFeatureFilterModel>();

            List<CategoryFeature> CategoryFeature = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == AlertProduct.CategoryId &&
                                                                                        cf.AvailableADSAlert).ToList();

            List<FeatureValueAssign> AssignedItems = _sdb.FeatureValueAssign.Where(FVA => FVA.EntityId == AlertProduct.Id &&
                                                                                          FVA.EntityType == EntityType.Alert).ToList();

            foreach (var item in CategoryFeature)
            {

                List<CategoryFeatureValuesResultModel> selectedItem = this.ManageFeatureValue(categoryId: AlertProduct.CategoryId,
                                                                                                 type: 4,
                                                                                                categoryFeatureFilter: categoryFeatureFilter);

                foreach (var CFVItem in selectedItem)
                {
                    if (item.Id == CFVItem.categoryFeatureId)
                    {
                        if (!model.categoryFeatureValues.Any(cf => cf.categoryFeatureId == CFVItem.categoryFeatureId))
                            model.categoryFeatureValues.Add(CFVItem);
                    }
                    else
                    {
                        if (item.ChildeList.Any(ch => ch.CategoryFeatureId == CFVItem.categoryFeatureId))
                        {
                            if (!model.categoryFeatureValues.Any(cf => cf.categoryFeatureId == CFVItem.categoryFeatureId))
                                model.categoryFeatureValues.Add(CFVItem);
                        }

                        if (CategoryFeature.First(cf => cf.Id == CFVItem.categoryFeatureId).ParentList.Count == 0)
                        {
                            if (!model.categoryFeatureValues.Any(cf => cf.categoryFeatureId == CFVItem.categoryFeatureId))
                                model.categoryFeatureValues.Add(CFVItem);
                        }
                    }
                }

                if (AssignedItems.Any(cf => cf.CategoryFeatureId == item.Id) && !item.HasCustomValue)
                {
                    categoryFeatureFilter.Add(new CategoryFeatureFilterModel
                    {
                        CategoryFeatureId = item.Id,
                        FeatureValueId = AssignedItems.First(cf => cf.CategoryFeatureId == item.Id).ListFeatureValue.Select(fv => fv.FeatureValueId).ToList()
                    });
                }

            }

            foreach (var item in AssignedItems)
            {
                var selectItem = new ProductselectedItemViewModel
                {
                    categoryFeatureId = item.CategoryFeatureId,
                };

                if (item.CategoryFeature.HasCustomValue)
                {
                    selectItem.customValue = item.CustomValue ?? "";
                }
                else
                {
                    selectItem.featureValueIds = item.ListFeatureValue.Select(fv => fv.FeatureValueId).ToList();
                }
                if (!model.selectedItem.Any(s => s.categoryFeatureId == selectItem.categoryFeatureId))
                    model.selectedItem.Add(selectItem);

            }




            model.selectedItem = model.selectedItem.Distinct().OrderBy(si => si.categoryFeatureId).ToList();
            model.categoryFeatureValues = model.categoryFeatureValues.OrderBy(cf => cf.categoryFeatureId).ToList();
            return model;
        }

        public InitProductExchangeViewModel InitEditProductExchange(long userId, long productId)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            Product product = _sdb.Product.Find(productId);

            if (product == null || product.UserId != user.Id)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            if (!product.CategoryExchangeId.HasValue)
                throw new RecabException((int)ExceptionType.ProductHasNoExchange);

            List<CategoryFeatureFilterModel> categoryFeatureFilter = new List<CategoryFeatureFilterModel>();

            List<CategoryFeature> CategoryFeature = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == product.CategoryExchangeId.Value &&
                                                                                        cf.AvailableInExchange).ToList();
            InitProductExchangeViewModel model = new InitProductExchangeViewModel();

            model.exchangeCategory = product.Category.CategoryExchanges.Select(cx => new ExchangeCategoryViewModel { categoryId = cx.ExchangeCategoryId, title = _sdb.Categoris.Find(cx.ExchangeCategoryId).Title }).ToList();
            foreach (var item in CategoryFeature)
            {

                List<CategoryFeatureValuesResultModel> selectedItem = this.ManageFeatureValue(categoryId: product.CategoryExchangeId.Value,
                                                                                                type: 0,
                                                                                                categoryFeatureFilter: categoryFeatureFilter);

                foreach (var CFVItem in selectedItem)
                {
                    if (item.Id == CFVItem.categoryFeatureId)
                    {
                        model.advertiseCategoryFeatureValues.Add(CFVItem);
                    }
                    else
                    {
                        if (item.ChildeList.Any(ch => ch.CategoryFeatureId == CFVItem.categoryFeatureId))
                            model.advertiseCategoryFeatureValues.Add(CFVItem);

                        if (CategoryFeature.FirstOrDefault(cf => cf.Id == CFVItem.categoryFeatureId) != null ? CategoryFeature.First(cf => cf.Id == CFVItem.categoryFeatureId).ParentList.Count == 0 : false)
                            model.advertiseCategoryFeatureValues.Add(CFVItem);
                    }
                }

                if (product.ProductFeatures.Any(cf => cf.CategoryFeatureId == item.Id) && !item.HasCustomValue)
                {

                    if (product.ExchangeFeatureValues.FirstOrDefault(cf => cf.CategoryFeatureId == item.Id) != null ? product.ExchangeFeatureValues.First(cf => cf.CategoryFeatureId == item.Id).FeatureValueId.HasValue : false)
                    {
                        var t = new List<long>();
                        t.Add(product.ExchangeFeatureValues.First(cf => cf.CategoryFeatureId == item.Id).FeatureValueId.Value);

                        categoryFeatureFilter.Add(new CategoryFeatureFilterModel
                        {
                            CategoryFeatureId = item.Id,
                            FeatureValueId = t
                        });
                    }
                }
            }

            foreach (var item in product.ExchangeFeatureValues)
            {
                var selectItem = new ExchangeProductselectedItemViewModel
                {
                    categoryFeatureId = item.CategoryFeatureId,
                };

                if (item.CategoryFeature.HasCustomValue)
                {
                    selectItem.customValue = item.CustomValue ?? "";
                }
                else
                {
                    selectItem.featureValueIds.Add(item.FeatureValueId.Value);
                }

                model.exchangeCategoryFeatureValues.Add(selectItem);

            }


            return model;



        }

        #endregion

        #region Private

        private bool RequierdType(FeatureConfigType type, CategoryFeature categoryFeature)
        {
            switch (type)
            {
                case FeatureConfigType.Advertise:
                    return categoryFeature.RequiredInADInsert;

                case FeatureConfigType.Article:
                    return categoryFeature.RequiredInATInsert;

                case FeatureConfigType.Review:
                    return categoryFeature.RequiredInRVInsert;

                case FeatureConfigType.TodayPrice:
                    return categoryFeature.AvailableTodayPrice;

                case FeatureConfigType.Alert:
                    return categoryFeature.RequiredInADSAlertInsert;

                default:
                    throw new RecabException();
            }


        }

        private string PackageMediaCount(long productId)
        {
            Product product = _sdb.Product.Find(productId);

            if (product == null)
                throw new RecabException((int)ExceptionType.ProductNotFound);

            var conf = product.UserPackageCredit.CategoryPurchasePackageType.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.PictureCount);
            return conf != null ? conf.Value : "";
        }

        #endregion
    }
}
