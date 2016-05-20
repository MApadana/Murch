using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;

namespace Exon.Recab.Domain.MongoDb
{
    public class MdbContext
    {
        private readonly IMongoClient _client ;

        private readonly IMongoDatabase _database ;

        public readonly IMongoCollection<BsonDocument> Products;

        public readonly IMongoCollection<BsonDocument> ExchangeProdocts;

        public readonly IMongoCollection<BsonDocument> Reviews;

        public readonly IMongoCollection<BsonDocument> Articles;

        public readonly IMongoCollection<BsonDocument> Rates;

        public readonly IMongoCollection<BsonDocument> TodayPrice;

        public readonly IMongoCollection<BsonDocument> TodayPriceHistory;

        public readonly IMongoCollection<BsonDocument> UserAlertVisit;

        public readonly IMongoCollection<BsonDocument> UserProductVisit;

        public readonly IMongoCollection<BsonDocument> Tag;

        public readonly IMongoCollection<BsonDocument> UserNews;

        public readonly IMongoCollection<BsonDocument> News;

        public readonly IMongoCollection<BsonDocument> Email;

        public readonly IMongoDatabase db;

        public MdbContext()
        {
            AppSettingsReader settingsReader = new AppSettingsReader();
            string ConnectionString = settingsReader.GetValue("MongoConnectionString", typeof(string)) as string;

            _client = new MongoClient(ConnectionString);

            _database = _client.GetDatabase("kala");

            Products = _database.GetCollection<BsonDocument>("Products");

            ExchangeProdocts = _database.GetCollection<BsonDocument>("ExchangeProdocts");

            Reviews = _database.GetCollection<BsonDocument>("Reviews");

            Articles = _database.GetCollection<BsonDocument>("Articles");

            Rates = _database.GetCollection<BsonDocument>("Rates");

            TodayPrice = _database.GetCollection<BsonDocument>("TodayPrices");

            TodayPriceHistory = _database.GetCollection<BsonDocument>("TodayPriceHistory");

            UserAlertVisit = _database.GetCollection<BsonDocument>("UserAlertVisit");

            UserProductVisit = _database.GetCollection<BsonDocument>("UserProductVisit");

            Tag = _database.GetCollection<BsonDocument>("Tag");

            UserNews = _database.GetCollection<BsonDocument>("UserNews");

            News = _database.GetCollection<BsonDocument>("News");

            Email = _database.GetCollection<BsonDocument>("Email");

            db = _database;

            #region Config

            //db.Products.createIndex( { Location: "2dsphere" } )
            //db.Tag.createIndex( { Title: "text" } )
            #endregion
        }

    }
}
