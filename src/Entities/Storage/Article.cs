using KnOwl.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KnOwl.Entities.Storage
{
    [Owned]
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ArticleType Type { get; set; }
        public DateTime Created { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Image> Images { get; set; }
    }

    public enum ArticleType
    {
        Private,
        Shared,
        Moderating
    }
}