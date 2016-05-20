using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.User
{
    public class UserAdminFindModel
    {
        public long categoryId { get; set; }

        public long cumUserId { get; set; }

        public bool isDealership { get; set; }

        public int pageSize { get; set; }

        public int skipPage { get; set; }
    }
}
