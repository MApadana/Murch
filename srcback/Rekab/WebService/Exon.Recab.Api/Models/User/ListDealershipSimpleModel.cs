
namespace Exon.Recab.Api.Models.User
{
    public class ListDealershipSimpleModel
    {
        public long userId { get; set; }
        public long? categoryId { get; set; }
        public int pageIndex { get;  set; }
        public int pageSize { get;  set; }
        
    }
}