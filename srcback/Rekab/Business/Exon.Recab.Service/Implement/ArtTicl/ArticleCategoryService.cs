using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.Entity.CMS;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using Exon.Recab.Service.Model.ArticleModel;
using Exon.Recab.Domain.Constant.CS.Exception;

namespace Exon.Recab.Service.Implement.ArtTicl
{
    public class ArticleStructureService
    {
        public readonly SdbContext _sdb;

        public ArticleStructureService()
        {
            _sdb = new SdbContext();
        }

        #region ADD
        public bool AddArticleStructure(string title, long categoryId,string logoUrl ,long? parentId)
        {
            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            if (parentId.HasValue)
            {
                ArticleStructure ArticleStructure = _sdb.ArticleStructures.Find(parentId);

                if (ArticleStructure == null || category.Id != ArticleStructure.CategoryId)
                    throw new RecabException((int)ExceptionType.ArticleStructureNotFound);
            }


            _sdb.ArticleStructures.Add(new ArticleStructure
            {
                Title = title ?? "",
                CategoryId = categoryId,
                ParentArticleStructureId = parentId,
                LogoUrl = logoUrl ?? "" 
                
            });

            _sdb.SaveChanges();

            return true;

        }

        #endregion

        #region search
        public List<ArticleStructureSimpleViewModel> SearchArticleStructureByParent(long categoryId, ref long count, long? parentId, int size = 1, int skip = 0)
        {
            Category Category = _sdb.Categoris.Find(categoryId);

            if (Category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            List<ArticleStructure> ArticleStructure = new List<ArticleStructure>();

            if (parentId.HasValue)
            {
                ArticleStructure parent = _sdb.ArticleStructures.Find(parentId);

                if (parent == null)
                    throw new RecabException((int)ExceptionType.ArticleStructureNotFound);

                ArticleStructure = _sdb.ArticleStructures.Where(ac => ac.ParentArticleStructureId == parentId && categoryId == Category.Id).ToList();

                ArticleStructure.Add(parent);
            }
            else
            {

                ArticleStructure = _sdb.ArticleStructures.Where(ac => ac.ParentArticleStructureId == parentId && categoryId == Category.Id).ToList();
            }

            List<ArticleStructureSimpleViewModel> model = new List<ArticleStructureSimpleViewModel>();

            count = ArticleStructure.Count;

            if (count == 0)
                return model;

            ArticleStructure = ArticleStructure.OrderBy(ac => ac.Id).Skip(size * skip).Take(size).ToList();

            foreach (var item in ArticleStructure)
            {
                model.Add(new ArticleStructureSimpleViewModel
                {
                    articleStructureId = item.Id,
                    articleStructureTitle = item.Title,
                    articleCount = this.ArticleStructureCount(item),
                    path = ArticleStructurePath(item) ,
                    logoUrl=item.LogoUrl ?? ""
                });

            }

            return model;
        }

        public List<ArticleStructureSimpleViewModel> GetAllArticleStructure(long categoryId, ref long count, int size = 1, int skip = 0)
        {
            Category Category = _sdb.Categoris.Find(categoryId);

            if (Category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            count = _sdb.ArticleStructures.Count();

            return this.GetAllArticleStructure(category: Category, ArticleStructure: new ArticleStructure())
                       .OrderBy(m => m.articleStructureId)
                       .Skip(skip * size)
                       .Take(size).ToList();
        }

        public ArticleStructureTreeViewModel GetTreeArticleStructure(long categoryId, ref long count, int size = 1, int skip = 0)
        {
            Category Category = _sdb.Categoris.Find(categoryId);

            if (Category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            count = _sdb.ArticleStructures.Count();

            return this.GetTreeArticleStructure(category: Category, ArticleStructure: new ArticleStructure());                       
        }

        public List<ArticleStructureSimpleViewModel> GetAllArticleStructureForParentEdit(long categoryId, long ArticleStructureId, ref long count, int size = 1, int skip = 0)
        {
            Category Category = _sdb.Categoris.Find(categoryId);

            if (Category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            ArticleStructure ArticleStructure = _sdb.ArticleStructures.Find(ArticleStructureId);

            if (ArticleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);


            count = _sdb.ArticleStructures.Count();

            return this.GetAllArticleStructureForParentEdit(category: Category,
                                                           ArticleStructure: new ArticleStructure(),
                                                           filterArticleStructure: ArticleStructure)
                       .OrderBy(m => m.articleStructureId)
                       .Skip(skip * size)
                       .Take(size).ToList();
        }

        public EditArticleStructureViewModel GetSingleArticleStructure(long articleStructureId)
        {
            ArticleStructure ArticleStructure = _sdb.ArticleStructures.Find(articleStructureId);

            if (ArticleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);

            return new EditArticleStructureViewModel
            {
                articleStructureId = ArticleStructure.Id,
                categoryId = ArticleStructure.CategoryId,
                parentArticleStructureId = ArticleStructure.ParentArticleStructureId,
                title = ArticleStructure.Title ?? "" ,
                logoUrl = ArticleStructure.LogoUrl ??""
            };

        }

        #endregion

        #region edit

        public bool EditArticleStructure(long ArticleStructureId, string title,string logoUrl ,long categoryId, long? parentId)
        {
            ArticleStructure ArticleStructure = _sdb.ArticleStructures.Find(ArticleStructureId);

            if (ArticleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);


            Category category = _sdb.Categoris.Find(categoryId);

            if (category == null)
                throw new RecabException((int)ExceptionType.CategoryNotFound);

            if (parentId.HasValue)
            {
                ArticleStructure ArticleStructureParent = _sdb.ArticleStructures.Find(parentId);

                if (ArticleStructureParent == null || category.Id != ArticleStructureParent.CategoryId)
                    throw new RecabException((int)ExceptionType.ArticleStructureNotFound);
            }


            ArticleStructure.Title = title ?? "";

            ArticleStructure.CategoryId = categoryId;

            ArticleStructure.ParentArticleStructureId = parentId;

            ArticleStructure.LogoUrl = logoUrl ?? "";

            _sdb.SaveChanges();

            return true;

        }

        #endregion

        #region delete
        public bool DeleteArticleStructure(long ArticleStructureId)
        {
            ArticleStructure ArticleStructure = _sdb.ArticleStructures.Find(ArticleStructureId);

            if (ArticleStructure == null)
                throw new RecabException((int)ExceptionType.ArticleStructureNotFound);

            if (_sdb.ArticleStructures.Any(ac => ac.ParentArticleStructureId == ArticleStructure.Id))
                throw new RecabException((int)ExceptionType.ArticleStructureHasChild);

            if (this.ArticleStructureCount(ArticleStructure) > 0)
                throw new RecabException((int)ExceptionType.ArticleStructureHasArticle);

            _sdb.ArticleStructures.Remove(ArticleStructure);
            _sdb.SaveChanges();
            return true;


        }
        #endregion

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

        private string ArticleStructurePath(ArticleStructure ArticleStructure)
        {
            string model = ArticleStructure.Title;

            while (ArticleStructure.ParentArticleStructureId.HasValue)
            {
                ArticleStructure = ArticleStructure.ParentArticleStructure;
                model = model + "<" + ArticleStructure.Title;

            }
            return model;
        }

        private List<ArticleStructureSimpleViewModel> GetAllArticleStructure(Category category, ArticleStructure ArticleStructure)
        {
            List<ArticleStructureSimpleViewModel> model = new List<ArticleStructureSimpleViewModel>();

            if (ArticleStructure.CategoryId == 0)
            {
                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id && !ac.ParentArticleStructureId.HasValue).ToList();

                foreach (var item in collection)
                {
                    model = model.Concat(this.GetAllArticleStructure(category: category, ArticleStructure: item)).ToList();
                }
            }

            else
            {
                model.Add(new ArticleStructureSimpleViewModel
                {
                    articleStructureId = ArticleStructure.Id,
                    articleStructureTitle = ArticleStructure.Title,
                    articleCount = this.ArticleStructureCount(ArticleStructure),
                    path = ArticleStructurePath(ArticleStructure) ,
                    logoUrl = ArticleStructure.LogoUrl ?? "" 
                });

                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id && ac.ParentArticleStructureId == ArticleStructure.Id).ToList();

                foreach (var item in collection)
                {
                    model = model.Concat(this.GetAllArticleStructure(category: category, ArticleStructure: item)).ToList();
                }

            }

            return model;
        }

        private ArticleStructureTreeViewModel GetTreeArticleStructure(Category category, ArticleStructure ArticleStructure)
        {
            ArticleStructureTreeViewModel model = new ArticleStructureTreeViewModel();

            if (ArticleStructure.CategoryId == 0)
            {
                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id && !ac.ParentArticleStructureId.HasValue).ToList();

                foreach (var item in collection)
                {
                    model.childs.Add(this.GetTreeArticleStructure(category: category, ArticleStructure: item));
                }
            }

            else
            {
                model = new ArticleStructureTreeViewModel
                {
                    articleStructureId = ArticleStructure.Id,
                    articleStructureTitle = ArticleStructure.Title,
                    articleCount = this.ArticleStructureCount(ArticleStructure),
                    path = ArticleStructurePath(ArticleStructure),
                    logoUrl = ArticleStructure.LogoUrl
                };


                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id && ac.ParentArticleStructureId == ArticleStructure.Id).ToList();

                foreach (var item in collection)
                {
                    model.childs.Add(this.GetTreeArticleStructure(category: category, ArticleStructure: item));
                }

            }

            return model;
        }

        private List<ArticleStructureSimpleViewModel> GetAllArticleStructureForParentEdit(Category category,
                                                                                        ArticleStructure ArticleStructure,
                                                                                        ArticleStructure filterArticleStructure)
        {
            List<ArticleStructureSimpleViewModel> model = new List<ArticleStructureSimpleViewModel>();

            if (ArticleStructure.CategoryId == 0)
            {
                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id &&
                                                                                    !ac.ParentArticleStructureId.HasValue &&
                                                                                     ac.Id != filterArticleStructure.Id).ToList();

                foreach (var item in collection)
                {
                    model = model.Concat(this.GetAllArticleStructureForParentEdit(category: category,
                                                                                 ArticleStructure: item,
                                                                                 filterArticleStructure: filterArticleStructure)).ToList();
                }
            }

            else
            {
                model.Add(new ArticleStructureSimpleViewModel
                {
                    articleStructureId = ArticleStructure.Id,
                    articleStructureTitle = ArticleStructure.Title,
                    articleCount = this.ArticleStructureCount(ArticleStructure),
                    path = ArticleStructurePath(ArticleStructure)
                });

                List<ArticleStructure> collection = _sdb.ArticleStructures.Where(ac => ac.CategoryId == category.Id &&
                                                                                     ac.ParentArticleStructureId == ArticleStructure.Id &&
                                                                                     ac.Id != filterArticleStructure.Id).ToList();

                foreach (var item in collection)
                {
                    model = model.Concat(this.GetAllArticleStructureForParentEdit(category: category,
                                                                                ArticleStructure: item,
                                                                                filterArticleStructure: filterArticleStructure)).ToList();
                }

            }

            return model;
        }
    }
}
