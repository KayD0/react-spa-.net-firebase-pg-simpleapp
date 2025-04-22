using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProdBase.Domain.Entities
{
    [Table("user_profiles")]
    public class UserProfile
    {
        [Key]
        [Column("id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required]
        [Column("firebase_uid")]
        [StringLength(128)]
        [JsonPropertyName("firebase_uid")]
        public string FirebaseUID { get; set; }

        [StringLength(100)]
        [Column("display_name")]
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [Column("bio", TypeName = "text")]
        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [StringLength(100)]
        [Column("location")]
        [JsonPropertyName("location")]
        public string Location { get; set; }

        [StringLength(255)]
        [Column("website")]
        [JsonPropertyName("website")]
        public string Website { get; set; }

        [Column("created_at")]
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
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
