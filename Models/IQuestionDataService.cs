namespace key_to_mallorca_wasm.Models;

public interface IQuestionDataService
{
    Task<List<Question>> GetQuestionSet(string questionSetName);
}