using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class AddFeatureValueModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryFeaturId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string title { get; set; }   
        
        public string description { get; set; }

        public string pub_IconUrl { get; set; }

        public string hideContainer { get;  set; }

        public string showContainer { get; set; }
    }
}