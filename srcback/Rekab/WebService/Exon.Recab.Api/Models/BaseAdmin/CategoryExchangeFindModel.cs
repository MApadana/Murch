using Exon.Recab.Api.Infrastructure.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.BaseAdmin
{
    public class CategoryExchangeFindModel
    {
        [Required(ErrorMessageResourceType = typeof(ModelStateResource), ErrorMessageResourceName = "Required")]
        public long categoryId { get; set; }

        public int pageIndex { get;  set; }
        public int pageSize { get;  set; }

        public CategoryExchangeFindModel()
        {
            pageIndex = 0;
            pageSize = 1;
        }
    }
}
