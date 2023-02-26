using System.Text.Json.Serialization;

namespace key_to_mallorca_wasm.Models;

public class Question
{
    [JsonPropertyName("-id")] public string? Id { get; set; }
    [JsonPropertyName("questionA")] public string? QuestionA { get; set; }
    [JsonPropertyName("questionB")] public string? QuestionB { get; set; }
    [JsonPropertyName("answerA")] public string? AnswerA { get; set; }
    [JsonPropertyName("answerB")] public string? AnswerB { get; set; }
}