using System.Net.Http.Json;
using key_to_mallorca_wasm.Models;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

namespace key_to_mallorca_wasm.Services;
public class DataService
{
    private readonly HttpClient _http;
    // private readonly IJSRuntime JSRuntime { get; set; }

    public DataService(HttpClient http)
    {
        _http = http;
    }


}