using key_to_mallorca_wasm.Models;
using Microsoft.AspNetCore.Components;

namespace key_to_mallorca_wasm.Controllers;



public class KeyController: IKey
{
    public KeyController(IQuestionDataService questionDataService, NavigationManager navigationManager, string currentQuestionsSetName = "Key-to-Families", List<Question>? currentQuestionSet = null, Question? currentQuestion = null)
    {
        QuestionDataService = questionDataService;
        NavigationManager = navigationManager;
        CurrentQuestionsSetName = currentQuestionsSetName;
        CurrentQuestionSet = currentQuestionSet;
        CurrentQuestion = currentQuestion;
    }

    public string QuestionA => CurrentQuestion?.QuestionA ?? "Something went wrong loading question";

    public string QuestionB => CurrentQuestion?.QuestionB ?? "Something went wrong loading question";
    public bool IsQuestionLoaded => CurrentQuestion != null;

    private string CurrentQuestionsSetName { get; set; }
    // private readonly QuestionHistory _questionHistory = new();
    // private readonly DataService _dataService;
    private Question? CurrentQuestion { get; set; }
    private List<Question>? CurrentQuestionSet { get; set; }

    private readonly QuestionHistory _questionHistory = new();
    [Inject] private IQuestionDataService QuestionDataService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    public async Task GetNextQuestion(char answer)
    {
        CurrentQuestionSet ??= await _getQuestionsSet(CurrentQuestionsSetName);
        if (answer == default && CurrentQuestion == null)
        {
            CurrentQuestion ??= CurrentQuestionSet[0];
            _questionHistory.AddQuestion(CurrentQuestion, CurrentQuestionsSetName);
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
            CurrentQuestion = CurrentQuestionSet[id - 1];
            _questionHistory.AddQuestion(CurrentQuestion, CurrentQuestionsSetName);
        }
        else if (answerText.StartsWith("Family") || answerText.StartsWith("Group"))
        {
            CurrentQuestionsSetName = answerText;
            await _getQuestionsSet(answerText);
            CurrentQuestion = CurrentQuestionSet[0];
            _questionHistory.AddQuestion(CurrentQuestion, CurrentQuestionsSetName);
        }
        else
        {
            NavigationManager.NavigateTo($"species/{answerText.Replace(" ", "_").ToLower()}");
        }


    }

    private async Task RestoreQuestionFromQuestionHistory(QuestionHistoryEntry? questionEntry)
    {
        if (questionEntry == null) return;
        if (questionEntry.QuestionSetName != CurrentQuestionsSetName)
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
        CurrentQuestionSet = (await QuestionDataService.GetQuestionSet(questionSetName));
        return CurrentQuestionSet;
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