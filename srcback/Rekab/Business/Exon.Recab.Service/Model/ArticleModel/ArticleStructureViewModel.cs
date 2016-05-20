using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ArticleModel
{
    public class ArticleStructureViewModel
    {
        public string title { get; set; }

        public long articleStructureId { get; set; }    

        public long articleCount { get; set; }

        public string logoUrl { get; set; }
    }
}
