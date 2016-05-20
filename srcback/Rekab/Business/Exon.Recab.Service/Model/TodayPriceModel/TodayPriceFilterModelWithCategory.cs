using Exon.Recab.Domain.Entity;


namespace Exon.Recab.Service.Model.TodayPriceModel
{
    public class TodayPriceFilterModelWithCategory
    {
        public TodayPriceFilterModel Filter { get; set; }

        public CategoryFeature CategoryFeature  { get; set; }
    }
}
