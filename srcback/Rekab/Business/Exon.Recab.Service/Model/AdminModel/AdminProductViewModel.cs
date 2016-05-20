using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Model.AdminModel
{
    public class AdminProductViewModel
    {
        public long advertiseId { get;  set; }

        public string categoryTitle { get;  set; }

        public string confirmDate { get;  set; }

        public long cumUserId { get; set; }

        public string userName { get; set; }

        public string mobile { get; set; }

        public long? dealershipId { get;  set; }

        public string description { get;  set; }

        public long? exchangeCategoryId { get;  set; }

        public string imageUrl { get; set; }

        public string insertDate { get;  set; }
        
        public long packageId { get;  set; }

        public string packageTitle { get;  set; }

        public int status { get;  set; }

        public string title { get;  set; }

        public bool updateAble { get;  set; }

        public int updateCount { get;  set; }
        public long categoryId { get;  set; }
    }
}
