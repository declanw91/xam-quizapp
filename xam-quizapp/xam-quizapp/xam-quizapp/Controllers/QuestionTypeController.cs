using System.Collections.Generic;

namespace quizapp.Controllers
{
    public class QuestionTypeController
    {
        public QuestionTypeController()
        {

        }

        public List<string> GetQuestionTypeOptions()
        {
            var questionTypeList = new List<string>();
            questionTypeList.Add("Multiple Choice");
            questionTypeList.Add("True or False");
            return questionTypeList;
        }
    }
}
