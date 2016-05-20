using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exon.Recab.Domain.Entity
{
	public class CategoryFeature :BaseEntity
	{
        [Required]   
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }        

		//html element for view
		public long? ElementId { get; set; }

		[ForeignKey("ElementId")]
		public virtual Element Element { get; set; }

        public long? AndroidElementId { get; set; }

        [ForeignKey("AndroidElementId")]
        public virtual AndroidElement AndroidElement { get; set; }

        public long? CategoryFeatureRangeId { get; set; }

        public virtual List<FeatureValue> FeatureValueList { get; set; }

        public virtual List<CategoryFeatureDependency> ChildeList { get; set; }

        public virtual List<CategoryFeatureDependency> ParentList { get; set; }

        public virtual List<CategoryFeatureDependency> HideList { get; set; }

        public virtual List<CategoryFeatureDependency> ShowList { get; set; }

        public virtual List<CategoryFeatureDefaultValue> DisableList { get; set; }

        public virtual List<CategoryFeatureDefaultValue> EnableList { get; set; }

        [MaxLength(300)]
        public string ContainerName { get; set; }

        [MaxLength(3000)]
        public string ShowContainer { get; set; }

        [MaxLength(3000)]
        public string HideContainer { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }

        [MaxLength(150)]
        public string Pattern { get; set; }

        #region Advertise

        public int OrderId { get; set; }

        public bool AvailableInADS { get; set; }

        //توضیحات در جزییات آگهی قرار دارد ولی در فیلد های جست وجو نیست
        public bool AvailableInSearchBox { get; set; }

        public bool AvailableInLigthSearch { get; set; }

        //فیلد هایی که در خروجی اولیه جستو جو نمایش داده شود
        public bool AvailableInSearchResult { get; set; }

        public bool LoadInFirstTime { get; set; }        

        public bool RequiredInADInsert { get; set; }

        public bool AvailableInExchange { get; set; }

        public bool AvailableInTitle { get; set; }

        public bool AvailableSearchMultiSelect { get; set; }

        public bool AvailableInADCompaire { get; set; }

        public bool AvailableInADTextSearch { get; set; }

        public bool AvailableInRelativeADS { get; set; }

        public int RelativeADSOrder { get; set; }

        #region Alert

        public bool AvailableADSAlert { get; set; }

        public bool RequiredInADSAlertInsert { get; set; }
        #endregion

        #endregion

        #region Review
        public bool AvailableInReview { get; set; }

        public bool AvailableInRVSearch { get; set; }

        public bool AvailableInRVTitle{ get; set; }

        public bool RequiredInRVInsert { get; set; }

        public bool AvailableInRVTextSearch { get; set; }

        #endregion

        #region   icon
        public bool AvailableIcon { get; set; }

        // public string IconUrl { get; set; }

        public bool AvailableFVIcon { get; set; }

        #endregion

        #region  Article
        public bool AvailableInArticle { get; set; }

        public bool RequiredInATInsert { get; set; }

        public bool AvailableInATSearch { get; set; }

        public bool AvailableInATTitle { get; set; }

        public bool AvailableInATTextSearch { get; set; }
        #endregion

        #region  Feedback
        public bool AvailableInFeedback { get; set; }
        #endregion

        #region Today price

        public bool AvailableTodayPrice { get; set; }

        public bool AvailableTPInSearch { get; set; }

        public bool RequierdInTPInsert { get; set; }

        #endregion

        #region public
        public bool HasCustomValue { get; set; }

        public bool HasMultiSelectValue { get; set; }

        public int TitleOrder { get; set; }

        public bool IsMap { get; set; }

        public bool IsRenge { get; set; }

        public bool AvailableValueSearch { get; set; }

        #endregion

        public CategoryFeature()
        {
            FeatureValueList = new List<FeatureValue>();

            ChildeList = new List<CategoryFeatureDependency>();

            ParentList = new List<CategoryFeatureDependency>();

            HideList = new List<CategoryFeatureDependency>();

            DisableList = new List<CategoryFeatureDefaultValue>();

            EnableList = new List<CategoryFeatureDefaultValue>();
        }

	}
}
