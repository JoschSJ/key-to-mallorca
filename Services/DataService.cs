// using Microsoft.Extensions.Configuration;
// using System.Threading.Tasks;
// using System.Net.Http;
using System.Net.Http.Json;
using key_to_mallorca_wasm.Models;
using Newtonsoft.Json.Linq;

namespace key_to_mallorca_wasm.Services;
public class DataService
{
    private readonly HttpClient _http;

    public DataService(HttpClient http)
    {
        _http = http;
    }

    public async Task<QuestionsList> LoadQuestionsListAsync(string fileName)
    {
        var filePath = $"data/{fileName}.json";
        return (await _http.GetFromJsonAsync<QuestionsList>(filePath))!;
    }

    public async Task<string> LoadWikipediaPageAsync(string pageName)
    {
        var response = await _http.GetAsync($"https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&titles={pageName}&explaintext=1");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var page = json["query"]["pages"].First.First;
            var extract = page.Value<string>("extract");
            return extract;
        }

        throw new HttpRequestException($"Failed to load page: {pageName}. Status code: {response.StatusCode}");
    }
}