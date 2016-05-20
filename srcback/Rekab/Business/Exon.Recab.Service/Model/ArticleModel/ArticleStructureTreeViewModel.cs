using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ArticleModel
{
   public class ArticleStructureTreeViewModel
    {
        public string articleStructureTitle { get; set; }

        public long articleStructureId { get; set; }

        public long articleCount { get; set; }

        public string path { get; set; }

        public List<ArticleStructureTreeViewModel> childs { get; set; }
        public string logoUrl { get; set; }

        public ArticleStructureTreeViewModel()
        {
            childs = new List<ArticleStructureTreeViewModel>();
        }
    }
}
