using System.Text.Json.Serialization;


namespace FakeUser.Infrastructure.Dtos
{
    public class FakeUserDto
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

    }
}
