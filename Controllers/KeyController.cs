using System.Net.Http.Json;
using key_to_mallorca_wasm.Models;

namespace key_to_mallorca_wasm.Controllers;



public class KeyController: IKey
{
    public KeyController(HttpClient http)
    {
        _http = http;
    }

    public bool IsQuestionLoaded { get; set; }
    public string QuestionA => CurrentQuestion?.QuestionA ?? "Something went wrong loading question";

    public string QuestionB => CurrentQuestion?.QuestionB ?? "Something went wrong loading question";

    private string _currentQuestionsSetName = "Key-to-Families";
    // private readonly QuestionHistory _questionHistory = new();
    // private readonly DataService _dataService;
    private Question? CurrentQuestion { get; set; }
    private List<Question>? CurrentQuestionSet { get; set; }

    private readonly HttpClient _http;
    private readonly QuestionHistory _questionHistory = new();

    public async Task GetNextQuestion(char answer = '-')
    {
        CurrentQuestionSet ??= await _getQuestionsSet(_currentQuestionsSetName);
        CurrentQuestion ??= CurrentQuestionSet[0];
        if (answer == '-')
        {
            _questionHistory.AddQuestion(CurrentQuestion, _currentQuestionsSetName);
            return;
        }

        var answerText = answer == 'a' ?
            CurrentQuestion?.AnswerA : CurrentQuestion?.AnswerB;

        //todo sort out this to be less bad :)
        if (answerText == null) throw new Exception("CurrentQuestion.Answer returned null");

        var answerIsNum = int.TryParse((answerText), out var id);
        if (answerIsNum)
        {
            // offset for json answer offset 🙄
            id--;
            CurrentQuestion = CurrentQuestionSet[id];
            _questionHistory.AddQuestion(CurrentQuestion, _currentQuestionsSetName);
        }
        else if (answerText.StartsWith("Family") || answerText.StartsWith("Group"))
        {
            _currentQuestionsSetName = answerText;
            await _getQuestionsSet(answerText);
            CurrentQuestion = CurrentQuestionSet[0];
            _questionHistory.AddQuestion(CurrentQuestion, _currentQuestionsSetName);
        }


        Console.WriteLine(answerText);
        //TODO: add question history call here once decided how that's going to work
    }

    private async Task RestoreQuestionFromQuestionHistory(QuestionHistoryEntry? questionEntry)
    {
        if (questionEntry == null) return;
        if (questionEntry.QuestionSetName != _currentQuestionsSetName)
        {
            await _getQuestionsSet(questionEntry.QuestionSetName);
        }

        CurrentQuestion = questionEntry.Question;
    }
    public async Task HistoryBackward()
    {
        await RestoreQuestionFromQuestionHistory(_questionHistory.GetPreviousQuestion());
    }

    public async Task HistoryForward()
    {
        await RestoreQuestionFromQuestionHistory(_questionHistory.GetNextQuestion());
    }


    public async Task ResetToStart()
    {
        await RestoreQuestionFromQuestionHistory(_questionHistory.ClearHistory());
    }

    private async Task<List<Question>> _getQuestionsSet(string questionSetName)
    {
        CurrentQuestionSet = (await LoadQuestionsListAsync(questionSetName)).Questions;
        IsQuestionLoaded = true;
        return CurrentQuestionSet;
        // return new List<Question>();
    }

    private async Task<QuestionsList> LoadQuestionsListAsync(string fileName)
    {
        var filePath = $"data/{fileName}.json";
        return (await _http.GetFromJsonAsync<QuestionsList>(filePath))!;
    }
}

internal class QuestionHistory
{
    private readonly List<QuestionHistoryEntry> _history = new();
    private int _currentIndex = -1;
    // private int? _markedIndex = null;

    public void AddQuestion(Question question, string fileName)
    {
        // Handle rewriting history
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

    public QuestionHistoryEntry? ClearHistory()
    {
        if (_currentIndex == -1 || _history.Count <= 1) return null;

        _history.RemoveRange(1, _history.Count - 1);
        _currentIndex = 0;
        // _markedIndex = null;
        return _history[0];
    }
}

internal class QuestionHistoryEntry
{
    public QuestionHistoryEntry(Question question, string questionSetName)
    {
        Question = question;
        QuestionSetName = questionSetName;
    }

    public Question Question { get; }
    public string QuestionSetName { get; }
}