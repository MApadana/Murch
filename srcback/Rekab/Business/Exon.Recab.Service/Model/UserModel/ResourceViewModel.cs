using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.UserModel
{
    public class ResourceViewModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public int type { get; set; }
        public long? parentId { get; set; }
        public string key { get; set; }
        public List<ResourceViewModel> childs { get; set; }
    }
}
