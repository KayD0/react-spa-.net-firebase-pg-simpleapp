using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProdBase.Web.Models
{
    [Table("user_profiles")]
    public class UserProfile
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        [JsonPropertyName("firebase_uid")]
        public string FirebaseUID { get; set; }

        [StringLength(100)]
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [Column(TypeName = "text")]
        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [StringLength(100)]
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [StringLength(255)]
        [JsonPropertyName("website")]
        public string Website { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public UserProfile()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Dictionary<string, object> ToMap()
        {
            return new Dictionary<string, object>
            {
                { "id", Id },
                { "firebase_uid", FirebaseUID },
                { "display_name", DisplayName ?? "" },
                { "bio", Bio ?? "" },
                { "location", Location ?? "" },
                { "website", Website ?? "" },
                { "created_at", CreatedAt.ToString("o") },
                { "updated_at", UpdatedAt.ToString("o") }
            };
        }
    }
}
