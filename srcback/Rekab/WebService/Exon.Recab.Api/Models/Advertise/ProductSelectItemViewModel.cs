using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Advertise
{
    public class productSelectItemViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryFeatureId { get; set; }

        //rename2 : featureValueIds
        public List<long> featureValueIds { get; set; }

        public string customValue { get; set; }

        public productSelectItemViewModel()
        {
            featureValueIds = new List<long>();
        }
    }
}
