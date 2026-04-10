using System.Text.Json.Serialization;

namespace MauiLearn.Core.Models
{
    public class ProjectModels
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        [JsonIgnore]
        public int CategoryID { get; set; }

        public CategoryModel? Category { get; set; }

        public List<ProjectTask> Tasks { get; set; } = [];

        //public List<Tag> Tags { get; set; } = [];
        //public List<T> Tags { get; set; } = [];

        public string AccessibilityDescription
        {
            get { return $"{Name} Project. {Description}"; }
        }

        public override string ToString() => $"{Name}";
    }

    public class ProjectsJson
    {
        public List<ProjectModels> Projects { get; set; } = [];
    }
}