using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Role
{
    public class AdminUserSearchModel
    {
        public long? roleId { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }
}
