using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ArticleModel
{
   public class ResultSearchArticleViewModel
    {
        public List<ArticleStructureViewModel> parents { get; set; }

        public List<ArticleStructureViewModel> children { get; set; }


        public List<ArticleSearchResultItemViewModel> articles { get; set; }

        public string title { get; set; }

        public long articleStructureId { get; set; }

        public long categoryId { get; set; }
    }
}
