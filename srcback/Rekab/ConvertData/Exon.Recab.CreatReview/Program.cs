using Exon.Recab.Domain.Entity;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Model.ReviewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.CreatReview
{
    class Program
    {
        static void Main(string[] args)
        {
            Recab.Domain.SqlServer.SdbContext _sdb = new Domain.SqlServer.SdbContext();

            Recab.Service.Implement.ReView.ReviewService _ReviewService = new Service.Implement.ReView.ReviewService();
            int cont = 0;
            try
            {

                List<FeatureValue> Brands = _sdb.FeatureValues.Where(fv => fv.CategoryFeatureId == 51).ToList();

                foreach (var BrandItem in Brands)
                {

                    List<FeatureValue> Models = _sdb.FeatureValues.Where(fv => fv.CategoryFeatureId == 52 && fv.ParentList.Any(p => p.FeatureValueId == BrandItem.Id)).ToList();

                    foreach (var ModelItem in Models)
                    {

                        //List<FeatureValue> Salls = _sdb.FeatureValues.Where(fv => fv.CategoryFeatureId == 73 && fv.ParentList.Any(p => p.FeatureValueId == ModelItem.Id)).ToList();


                        //foreach (var Sallitem in Salls)
                        //{

                            //List<FeatureValue> Dandehs = _sdb.FeatureValues.Where(fv => fv.CategoryFeatureId == 11 && fv.ParentList.Any(p => p.FeatureValueId == Sallitem.Id)).ToList();

                            //foreach (var dandehitem in Dandehs)
                            //{

                            //    List<FeatureValue> hajms = _sdb.FeatureValues.Where(fv => fv.CategoryFeatureId == 49 && fv.ParentList.Any(p => p.FeatureValueId == dandehitem.Id)).ToList();

                            //    foreach (var hajmItem in hajms)
                            //    {

                                    List<SelectItemModel> temps = new List<SelectItemModel>();

                                    List<long> brandFeatureValue = new List<long>();

                                    brandFeatureValue.Add(BrandItem.Id);

                                    temps.Add(new SelectItemModel { CategoryFeatureId = BrandItem.CategoryFeatureId, FeatureValueIds = brandFeatureValue });

                                    List<long> modelFeatureValue = new List<long>();

                                    modelFeatureValue.Add(ModelItem.Id);

                                    temps.Add(new SelectItemModel { CategoryFeatureId = ModelItem.CategoryFeatureId, FeatureValueIds = modelFeatureValue });


                                    //List<long> sallFeatureValue = new List<long>();

                                    //sallFeatureValue.Add(Sallitem.Id);

                                    //temps.Add(new SelectItemModel { CategoryFeatureId = Sallitem.CategoryFeatureId, FeatureValueIds = sallFeatureValue });

                                    //List<long> dandehFeatureValue = new List<long>();

                                    //dandehFeatureValue.Add(dandehitem.Id);

                                    //temps.Add(new SelectItemModel { CategoryFeatureId = dandehitem.CategoryFeatureId, FeatureValueIds = dandehFeatureValue });

                                    //List<long> hajmFeatureValue = new List<long>();

                                    //hajmFeatureValue.Add(hajmItem.Id);

                                    //temps.Add(new SelectItemModel { CategoryFeatureId = hajmItem.CategoryFeatureId, FeatureValueIds = hajmFeatureValue });


                                    _ReviewService.AddNewReview(userId: 2,
                                                                 categoryId:2,
                                                                 body: " ",
                                                                 reviewItem: temps,
                                                                 media: new List<Service.Model.ProdoctModel.MediaModel>());
                        //        }
                        //    }
                        //}

                    }

                }
            }
            catch (Exception e)
            {
                throw e;
            }



        }
    }
}
