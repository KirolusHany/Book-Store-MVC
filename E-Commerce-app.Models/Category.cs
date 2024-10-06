using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce_app.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required,DisplayName("Category Name") ,MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order "),Range(1,100)]
        public int DisplayOrder { get; set; }
        [JsonIgnore]
        public IEnumerable<Product>? Products { get; set; }  
    }
}
