using System.Net.Http.Json;
using key_to_mallorca_wasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

namespace key_to_mallorca_wasm.Services;
public class SpeciesService
{
    [Inject] private HttpClient Http { get; }
    public SpeciesService(HttpClient http)
    {
        Http = http;
    }

    public async Task<SpeciesList> GetSpeciesList()
    {
        var json = await Http.GetFromJsonAsync<SpeciesList>("data/Specieslist.json");
        if (json == null)
            throw new NullReferenceException("There was an error loading species list");
        return json;
    }


}