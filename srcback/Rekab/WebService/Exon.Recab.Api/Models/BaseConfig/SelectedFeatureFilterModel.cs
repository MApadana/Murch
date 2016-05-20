using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.BaseConfig
{
	public class SelectedFeatureFilterModel
	{
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string categoryFeatureId { get; set; }

        public List<string> featureValueIds { get; set; }
       

        public SelectedFeatureFilterModel()
        {
            featureValueIds = new List<string>();
        }
	}
}