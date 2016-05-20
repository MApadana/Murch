using System.Collections.Generic;


namespace Exon.Recab.Service.Model.PackageModel
{
    public class PackageInitConfigItemViewModel
    {
        public List<string> BaseValue { get; set; }

        public string configTitle { get; set; }

        public string configType { get; set; }

        public string configValue { get; set; }

        public PackageInitConfigItemViewModel()
        {
            BaseValue = new List<string>();
        }
    }
}
