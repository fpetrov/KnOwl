using System.ComponentModel.DataAnnotations;

namespace KnOwl.Entities.Storage
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}