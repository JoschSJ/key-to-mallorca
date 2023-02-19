// using key_to_mallorca_wasm.Models;
//
// namespace key_to_mallorca_wasm.Services;
//
// public class QuestionService
// {
//     private List<Question>? CurrentQuestionSet { get; set; }
//     public Question? CurrentQuestion { get; set; }
//
//     private string _currentQuestionsSetName = "Key-to-Families";
//     private readonly QuestionHistory _questionHistory = new();
//     private readonly DataService _dataService;
//
//     public QuestionService(DataService dataService)
//     {
//         _dataService = dataService;
//         // _currentQuestionsSet = GetQuestionsSet(_currentQuestionsSetName);
//         // CurrentQuestion = LoadNextQuestionAsync();
//     }
//
//     public async Task LoadNextQuestionAsync(string answer = "1")
//     {
//         CurrentQuestionSet ??= await GetQuestionsSet(_currentQuestionsSetName);
//
//         var isNum = int.TryParse(answer, out var id);
//         if (isNum)
//         {
//             // offset for json answer offset 🙄
//             id--;
//             CurrentQuestion = CurrentQuestionSet[id];
//         }
//         else
//         {
//             _currentQuestionsSetName = answer;
//             await GetQuestionsSet(answer);
//             CurrentQuestion = CurrentQuestionSet[0];
//         }
//
//         _questionHistory.AddQuestion( CurrentQuestion, _currentQuestionsSetName);
//     }
//
//     public async Task GetPreviousQuestion()
//     {
//         var previousQuestion = _questionHistory.GetPreviousQuestion();
//         if (previousQuestion == null) return;
//
//         if (previousQuestion.QuestionSetName != _currentQuestionsSetName)
//             await GetQuestionsSet(previousQuestion.QuestionSetName);
//         CurrentQuestion = previousQuestion.Question;
//     }
//
//     public async Task GetNextQuestionFromHistory()
//     {
//         var nextQuestion = _questionHistory.GetNextQuestion();
//         if (nextQuestion == null) return;
//
//         if (nextQuestion.QuestionSetName != _currentQuestionsSetName)
//             await GetQuestionsSet(nextQuestion.QuestionSetName);
//         CurrentQuestion = nextQuestion.Question;
//     }
//
//     private async Task<List<Question>> GetQuestionsSet(string fileName)
//     {
//         CurrentQuestionSet = (await _dataService.LoadQuestionsListAsync(fileName)).Questions;
//         return CurrentQuestionSet;
//     }
//
//     public async Task Reset()
//     {
//         _currentQuestionsSetName = "key-to-Families";
//         await GetQuestionsSet(_currentQuestionsSetName);
//         await LoadNextQuestionAsync();
//     }
// }