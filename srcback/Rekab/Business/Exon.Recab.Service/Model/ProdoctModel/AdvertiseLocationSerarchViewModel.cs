using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ProdoctModel
{
  public  class AdvertiseLocationSerarchViewModel
    {
        public SearchResultItemViewModel advertise { get; set; }

        public double lat { get; set; }

        public double lng { get; set; }

        public double distance { get; set; }

        public AdvertiseLocationSerarchViewModel()
        {
            advertise = new SearchResultItemViewModel();
        }

    }
}
