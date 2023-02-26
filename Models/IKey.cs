namespace key_to_mallorca_wasm.Models;

public interface IKey
{
    bool IsQuestionLoaded { get; }
    string QuestionA { get; }
    string QuestionB { get; }

    Task GetNextQuestion(char answer = default);
    Task HistoryBackward();
    Task HistoryForward();
    Task ResetToStart();
    // Task MarkQuestion();
    // Task GetMarkedQuestion();
}