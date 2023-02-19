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

    public async Task GetNextQuestion(char answer)
    {
        CurrentQuestionSet ??= await _getQuestionsSet(_currentQuestionsSetName);
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
        }
        else if (answerText.StartsWith("Family") || answerText.StartsWith("Group"))
        {
            _currentQuestionsSetName = answerText;
            await _getQuestionsSet(answerText);
            CurrentQuestion = CurrentQuestionSet[0];
        }

        //TODO: add question history call here once decided how that's going to work
    }

    public Task HistoryForward()
    {
        //TODO
        throw new NotImplementedException();
    }

    public Task HistoryBackward()
    {
        //TODO
        throw new NotImplementedException();
    }

    public Task Reset()
    {
        //TODO
        throw new NotImplementedException();
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