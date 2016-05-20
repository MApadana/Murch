using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.ConfigModel
{
   public class WebCFConfigViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public string elementTitle { get; set; }

        public string htmlName { get; set; }

        public string htmlId { get; set; }

        public string defaulteClass { get; set; }        

        public bool hasMultiSelectValue { get; set; }

        public string regex { get;  set; }

        public bool isRequired { get; set; }

        public bool hasValueSearch { get; set; }

        public List<long> children { get; set; }

        public List<long> hideList { get; set; }

        public List<CFDefaultValueViewModel> enableList { get;  set; }

        public List<CFDefaultValueViewModel> disableList { get;  set; }

        public string containerName { get;  set; }

        public string hideContainer { get; set; }

        public WebCFConfigViewModel()
        {
            children = new List<long>();
            hideList = new List<long>();
            enableList = new List<CFDefaultValueViewModel>();
            disableList = new List<CFDefaultValueViewModel>();
            containerName = "";
            hideContainer = "";

        }
    }
}
