namespace Exon.Recab.Api.Models.Advertise
{
    public class FaveFindModel
    {
        public long userId { get; set; }

        public long categoryId { get; set; }

        public int pageSize { get; set; }

        public int pageIndex { get; set; }
    }
}
