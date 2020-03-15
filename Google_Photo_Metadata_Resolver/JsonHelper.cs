using Newtonsoft.Json;

namespace Google_Photo_Metadata_Resolver
{
    class JsonHelper
    {}

    public class Metadata
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageViews")]
        public long ImageViews { get; set; }

        [JsonProperty("creationTime")]
        public NTime CreationTime { get; set; }

        [JsonProperty("modificationTime")]
        public NTime ModificationTime { get; set; }

        [JsonProperty("geoData")]
        public GeoData GeoData { get; set; }

        [JsonProperty("geoDataExif")]
        public GeoData GeoDataExif { get; set; }

        [JsonProperty("photoTakenTime")]
        public NTime PhotoTakenTime { get; set; }
    }

    public class NTime
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }

    public class GeoData
    {
        [JsonProperty("latitude")]
        public long Latitude { get; set; }

        [JsonProperty("longitude")]
        public long Longitude { get; set; }

        [JsonProperty("altitude")]
        public long Altitude { get; set; }

        [JsonProperty("latitudeSpan")]
        public long LatitudeSpan { get; set; }

        [JsonProperty("longitudeSpan")]
        public long LongitudeSpan { get; set; }
    }
}

