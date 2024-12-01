using Newtonsoft.Json;

namespace Sellasist_Optima.SellAsistModels
{
    public class AtrybutyGrupa
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("attributes")]
        public List<AtrybutyGrupy> Attributes { get; set; }
    }

    public class AtrybutyGrupy
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("options")]
        public List<string> Options { get; set; }

        [JsonProperty("required")]
        public bool Required { get; set; }

        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }
    }
}
