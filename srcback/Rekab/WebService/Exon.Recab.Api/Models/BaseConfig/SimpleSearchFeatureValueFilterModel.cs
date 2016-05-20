using Exon.Recab.Api.Infrastructure.Resource;
using Exon.Recab.Api.Utility.Constant;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.BaseConfig
{
	public class SimpleSearchFeatureValueFilterModel
	{
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public SearchType searchType { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public bool isSimple { get; set; }

        public List<SelectedFeatureFilterModel> selectedItems { get; set; }

		public SimpleSearchFeatureValueFilterModel()
		{
			selectedItems = new List<SelectedFeatureFilterModel>();
		}

	}
}