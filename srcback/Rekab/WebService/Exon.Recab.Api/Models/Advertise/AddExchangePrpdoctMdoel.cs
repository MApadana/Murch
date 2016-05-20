using Exon.Recab.Api.Infrastructure.Resource;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exon.Recab.Api.Models.Advertise
{
    public class AddExchangePrpdoctMdoel
    {
        
        public long? categoryId { get; set; }

        //rename2 :                           exchangeItems
        public List<productSelectItemViewModel> exchangeItems { get; set; }

        public AddExchangePrpdoctMdoel()
        {
            exchangeItems = new List<productSelectItemViewModel>();
        }
    }
}
