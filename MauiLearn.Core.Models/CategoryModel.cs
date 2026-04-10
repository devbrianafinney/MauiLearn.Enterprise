using System.Text.Json.Serialization;


namespace MauiLearn.Core.Models
{
    public class CategoryModel
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }


    }
}