using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Exon.Recab.Service.Model.AdminModel
{
    public class CategoryFeatureViewModel
    {
        public long categoryFeatureId { get; set; }

        #region ADS
        //در آگهی
  
        public bool ads_AvailableInADS { get; set; }


        // در جستجوی سبک
  
        public bool ads_AvailableInLigthSearch { get; set; }

        //در کادر جستجو
  
        public bool ads_AvailableInSearchBox { get; set; }

        //در بازخورد
  
        public bool ads_AvailableInFeedBack { get; set; }

        //در نتایج جست وجو
  
        public bool ads_AvailableInSearchResult { get; set; }

        // موجود در مقایسه آگهی
  
        public bool ads_AvailableInADCompare { get; set; }

        //مجود در عنوان آگهی
  
        public bool ads_AvailableAdTitle { get; set; }

        //موجودر در جستوجوی متنی
  
        public bool ads_AvailableInADTextSearch { get; set; }

        //مجود در معاوضه
  
        public bool ads_AvailableInExchange { get; set; }

        //اجباری 
  
        public bool ads_RequiredInInsert { get; set; }

        //چند مقداری در جست و جو
  
        public bool ads_AvailableInSearchMulti { get; set; }

        public string ads_ContainerName { get; set; }

        public string ads_HideContainer { get; set; }

        public bool ads_AvailableInAlert { get; set; }

        #endregion

        #region Review

        //در برسی فنی

        public bool rew_AvailableInReview { get; set; }

        //موجود در عنوان برسی فنی
  
        public bool rew_AvailableInRVTitle { get; set; }


        //جشتوجوی برسی فنی
  
        public bool rew_AvailableInRVSearch { get; set; }

        //اجباری در برسی فنی
  
        public bool rew_RequiredInRvInsert { get; set; }
        //جستو جوی متنی برسی قنی
  
        public bool rew_AvailableInRVTextSearch { get; set; }


        #endregion

        #region Article
        //اجباری در مقاله
  
        public bool art_RequiredInATInsert { get; set; }

        //عنوان مقاله
  
        public bool art_AvailableInATTitle { get; set; }

        //موجود در جست وجوی مقاله
  
        public bool art_AvailableInATsearch { get; set; }

        //موجود در مقاله
  
        public bool art_AvailableInArticle { get; set; }

        //جستو جوی متنی مقاله
  
        public bool art_AvailableInATTextSearch { get; set; }
        #endregion

        #region Public
        //عنوان
  
        public string pub_Title { get; set; }

        //الگوی محتوا
        public string pub_Pattern { get; set; }

        //ترتیب نمایش
  
        public int pub_SearchOrderId { get; set; }


  
        public long pub_CategoryId { get; set; }


        //نمایش مقادیر در جستجو برای اولین بار
  
        public bool pub_LoadInFirstTime { get; set; }

        //مقدار ورودی
  
        public bool pub_HasCustomValue { get; set; }

        //چند مقداری
  
        public bool pub_HasMultiSelectValue { get; set; }

        //کنترل وب
  
        public long pub_HtmlElementId { get; set; }

        //کنترل اندروید
  
        public long pub_AndroidElementId { get; set; }

        //کنترل محدوده
        public long? pub_CategoryFeatureRangeId { get; set; }

        //آیکن ویژگی
  
        public bool pub_AvailableCFIcon { get; set; }

        //آیکن مقدار
  
        public bool pub_AvailableFVIcon { get; set; }

        //ترتیب در عنوان
        public int? pub_TitleOrder { get; set; }

        //نقشه
  
        public bool pub_IsMap { get; set; }

        //محدوده ای
  
        public bool pub_IsRang { get; set; }

        //ادرس آیکن
        public string pub_IconUrl { get; set; }

        public bool pub_HasValueSearch { get; set; }
        #endregion

        #region TodayPrice
        //موجود در نرخ روز

        public bool tdp_AvailableInTodayPrice { get; set; }

        //جستو جوی نرخ روز
  
        public bool tdp_AvailableInTPSearch { get; set; }
       

        public bool tdp_RequierdInTPInsert { get;  set; }

        public int ads_ReletiveAdsOrder { get;  set; }

        public bool ads_RequiredInAlertInsert { get;  set; }


        #endregion
    }
}
