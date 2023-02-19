namespace key_to_mallorca_wasm.Services;
using key_to_mallorca_wasm.Models;
public class QuestionHistory
{
    private readonly List<QuestionHistoryEntry> _history = new();
    private int _currentIndex = -1;
    private int? _markedIndex = null;

    public void AddQuestion(Question question, string fileName)
    {
        if (_currentIndex < _history.Count - 1)
        {
            var countToRemove = _history.Count - 1 - _currentIndex;
            _history.RemoveRange(_currentIndex + 1, countToRemove);
        }

        var entry = new QuestionHistoryEntry(question, fileName);
        _history.Add(entry);
        _currentIndex = _history.Count - 1;
    }

    public QuestionHistoryEntry? GetPreviousQuestion()
    {
        if (_currentIndex <= 0) return null;
        _currentIndex--;
        return _history[_currentIndex];
    }

    public QuestionHistoryEntry? GetNextQuestion()
    {
        if (_currentIndex >= _history.Count - 1) return null;
        _currentIndex++;
        return _history[_currentIndex];
    }

    public void ClearHistory()
    {
        _history.Clear();
        _currentIndex = -1;
        _markedIndex = null;
    }
}

public class QuestionHistoryEntry
{
    public QuestionHistoryEntry(Question question, string questionSetName)
    {
        Question = question;
        QuestionSetName = questionSetName;
    }

    public Question Question { get; }
    public string QuestionSetName { get; }
}