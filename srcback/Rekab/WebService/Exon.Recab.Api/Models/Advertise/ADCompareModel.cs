using System.Collections.Generic;


namespace Exon.Recab.Api.Models.Advertise
{
   public class ADCompareModel
    {
        public List<long> advertiseIds { get; set; }
    
        public long categoryId { get; set; }

        public ADCompareModel()
        {
            advertiseIds = new List<long>();
        }
    }
}
