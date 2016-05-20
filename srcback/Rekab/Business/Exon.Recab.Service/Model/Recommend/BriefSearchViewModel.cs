using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exon.Recab.Service.Constant;

namespace Exon.Recab.Service.Model.Recommend
{
    public class BriefSearchViewModel
    {
        public BriefSearchType type { get; set; }

        public string title { get; set; }

        public List<BriefSearchItemViewModel> items { get; set; }


        public BriefSearchViewModel()
        {
            items = new List<BriefSearchItemViewModel>();
        }
    }
}
