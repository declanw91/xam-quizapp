using Newtonsoft.Json.Linq;
using quizapp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace quizapp.Controllers
{
    public class QuestionController
    {
        private HttpClient _requestClient;
        public QuestionController()
        {
            _requestClient = new HttpClient();
        }
        public async Task<List<QuizQuestion>> GetQuizQuestions(string category, string difficulty)
        {
            var questionList = new List<QuizQuestion>();
            var questionJson = await GetQuestionJson(category, difficulty);
            if (questionJson != null)
            {
                var questions = questionJson.GetValue("results") as JArray;
                foreach (var item in questions)
                {
                    questionList.Add(item.ToObject<QuizQuestion>());
                }
            }
            return questionList;
        }

        private string BuildRequestUrl(string category, string difficulty)
        {
            var questionUrl = $"https://opentdb.com/api.php?amount=10&type=multiple&category={category}&difficulty={difficulty.ToLowerInvariant()}";
            return questionUrl;
        }

        private async Task<JObject> GetQuestionJson(string category, string difficulty)
        {
            JObject response = null;
            var questionUrl = BuildRequestUrl(category, difficulty);
            using (_requestClient)
            {
                var json = await _requestClient.GetStringAsync(questionUrl);
                response = JObject.Parse(json);
            }
            return response;
        }
    }
}
