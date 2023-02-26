using System.Net.Http.Json;
using key_to_mallorca_wasm.Models;
using Microsoft.AspNetCore.Components;

namespace key_to_mallorca_wasm.Services;

public class OnlineQuestionDataService: IQuestionDataService
{
    public OnlineQuestionDataService(HttpClient http)
    {
        Http = http;
    }
    [Inject] private HttpClient Http { get; }

    public async Task<List<Question>> GetQuestionSet(string questionSetName)
    {
        var filePath = $"data/{questionSetName}.json";
        var json = await Http.GetFromJsonAsync<QuestionsList>(filePath);
        if (json == null)
        {
            throw new NullReferenceException($"There was an error loading question set {questionSetName}");
        }
        return json.Questions;
    }
}