using System;
using System.Text.Json.Serialization;

namespace ProdBase.Application.DTOs
{
    public class ProfileDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firebase_uid")]
        public string FirebaseUID { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }
    }

    public class ProfileUpdateRequest
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
        
        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("website")]
        public string Website { get; set; }
    }
}
