using System.Collections.Generic;


namespace Exon.Recab.Service.Model.ConfigModel
{
    public class BaseValuesModel
    {      
        public long featureValueId { get; set; }

        public string title { get; set; }

        public List<long> hideList { get; set; }

        public List<long> showList { get; set; }

        public List<CFDefaultValueViewModel> enableList { get; set; }

        public List<CFDefaultValueViewModel> disableList { get; set; }

        public string showContainer { get;set; }

        public string hideContainer { get; set; }

        public BaseValuesModel()
        {
            enableList = new List<CFDefaultValueViewModel>();
            disableList = new List<CFDefaultValueViewModel>();

            hideList = new List<long>();
            showList = new List<long>();
            showContainer = "";
            hideContainer = "";
            title = "";

        }
    }
}
