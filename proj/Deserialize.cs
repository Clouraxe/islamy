namespace Typed
{
    using System.Text.Json.Serialization;

    public class HadithRoot
    {
        [JsonPropertyName("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonPropertyName("hadiths")]
        public List<Hadith>? Hadiths { get; set; }
    }
    
    public class _2
    {
        [JsonPropertyName("hadithnumber_first")]
        public int HadithnumberFirst { get; set; }

        [JsonPropertyName("hadithnumber_last")]
        public int HadithnumberLast { get; set; }

        [JsonPropertyName("arabicnumber_first")]
        public int ArabicnumberFirst { get; set; }

        [JsonPropertyName("arabicnumber_last")]
        public int ArabicnumberLast { get; set; }
    }

    public class Grade
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("grade")]
        public string? Value { get; set; }
    }

    public class Hadith
    {
        [JsonPropertyName("hadithnumber")]
        public int Hadithnumber { get; set; }

        [JsonIgnore]
        [JsonPropertyName("arabicnumber")]
        public int Arabicnumber { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }

        [JsonPropertyName("grades")]
        public List<Grade>? Grades { get; set; }

        [JsonPropertyName("reference")]
        public Reference? Reference { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("section")]
        public Section? Section { get; set; }

        [JsonPropertyName("section_detail")]
        public SectionDetail? SectionDetail { get; set; }
    }

    public class Reference
    {
        [JsonPropertyName("book")]
        public int Book { get; set; }

        [JsonPropertyName("hadith")]
        public int Hadith { get; set; }
    }


    public class Section
    {
        [JsonPropertyName("2")]
        public string? _2 { get; set; }
    }

    public class SectionDetail
    {
        [JsonPropertyName("2")]
        public _2? _2 { get; set; }
    }


}




