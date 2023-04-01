using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QA.Model;
namespace QA.Data
{
    public interface IDataRepository
    {
         IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers ();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        IEnumerable <QuestionGetManyResponse> GetQuestionBySearchWithPaging(string search,int pageSize,int pageNumber);
        IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions();
         Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestionsAsync ();
       Task< QuestionGetSingleResponse> GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        Task<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest question);

       Task< QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest  question);

        void DeleteQuestion(int questionId);

        AnswerGetResponse PostAnswer(AnswerPostRequest answer);
    }
}
