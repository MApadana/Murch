
using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;


namespace Exon.Recab.Api.Models.User
{
    public class UserFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long userId { get; set; }

        public int pageSize { get; set; }
       
        public int pageIndex { get; set; }

        public UserFindModel()
        {
            pageSize = 1;

            pageIndex = 0;
        }

    }    
}
