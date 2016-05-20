using Exon.Recab.Api.Models.Advertise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Api.Models.Review
{
    public class EditReviewModel
    {
        public long userId { get; set; }
  
        public string body { get; set; }
         
        public long reviewId { get;  set; }

        public List<AddAdvertiseMediaModel> media { get; set; }

        public EditReviewModel()
        {
            media = new List<AddAdvertiseMediaModel>();     
        }
    }
}
