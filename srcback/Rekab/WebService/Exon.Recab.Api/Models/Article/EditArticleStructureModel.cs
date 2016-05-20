using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Article
{
    public class EditArticleStructureModel
    {
        public long articleStructureId { get; set; }
        public long categoryId { get; set; }
        public string logoUrl { get;  set; }
        public long? parentArticleStructureId { get; set; }
        public string title { get; set; }
    }
}
