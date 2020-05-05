using Newtonsoft.Json.Linq;
using quizapp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace quizapp.Controllers
{
    public class CategoryController
    {
        private static string _categoryUrl;
        private HttpClient _requestClient;
        public CategoryController()
        {
            _categoryUrl = "https://opentdb.com/api_category.php";
            _requestClient = new HttpClient();
        }
        public async Task<List<QuizCategory>> GetQuizCategories()
        {
            var catList = new List<QuizCategory>();
            var categoryJson = await GetCategoryJson();
            if(categoryJson != null)
            {
                var categories = categoryJson.GetValue("trivia_categories") as JArray;
                foreach(var item in categories)
                {
                    catList.Add(item.ToObject<QuizCategory>());
                }
            }
            return catList;
        }

        private async Task<JObject> GetCategoryJson()
        {
            JObject response = null;
            try
            {
                using (_requestClient)
                {
                    var json = await _requestClient.GetStringAsync(_categoryUrl);
                    response = JObject.Parse(json);
                }
            }
            catch(HttpRequestException ex)
            {
                // unable to process request
            }
            
            return response;
        }
    }
}
