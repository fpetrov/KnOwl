using KnOwl.Models.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnOwl.Services.Article
{
    public interface IArticleService
    {
        Task<List<Entities.Storage.Article>> GetAll();
        Task<Entities.Storage.Article> Get(int id);
        Task<List<Entities.Storage.ShortedArticle>> Take(int? count);
        Task<Entities.Storage.Article> Add(ArticleRequest request);
        Task<Entities.Storage.Article> Update(ArticleRequest request);
        Task<Entities.Storage.Article> Remove(int id);
    }
}