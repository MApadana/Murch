using MongoDB.Bson.Serialization.Attributes;

namespace Exon.Recab.Service.Model.ProdoctModel
{
    public class FeatuerValueAggModel
    {
        public string _id { get; set; }

        public int count { get; set; }
    }

    public class FeatuerValueAggModelLong
    {
        public long _id { get; set; }

        public int count { get; set; }
    }
}
