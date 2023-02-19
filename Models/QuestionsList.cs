using System.Text.Json.Serialization;
// using Newtonsoft.Json;

namespace key_to_mallorca_wasm.Models;

public class QuestionsList
{
    [JsonPropertyName("question")] public List<Question> Questions { get; set; } = new();
}