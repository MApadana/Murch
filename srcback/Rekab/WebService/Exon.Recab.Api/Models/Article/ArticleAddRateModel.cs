using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exon.Recab.Api.Models.Article
{
    public class ArticleAddRateModel
    {
        public long articleId { get; set; }
        public int rate { get;set; }
        public long userId { get; set; }
    }
}