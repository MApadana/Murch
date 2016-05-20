using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Domain.Constant.Media;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.MongoDb;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Service.Implement.Helper;
using Exon.Recab.Service.Model.ArticleModel;
using Exon.Recab.Service.Model.PublicModel;
using Exon.Recab.Service.Model.ReviewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace Exon.Recab.Service.Implement.ArTicl
{
    public class ArticleService
    {
        private SdbContext _sdb;
        private MdbContext _mdb;

        public ArticleService()
        {
            this._sdb = new SdbContext();
            this._mdb = new MdbContext();

        }

        #region add
        public bool AddNewArticle(long userId,
                                  long articleStructureId,
                                  string title,
                                  string body,
                                  string briefdes,
                                  string logoUrl,
                                  List<SelectItemModel> articleItem)

        {
            #region Validation
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            ArticleStructure articleStructure = _sdb.ArticleStructures.Find(articleStructureId);

            if (articleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);


            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == articleStructure.CategoryId && cf.AvailableInArticle).ToList();

            string ArtCFTitle = "";
            List<SelectItemModel> ConfirmItems = articleItem.GetConfirmedItems(entityCategoryFeature: InsertList,
                                                                               reqiredCategoryFeature: new List<CategoryFeature>(),
                                                                               titleItems : new List<CategoryFeature>(),
                                                                               title : ref ArtCFTitle);

            #endregion

            #region AddHeader

            Article newArticle = new Article
            {
                Title = title ?? "",
                Body = body ?? "",
                UserId = user.Id,
                ArticleStructureId = articleStructure.Id,
                BrifDescription = briefdes ?? "",
                Rate = 0,
                VisitCount = 0,
                CreateTime = DateTime.UtcNow,
                LogoUrl = logoUrl

            };

            _sdb.Articles.Add(newArticle);

            _sdb.SaveChanges();


            if (logoUrl != null && logoUrl != "")
            {
                _sdb.Media.Add(new Media
                {
                    EntityId = newArticle.Id,
                    MediaType = MediaType.Picture,
                    MediaURL = logoUrl,
                    EntityType = EntityType.Article
                });
                _sdb.SaveChanges();
            }

            #endregion

            #region FeatureValue

            foreach (var item in ConfirmItems)
            {
                FeatureValueAssign FeatureValueAssign;
                if (item.FeatureValueIds.Count > 0)
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Article,
                        CustomValue = "",
                        EntityId = newArticle.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                    foreach (var fvItem in item.FeatureValueIds)
                    {
                        FeatureValueAssign.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem { FeatureValueId = fvItem });
                    }

                }
                else
                {
                    FeatureValueAssign = new FeatureValueAssign
                    {
                        EntityType = EntityType.Article,
                        CustomValue = item.CustomValue,
                        EntityId = newArticle.Id,
                        CategoryFeatureId = item.CategoryFeatureId
                    };

                }

                _sdb.FeatureValueAssign.Add(FeatureValueAssign);
            }
            _sdb.SaveChanges();
            #endregion

            MongoArticleSave(newArticle);

            return true;
        }
        #endregion

        #region  edit
        public bool EditArticle(long articleId,
                                long articleStructureId,
                                string title,
                                string brief,
                                string logoUrl,
                                string body,
                                List<ArticleStructureFeature> articleItem)
        {
            Article Article = _sdb.Articles.Find(articleId);

            if (Article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);

            ArticleStructure articleStructure = _sdb.ArticleStructures.Find(articleStructureId);

            if (articleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);

            #region Validation

            int inputDistinctCount = articleItem.Select(pi => pi.CategoryFeatureId).Distinct().Count();


            if (inputDistinctCount != articleItem.Count)
                throw new RecabException((int)ExceptionType.SomeCategoryFeatureRedundant);


            List<CategoryFeature> InsertList = _sdb.CategoryFeatures.Where(cf => cf.CategoryId == articleStructure.CategoryId && cf.AvailableInArticle).ToList();


            //validate item  and concat to cf
            var inputItems = InsertList.Join(articleItem,
                                             i => i.Id,
                                             pi => pi.CategoryFeatureId,
                                             (i, pi) => new
                                             {
                                                 CategoryFeature = i,
                                                 ArticleSelectItem = pi
                                             }).ToList();


            // اگر تعداد آیتم های تایید شده با همه تعداد همه آیتم ها برابر نبود 
            if (inputItems.Count != inputDistinctCount)
                throw new RecabException((int)ExceptionType.InvalidInsertCategoryFeature);

            #endregion



            Article.Title = title ?? "";
            Article.Body = body ?? "";
            Article.ArticleStructureId = articleStructure.Id;
            Article.BrifDescription = brief ?? "";
            logoUrl = logoUrl ?? "";

            var articleCategoryFeature = _sdb.FeatureValueAssign.Where(fva => fva.EntityId == Article.Id && fva.EntityType == EntityType.Article).ToList();

            List<FeatureValueAssignFeatureValueItem> deleteItem = new List<FeatureValueAssignFeatureValueItem>();

            foreach (var item in articleCategoryFeature)
            {
                foreach (var itemc in item.ListFeatureValue)
                {
                    deleteItem.Add(itemc);
                }

            }

            _sdb.FeatureValueAssignFeatureValueItems.RemoveRange(deleteItem);
            _sdb.FeatureValueAssign.RemoveRange(articleCategoryFeature);

            foreach (var item in inputItems)
            {
                #region For fv Validation And cust
                if (!item.CategoryFeature.HasCustomValue)//no custom value
                {

                    if (!item.CategoryFeature.HasMultiSelectValue && item.ArticleSelectItem.FeatureValueId.Count > 1)  //if single select And has multi value
                        throw new RecabException((int)ExceptionType.CategoryFeatureModelStateErrorWrongMulti, item.CategoryFeature.Title);

                    else
                    {
                        if (!item.CategoryFeature.HasMultiSelectValue &&
                            item.ArticleSelectItem.FeatureValueId.Count == 1 &&
                            !item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == item.ArticleSelectItem.FeatureValueId.First())) // if single select and id is wrong
                            throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);

                        foreach (var temp in item.ArticleSelectItem.FeatureValueId)
                        {
                            if (!item.CategoryFeature.FeatureValueList.Any(fv => fv.Id == temp)) // if multi and data wrong
                                throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorWrongId, item.CategoryFeature.Title);
                        }

                    }
                }

                else  // custom value
                {
                    Regex regex = new Regex(item.CategoryFeature.Pattern, RegexOptions.IgnoreCase);

                    Match match = regex.Match(item.ArticleSelectItem.CustomValue ?? "");

                    if (!match.Success)
                        throw new RecabException((int)ExceptionType.FeatureValueModelStateErrorRegex, item.CategoryFeature.Title);

                }

                FeatureValueAssign newFeature = new FeatureValueAssign
                {
                    EntityId = Article.Id,
                    EntityType = EntityType.Article,
                    CustomValue = item.CategoryFeature.HasCustomValue ? item.ArticleSelectItem.CustomValue : "",
                    CategoryFeatureId = item.CategoryFeature.Id
                };

                if (item.CategoryFeature.HasMultiSelectValue)
                {
                    foreach (var kk in item.ArticleSelectItem.FeatureValueId)
                    {
                        newFeature.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem
                        {
                            FeatureValueId = item.CategoryFeature.FeatureValueList.Find(fv => fv.Id == kk).Id
                        });
                    }
                }

                else
                {
                    foreach (var valuItem in item.ArticleSelectItem.FeatureValueId)
                        newFeature.ListFeatureValue.Add(new FeatureValueAssignFeatureValueItem
                        {
                            FeatureValueId = item.CategoryFeature.FeatureValueList.Find(fv => fv.Id == valuItem).Id
                        });

                }

                #endregion

                if (!(newFeature.ListFeatureValue.Count == 0 && newFeature.CustomValue == ""))
                    _sdb.FeatureValueAssign.Add(newFeature);

            }


            _sdb.SaveChanges();


            return true;
        }

        public bool EditArticle(long articleId,
                               long articleStructureId,
                               string title,
                               string logoUrl,
                               string brief,
                               string body)
        {
            Article Article = _sdb.Articles.Find(articleId);

            if (Article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);

            ArticleStructure articleStructure = _sdb.ArticleStructures.Find(articleStructureId);

            if (articleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);

            Article.Title = title ?? "";
            Article.Body = body ?? "";
            Article.ArticleStructureId = articleStructure.Id;
            Article.BrifDescription = brief ?? "";
            Article.LogoUrl = logoUrl ?? "";

            _sdb.SaveChanges();

            return true;
        }

        public RateResultViewModel AddRate(long userId, long articleId, int rate)
        {
            if (rate > 5 || rate < 0)
                throw new RecabException((int)ExceptionType.BadRateData);

           Article article = _sdb.Articles.FirstOrDefault(r => r.Id == articleId);
            if (article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);


            var existRate = _mdb.Rates.Find(GetMongoRateFilter(userId: userId, articleId: articleId)).ToList();


            var AllRate = _mdb.Rates.Find(new BsonDocument { { "ArticleId", article.Id.ToString() } }).ToList();

            if (existRate.Count() > 0)
            { _mdb.Rates.DeleteMany(filter: Builders<BsonDocument>.Filter.Eq("_id", existRate.First()["_id"])); };


            if (existRate.Count() > 0)
            {
                double t1 = Convert.ToDouble(existRate.First()["Rate"].ToString());


                article.Rate = ((article.Rate * AllRate.Count()) + (rate - t1)) / ((AllRate.Count() == 0 ? 1 : (AllRate.Count())));
            }
            else
            {
                article.Rate = (double)((double)(article.Rate * AllRate.Count()) + rate) / ((double)(AllRate.Count() + 1));
            }
            _sdb.SaveChanges();

            _mdb.Rates.InsertOne(document: new BsonDocument { {"ArticleId", article.Id.ToString() },
                                                               {"UserId", userId.ToString() } ,
                                                               {"Rate", rate.ToString() } });

            return new RateResultViewModel { rate = (float)article.Rate };
        }

        #endregion

        #region Search
        public List<ArticleViewModel> GetALLArticle(long articleStructureId, ref long count, int size = 1, int skip = 0)
        {
            ArticleStructure articleStructure = _sdb.ArticleStructures.Find(articleStructureId);

            if (articleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);



            List<Article> Articles = _sdb.Articles.Where(a => a.ArticleStructureId == articleStructure.Id).ToList();

            count = Articles.Count;
            return Articles.Count == 0 ? new List<ArticleViewModel>() :
                Articles.OrderBy(r => r.Id).Skip(size * skip).Take(size)
                .Select(r => new ArticleViewModel
                {
                    articleId = r.Id,
                    title = r.Title,
                    briefDescription = r.BrifDescription,
                    rate = (float)r.Rate,
                    visitCount = r.VisitCount,
                    createTime = r.CreateTime.UTCToPersianDateLong(),
                    logoUrl = r.LogoUrl ?? "",
                }).ToList();

        }

        public ArticleViewModel GetSingleArticle(long articleId, long userId)
        {
            Article Article = _sdb.Articles.Find(articleId);

            if (Article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);

            Article.VisitCount = Article.VisitCount + 1;

            _sdb.SaveChanges();

            List<CategoryFeature> categoryFeature = _sdb.CategoryFeatures.Where(c => c.CategoryId == Article.ArticleStructure.CategoryId &&
                                                                                     c.AvailableInRVSearch &&
                                                                                     c.AvailableInArticle).ToList();

            var categoryFeatureTitle = categoryFeature.Where(cf => cf.AvailableInRVTitle)
                                                       .Join(_sdb.FeatureValueAssign.Where(fva => fva.EntityId == Article.Id && fva.EntityType == EntityType.Article),
                                                             cf => cf.Id,
                                                             fva => fva.CategoryFeatureId,
                                                             (cf, fva) => fva).ToList();

            List<ArticleStructureFeatureViewModel> model = new List<ArticleStructureFeatureViewModel>();

            List<long> featerValue = new List<long>();

            List<FeatureValueAssign> Articlecf = _sdb.FeatureValueAssign.Where(fva => fva.EntityId == Article.Id && fva.EntityType == EntityType.Article).ToList();

            foreach (var item in categoryFeature)
            {
                int missdParent = 0;

                foreach (var cfParent in item.ParentList)
                {
                    if (!model.Any(m => m.categoryFeatureId == cfParent.CategoryFeatureId))
                        missdParent++;
                }

                if (missdParent == 0)
                {
                    FeatureValueAssign tempFeatureValueAssign = Articlecf.FirstOrDefault(cf => cf.CategoryFeatureId == item.Id);

                    if (tempFeatureValueAssign != null)
                    {
                        model.Add(new ArticleStructureFeatureViewModel
                        {
                            categoryFeatureId = item.Id,
                            featureValueIds = tempFeatureValueAssign.ListFeatureValue.Select(fva => fva.FeatureValueId).ToList()
                        });

                        featerValue = featerValue.Concat(tempFeatureValueAssign.ListFeatureValue.Select(fva => fva.FeatureValueId).ToList()).ToList();

                    }

                    else
                    {

                        ArticleStructureFeatureViewModel newArticleStructureFeatureViewModel = new ArticleStructureFeatureViewModel
                        {
                            categoryFeatureId = item.Id,

                        };

                        List<long> newFeatureValue = new List<long>();
                        foreach (var fvParent in item.FeatureValueList)
                        {
                            int parent = 0;
                            foreach (var missdItem in fvParent.ParentList)
                            {
                                if (featerValue.Any(fv => fv == missdItem.FeatureValueId))
                                { parent++; }
                            }

                            if (parent == 1)
                            {
                                newFeatureValue.Add(fvParent.Id);
                            }
                        }

                        newArticleStructureFeatureViewModel.featureValueIds = newFeatureValue;

                        model.Add(newArticleStructureFeatureViewModel);
                    }
                }
            }


            foreach (var item in model)
            {
                if (item.featureValueIds.Count == 0)
                    model.Remove(item);
            }

            Double userRate = 0;

            if (userId != 0)
            {
                var existRate = _mdb.Rates.Find(GetMongoRateFilter(userId: userId, articleId: articleId)).ToList();
                if (existRate.Count() > 0)
                { userRate = Convert.ToDouble(existRate.First()["Rate"].ToString()); }

            }

            return new ArticleViewModel
            {
                title = Article.Title,
                htmlContent = Article.Body,
                rate = (float)Article.Rate,
                briefDescription = Article.BrifDescription ?? "",
                visitCount = Article.VisitCount,
                createTime = Article.CreateTime.UTCToPersianDateLong(),
                searchFilterItems = model,
                articleId = articleId,
                userRate = (float)userRate,
                logoUrl = Article.LogoUrl ?? "",
                parent = GetArticleStructureParent(Article.ArticleStructure)
                        .Select(AS => new ArticleStructureViewModel
                        {
                            title = AS.Title,
                            articleStructureId = AS.Id,
                            articleCount = this.ArticleStructureCount(AS)
                        }).ToList()

            };


        }

        public AdminArticleViewModel InitEditArticle(long articleId)
        {
            Article Article = _sdb.Articles.Find(articleId);

            if (Article == null)
                throw new RecabException((int)ExceptionType.ArticleNotFound);

            List<FeatureValueAssign> AssingeFeature = _sdb.FeatureValueAssign.Where(cf => cf.EntityId == Article.Id &&
                                                                                        cf.EntityType == EntityType.Article &&
                                                                                        cf.CategoryFeature.RequiredInRVInsert).ToList();

            return new AdminArticleViewModel
            {
                title = Article.Title,
                briefDescription = Article.BrifDescription ?? "",
                htmlContent = Article.Body,
                articleId = articleId,
                articleStructureId = Article.ArticleStructureId,
                categoryId = Article.ArticleStructure.CategoryId,
                categoryTitle = Article.ArticleStructure.Category.Title,
                logoUrl = Article.LogoUrl ?? "",
                categoryFeatures = AssingeFeature.Select(cf => new ArticleStructureFeatureEditViewModel
                {
                    title = cf.CategoryFeature.Title,
                    categoryFeatureId = cf.CategoryFeatureId,
                    featureValue = cf.ListFeatureValue.Select(fv => new ArticleFeatureValueEditViewModel
                    {
                        featureValueId = fv.FeatureValueId,
                        title = fv.FeatureValue.Title
                    }).ToList()
                }).ToList()
            };


        }

        public ResultSearchArticleViewModel ArticleSearch(long? articleStructureId,
                                                          string keyword,
                                                          long categoryId,
                                                          ref long count,
                                                          int size = 1,
                                                          int skip = 0)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            ArticleStructure articleStructure = new ArticleStructure();
            if (articleStructureId.HasValue)
            {
                articleStructure = _sdb.ArticleStructures.Find(articleStructureId);

                if (articleStructure == null || articleStructure.CategoryId != categoryId)
                    throw new RecabException((int)ExceptionType.ArticleStructureNotFound);
            }

            List<ArticleStructure> articleStructuresChileds = _sdb.ArticleStructures.Where(AS => AS.CategoryId == category.Id &&
                                                                                        (articleStructureId.HasValue ?
                                                                                         AS.ParentArticleStructureId == articleStructureId :
                                                                                         AS.ParentArticleStructureId.HasValue == false)).ToList();

            List<ArticleStructure> articleStructuresParent = GetArticleStructureParent(articleStructure);


            List<Article> articles = new List<Article>();

            if (articleStructureId.HasValue)
            {

                List<long> articlesId = ProductHelperService.TagItems((int)EntityType.Article, keyword: keyword, categoryId: category.Id, sdb: ref _sdb, mdb: ref _mdb);

                if (keyword.Length >= 2)
                {
                    articles = _sdb.Articles.Join(articlesId, a => a.Id, t => t, (a, t) => a).Where(a => a.ArticleStructureId == articleStructure.Id).ToList();
                }
                else
                {
                    articles = _sdb.Articles.Where(a => a.ArticleStructureId == articleStructure.Id).ToList();
                }

                count = articles.Count;
            }
            else
            {
                List<long> articlesId = ProductHelperService.TagItems((int)EntityType.Article, keyword: keyword, categoryId: category.Id, sdb: ref _sdb, mdb: ref _mdb);

                if (keyword.Length >= 2)
                {
                    articles = _sdb.Articles.Join(articlesId, a => a.Id, t => t, (a, t) => a).ToList();
                }                

                count = articles.Count;
            }


            return new ResultSearchArticleViewModel
            {
                articleStructureId = articleStructureId.HasValue ? articleStructureId.Value : 0,

                title = articleStructureId.HasValue ? articleStructure.Title : "",

                categoryId = categoryId,


                articles = count == 0 ? new List<ArticleSearchResultItemViewModel>() :
                                                    articles.Skip(skip * size).Take(size).Select(a => new ArticleSearchResultItemViewModel
                                                    {
                                                        articleId = a.Id,
                                                        brief = a.BrifDescription ?? "",
                                                        rate = (float)a.Rate,
                                                        title = a.Title ?? "",
                                                        visitCount = a.VisitCount,
                                                        date = a.CreateTime.UTCToPersianDateShort(),
                                                        logoUrl = a.LogoUrl ?? "",
                                                    }).ToList(),



                children = articleStructuresChileds.Count > 0 ?
                          articleStructuresChileds.Select(AS => new ArticleStructureViewModel
                          {
                              title = AS.Title,
                              articleStructureId = AS.Id,
                              logoUrl = AS.LogoUrl ?? "",
                              articleCount = this.ArticleStructureCount(AS)
                          }).ToList()
                          : new List<ArticleStructureViewModel>(),

                parents = articleStructuresParent.Count > 0 ?
                          articleStructuresParent.Select(AS => new ArticleStructureViewModel
                          {
                              title = AS.Title,
                              articleStructureId = AS.Id,
                              logoUrl = AS.LogoUrl ?? "",
                              articleCount = this.ArticleStructureCount(AS)
                          }).ToList()
                          : new List<ArticleStructureViewModel>()
            };


        }


        #endregion

        #region delete
        public bool RemoveArticle(long articleId)
        {
            Article Article = _sdb.Articles.Find(articleId);

            if (Article == null)
                throw new RecabException((int)ExceptionType.UserNotFound);




            List<FeatureValueAssign> FeatureValueAssinge = _sdb.FeatureValueAssign.Where(fva => fva.EntityId == articleId && fva.EntityType == EntityType.Article).ToList();

            foreach (var item in FeatureValueAssinge)
            {
                _sdb.FeatureValueAssignFeatureValueItems.RemoveRange(item.ListFeatureValue);

            }
            _sdb.FeatureValueAssign.RemoveRange(FeatureValueAssinge);
            _sdb.Articles.Remove(Article);


            _mdb.Articles.DeleteMany(filter: Builders<BsonDocument>.Filter.Eq("Id", Article.Id.ToString()));
            _sdb.SaveChanges();


            return true;

        }

        #endregion

        #region mongo
        private void MongoArticleSave(object obj)
        {

            Article Article = obj as Article;

            BsonDocument newMongoArticle = new BsonDocument {

                { "Id" , Article.Id.ToString() },
                { "UserId",Article.UserId.ToString() },
                { "ArticleStructureId", Article.ArticleStructure.Id.ToString() },
                { "Title" , Article.Title },
                { "BrifDescription" , Article.BrifDescription },
                { "Body" , Article.Body},
                { "Rate", Article.Rate },
                { "VisitCount", Article.VisitCount } ,
                { "CreateTime" , Article.CreateTime.ToLongTimeString()}
            };


            List<FeatureValueAssign> FeatureValueAssigns = _sdb.FeatureValueAssign.Where(fva => fva.EntityType == EntityType.Article &&
                                                                                                  fva.EntityId == Article.Id).ToList();


            foreach (FeatureValueAssign item in FeatureValueAssigns)
            {
                if (item.CategoryFeature.HasCustomValue)
                {
                    newMongoArticle.Add("CF_" + item.CategoryFeatureId.ToString(), "CUM_" + item.CustomValue ?? "");
                }
                else
                {
                    if (item.CategoryFeature.HasMultiSelectValue)
                    {
                        BsonArray bArray = new BsonArray();

                        var listId = item.ListFeatureValue.Select(fv => fv.FeatureValueId).ToList();
                        foreach (var fv in listId)
                        {
                            bArray.Add(fv);
                        }

                        newMongoArticle.Add("CF_" + item.CategoryFeatureId.ToString(), bArray);
                    }
                    else
                    {
                        newMongoArticle.Add("CF_" + item.CategoryFeatureId.ToString(), item.ListFeatureValue.FirstOrDefault() != null ? item.ListFeatureValue.FirstOrDefault().FeatureValueId.ToString() : "0");
                    }
                }
            }


            _mdb.Articles.InsertOne(newMongoArticle);

        }

        private BsonDocument GetMongoFilterSearch(long categoryId, List<ArticleFilterModelWithCategory> filter)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "CategoryId", categoryId.ToString() } });

            foreach (var item in filter)
            {
                mongoFilter.Add(new BsonDocument { { "CF_" + item.CategoryFeature.Id.ToString(), item.Filter.FeatureValueId.ToString() } });

            }

            return new BsonDocument { { "$and", mongoFilter } };

        }

        #endregion

        #region Private
        private List<ArticleStructure> GetArticleStructureParent(ArticleStructure articleStructure)
        {
            List<ArticleStructure> parent = new List<ArticleStructure>();

            if (articleStructure.ParentArticleStructureId.HasValue)
            {

                parent = parent.Concat(GetArticleStructureParent(articleStructure.ParentArticleStructure)).ToList();

                parent.Add(articleStructure.ParentArticleStructure);
            }

            return parent;

        }

        private long ArticleStructureCount(ArticleStructure ArticleStructure)
        {
            long count = ArticleStructure.Articles.Count();

            var subcategory = _sdb.ArticleStructures.Where(ac => ac.ParentArticleStructureId == ArticleStructure.Id).ToList();

            foreach (var item in subcategory)
            {
                count = count + ArticleStructureCount(item);
            }

            return count;
        }

        private BsonDocument GetMongoRateFilter(long userId, long articleId)
        {
            BsonArray mongoFilter = new BsonArray();

            mongoFilter.Add(new BsonDocument { { "UserId", userId.ToString() } });

            mongoFilter.Add(new BsonDocument { { "ArticleId", articleId.ToString() } });


            return new BsonDocument { { "$and", mongoFilter } };

        } 
        #endregion
    }
}
