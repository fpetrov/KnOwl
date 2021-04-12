using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KnOwl.Entities.Storage
{
    [Owned]
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}