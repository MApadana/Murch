using Exon.Recab.Service.Model.ProdoctModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class DealershipDetailViewModel
    {

        public List<SearchResultItemViewModel> products { get; set; }
        public string tell { get; set; }
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public string description { get; set; }
        public string fax { get; set; }
        public string website { get; set; }
        public string logoUrl { get; set; }
        public List<string> categories { get; set; }
        public string title { get; set; }
        public long id { get; set; }

        public DealershipDetailViewModel()
        {
            products = new List<SearchResultItemViewModel>();
        }
    }
}
