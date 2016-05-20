
namespace Exon.Recab.Api.Models.User
{
    public class DealershipFindModel
    {
        public long categoryId { get;  set; }

        public long? cityId { get;  set; }

        public double distance { get;  set; }

        public double? lat { get;  set; }

        public double? lng { get;  set; }

        public long? stateId { get;  set; }

        public string title { get;  set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }
}
