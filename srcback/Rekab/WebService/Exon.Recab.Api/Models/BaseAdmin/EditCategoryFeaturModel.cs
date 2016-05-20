using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class EditCategoryFeaturModel
    {

        public long categoryFeatureId { get; set; }

        #region ADS
        //در آگهی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInADS { get; set; }


        // در جستجوی سبک
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInLigthSearch { get; set; }

        //در کادر جستجو
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInSearchBox { get; set; }

        //در بازخورد
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInFeedBack { get; set; }

        //در نتایج جست وجو
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInSearchResult { get; set; }

        // موجود در مقایسه آگهی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInADCompare { get; set; }

        //مجود در عنوان آگهی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableAdTitle { get; set; }

        //موجودر در جستوجوی متنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInADTextSearch { get; set; }

        //مجود در معاوضه
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInExchange { get; set; }

        //اجباری 
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_RequiredInInsert { get; set; }

        //چند مقداری در جست و جو
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_AvailableInSearchMulti { get; set; }

        public bool ads_AvailableInAlert { get; set; }

        public string ads_ContainerName { get; set; }

        public string ads_HideContainer { get; set; }

        #endregion

        #region Review

        //در برسی فنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool rew_AvailableInReview { get; set; }

        //موجود در عنوان برسی فنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool rew_AvailableInRVTitle { get; set; }


        //جشتوجوی برسی فنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool rew_AvailableInRVSearch { get; set; }

        //اجباری در برسی فنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool rew_RequiredInRvInsert { get; set; }
        //جستو جوی متنی برسی قنی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool rew_AvailableInRVTextSearch { get; set; }


        #endregion

        #region Article
        //اجباری در مقاله
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool art_RequiredInATInsert { get; set; }

        //عنوان مقاله
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool art_AvailableInATTitle { get; set; }

        //موجود در جست وجوی مقاله
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool art_AvailableInATsearch { get; set; }

        //موجود در مقاله
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool art_AvailableInArticle { get; set; }

        //جستو جوی متنی مقاله
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool art_AvailableInATTextSearch { get; set; }
        #endregion

        #region Public
        //عنوان
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string pub_Title { get; set; }

        //الگوی محتوا
        public string pub_Pattern { get; set; }

        //ترتیب نمایش
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public int pub_SearchOrderId { get; set; }


        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long pub_CategoryId { get; set; }


        //نمایش مقادیر در جستجو برای اولین بار
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_LoadInFirstTime { get; set; }

        //مقدار ورودی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_HasCustomValue { get; set; }

        //چند مقداری
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_HasMultiSelectValue { get; set; }

        //کنترل وب
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long pub_HtmlElementId { get; set; }

        //کنترل اندروید
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long pub_AndroidElementId { get; set; }

        //کنترل محدوده
        public long? pub_CategoryFeatureRangeId { get; set; }

        //آیکن ویژگی
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_AvailableCFIcon { get; set; }

        //آیکن مقدار
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_AvailableFVIcon { get; set; }

        //ترتیب در عنوان
        public int? pub_TitleOrder { get; set; }

        //نقشه
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_IsMap { get; set; }

        //محدوده ای
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool pub_IsRang { get; set; }

        //ادرس آیکن
        public string pub_IconUrl { get; set; }

        public bool pub_HasValueSearch { get; set; }

   
        #endregion

        #region TodayPrice
        //موجود در نرخ روز
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool tdp_AvailableInTodayPrice { get; set; }

        //جستو جوی نرخ روز
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool tdp_AvailableInTPSearch { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool tdp_RequierdInTPInsert { get;  set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public int ads_ReletiveAdsOrder { get;set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool ads_RequiredInAlertInsert { get; set; }



        #endregion


    }
}