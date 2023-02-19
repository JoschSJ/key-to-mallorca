namespace key_to_mallorca_wasm.Models;

public interface IKey
{
    bool IsQuestionLoaded { get; set; }
    string QuestionA { get; }
    string QuestionB { get; }

    Task GetNextQuestion(char answer);
    Task HistoryBackward();
    Task HistoryForward();
    Task ResetToStart();
    // Task MarkQuestion();
    // Task GetMarkedQuestion();
}