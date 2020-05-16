﻿using Newtonsoft.Json.Linq;
using quizapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace quizapp.Controllers
{
    public class QuestionController
    {
        private HttpClient _requestClient;
        public QuestionController()
        {
            
        }
        public async Task<List<QuizQuestion>> GetQuizQuestions(string category, string difficulty, string quizLength)
        {
            var questionList = new List<QuizQuestion>();
            var questionJson = await GetQuestionJson(category, difficulty, quizLength);
            if (questionJson != null)
            {
                var questions = questionJson.GetValue("results") as JArray;
                foreach (var item in questions)
                {
                    var questionItem = item.ToObject<QuizQuestion>();
                    questionItem.Answers = BuildAnswerArray(questionItem.Correct_Answer, questionItem.Incorrect_Answers);
                    questionItem.Question = HttpUtility.HtmlDecode(questionItem.Question);
                    questionItem.Correct_Answer = HttpUtility.HtmlDecode(questionItem.Correct_Answer);
                    questionList.Add(questionItem);
                }
            }
            return questionList;
        }

        private string BuildRequestUrl(string category, string difficulty, string quizLength)
        {
            var questionUrl = $"https://opentdb.com/api.php?amount={quizLength}&type=multiple&category={category}&difficulty={difficulty.ToLowerInvariant()}";
            return questionUrl;
        }

        private async Task<JObject> GetQuestionJson(string category, string difficulty, string quizLength)
        {
            JObject response = null;
            var questionUrl = BuildRequestUrl(category, difficulty, quizLength);
            InitialiseRequestClient();
            try
            {
                var json = await _requestClient.GetStringAsync(questionUrl);
                response = JObject.Parse(json);
            }
            catch(HttpRequestException ex)
            {
                var error = ex.InnerException;
            }
            
            return response;
        }

        private List<string> BuildAnswerArray(string correctAnswer, List<string> incorrectAnswers)
        {
            var answerList = new List<string>();
            foreach(var item in incorrectAnswers)
            {
                var decodedAnswer = HttpUtility.HtmlDecode(item);
                answerList.Add(decodedAnswer);
            }
            var decodedCorrectAnswer = HttpUtility.HtmlDecode(correctAnswer);
            answerList.Add(decodedCorrectAnswer);
            var shuffledAnswers = answerList.OrderBy(a => Guid.NewGuid()).ToList();
            return shuffledAnswers;
        }

        private void InitialiseRequestClient()
        {
            _requestClient = new HttpClient();
        }
    }
}
