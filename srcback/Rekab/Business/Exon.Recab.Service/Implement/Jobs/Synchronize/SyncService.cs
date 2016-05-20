using Exon.Recab.Domain.Constant.Prodoct;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Service.Implement.Advertise;
using Exon.Recab.Service.Implement.ReView;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Implement.Jobs.Synchronize
{
    public class SyncService
    {
        private SdbContext _sdb;

        private MdbContext _mdb;

        private AdvertiseService _AdvertiseService;

        private ReviewService _ReviewService;

        public SyncService()
        {
            _sdb = new SdbContext();
            _mdb = new MdbContext();
        }

        public void Run()
        {
            SyncProducts();

            Thread.Sleep(50000);

            SyncReviews();

            Thread.Sleep(50000);
        }

        private void SyncProducts()
        {
            List<Domain.Entity.Product> Products = _sdb.Product.Where(p => p.Status == ProdoctStatus.فعال).ToList();

            BsonArray productIds = new BsonArray();

            productIds.AddRange(Products.Select(p => p.Id.ToString()));

            _mdb.Products.DeleteMany(new BsonDocument { { "Id", new BsonDocument { { "$nin", productIds } } } });

            _AdvertiseService = new AdvertiseService(ref _sdb, ref _mdb);

            foreach (var item in Products)
            {
                _AdvertiseService.MongoProductUpdate(item);
            }
        }

        private void SyncReviews()
        {
            _ReviewService = new ReviewService(ref _sdb, ref _mdb);

            List<Review> Reviews = _sdb.Reviews.ToList();

            foreach (var item in Reviews)
            {
                _ReviewService.UpdateMongoReview(item);

            }

        }

        private void SyncArticle()
        {

        }

    }
}
