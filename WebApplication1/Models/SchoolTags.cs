using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class SchoolTags
    {
        [JsonPropertyName("amenity")]
        public string Amenity { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        //public string NameLt { get; set; }
        //public string NameMk { get; set; }
        //public string NameEn { get; set; }
    }
}
