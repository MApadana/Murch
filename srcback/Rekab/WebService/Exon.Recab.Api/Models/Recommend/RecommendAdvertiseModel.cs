namespace Exon.Recab.Api.Models.Recommend
{
    public class RecommendAdvertiseModel
    {
        public long categoryId { get; set; }

        public long entityId { get; set; }

        public EntityType type { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }

        

    }

    public enum EntityType
    {
        آگهی = 1,
        بررسی_فنی = 2,
        مقاله = 3,
        قیمت_روز = 4
    }
}
