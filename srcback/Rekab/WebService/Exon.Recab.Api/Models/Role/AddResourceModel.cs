using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.Role
{
    public class AddResourceModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string title { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public string url { get; set; }

        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public ResourceType type { get; set; }

        public long? parentId { get;  set; }
    }

    public enum ResourceType
    {
        homePageMenu = 0,
        dashBordMenu = 1,
        action = 2,
        none = 3



    }

}
