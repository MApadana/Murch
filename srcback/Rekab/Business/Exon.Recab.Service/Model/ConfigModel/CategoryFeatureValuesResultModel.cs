using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ConfigModel
{
	public class CategoryFeatureValuesResultModel
	{
		public long categoryFeatureId { get; set; }

		public string title { get; set; }

		public List<BaseValuesModel> values { get; set; }

		public CategoryFeatureValuesResultModel()
		{
			values = new List<BaseValuesModel>();
		}
	}
}
