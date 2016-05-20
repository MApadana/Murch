using Exon.Recab.Service.Implement.ToDayPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Infrastructure.Utility.Extension;
using System.Net.Http;
using System.Web.Http;
using Exon.Recab.Api.Models.TodayPrice;
using Exon.Recab.Service.Model.TodayPriceModel;

namespace Exon.Recab.Api.Controllers
{
    public class TodayPriceController : ApiController
    {
        #region init
        private readonly TodayPriceService _TodayPriceService;

        public TodayPriceController()
        {
            _TodayPriceService = new TodayPriceService();
        } 
        #endregion

        #region     report

        [HttpPost]
        public HttpResponseMessage TodayPriceSearchGroupBy(TodayPriceSearchModel model)
        {
            long count = 0;

            List<TodayPriceFilterModel> filter = new List<TodayPriceFilterModel>();

            foreach (var item in model.selectedItems)
            {
                filter.Add(new TodayPriceFilterModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueId = (item.featureValueIds.Count > 0 ? item.featureValueIds.FirstOrDefault() : new long()) });

            }

            return _TodayPriceService.TodayPriceSearchGroupBy(categoryId: model.categoryId,
                                                        keyword : model.keyword,
                                                       count: ref count,
                                                       size: model.pageSize,
                                                       skip: model.pageIndex,
                                                       filter: filter).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceCategoryGroupBy(TodayPriceSearchModel model)
        {
            return _TodayPriceService.TodayPriceCategoryGroupBy(categoryId: model.categoryId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceSearch(TodayPriceSearchModel model)
        {
            long count = 0;

            List<TodayPriceFilterModel> filter = new List<TodayPriceFilterModel>();

            foreach (var item in model.selectedItems)
            {
                filter.Add(new TodayPriceFilterModel { CategoryFeatureId = item.categoryFeatureId, FeatureValueId = (item.featureValueIds.Count > 0 ? item.featureValueIds.FirstOrDefault() : new long()) });

            }

            return _TodayPriceService.TodayPriceSearch(categoryId: model.categoryId,
                                                       keyword : model.keyword,
                                                       count: ref count,
                                                       size: model.pageSize,
                                                       skip: model.pageIndex,
                                                       filter: filter).GetHttpResponseWithCount(count);
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceHistorySearch(TodayPriceFindModel model)
        {
            return _TodayPriceService.GetAllTodayPriceHistory(model.todayPriceId).GetHttpResponse();
        }

        [HttpPost]
        public HttpResponseMessage TodayPriceHistoryDetail(TodayPriceFindModel model)
        {
            return _TodayPriceService.GetChartDataForTodayPrice(model.todayPriceId).GetHttpResponse();
        }

        #endregion

    }
}
