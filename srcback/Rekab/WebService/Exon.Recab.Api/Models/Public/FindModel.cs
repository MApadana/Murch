using Exon.Recab.Api.Infrastructure.Resource;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Public
{
    public class FindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long id { get; set; }

        public string title { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }

        public FindModel()
        {
            id = 0;
            pageSize = 20;
            pageIndex = 0;
        }
    }
}