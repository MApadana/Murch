using System.Collections.Generic;

namespace Exon.Recab.Service.Model.ConfigModel
{
    public class AndroidCFConfigViewModel
    {
        public long categoryFeatureId { get; set; }

        public string title { get; set; }

        public string androidType { get; set; }

        //rename : Children
        public List<long> children { get; set; }

        public bool required { get; set; }

        public bool hasMultiSelectValue { get; set; }

        public string regex { get;  set; }

        public string containerName { get;  set; }

        public string hideContainer { get;  set; }

        public List<long> hideList { get; set; }

        public List<CFDefaultValueViewModel> enableList { get; set; }

        public List<CFDefaultValueViewModel> disableList { get;  set; }

        public AndroidCFConfigViewModel()
        {
            children = new List<long>();

            hideList = new List<long>();

            enableList = new List<CFDefaultValueViewModel>();

            disableList = new List<CFDefaultValueViewModel>();
        }

    }
}
