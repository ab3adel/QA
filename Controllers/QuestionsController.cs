using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QA.Model;
using QA.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
namespace QA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class QuestionsController :ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IQuestionCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _auth0UserInfo;
   
        public QuestionsController(IDataRepository dataRepository,IQuestionCache cache,
                                   IHttpClientFactory httpClientFactory,IConfiguration configuration )
        {
            _dataRepository=dataRepository;
            _cache=cache;
            _auth0UserInfo=$"{configuration["Auth0:Authority"]}userinfo";
        }
        [HttpGet]
        public   IEnumerable<QuestionGetManyResponse> GetQuestions(string search , 
                                                                 bool includeAnswers,
                                                                 int pageNumber=1,
                                                                 int pageSize=20)
        {
            if (String.IsNullOrEmpty(search))
            {
                if (includeAnswers)
                {
                    return _dataRepository.GetQuestionsWithAnswers();
                }
                
                return  _dataRepository.GetQuestions();
            }
           else {
               return _dataRepository.GetQuestionBySearchWithPaging(search,
                                                                    pageNumber,pageSize);
           }
        }
        [HttpGet("unanswered")]
        public async Task<IEnumerable<QuestionGetManyResponse>> GetUnansweredQuestions()
        {
            return await _dataRepository.GetUnansweredQuestionsAsync();
        }
        [HttpGet("{questionId}")]
        public async Task < ActionResult<QuestionGetSingleResponse>> GetQuestion (int questionId)
        {
            var question = _cache.Get(questionId);
            if (question == null)
            {
                  question= await _dataRepository.GetQuestion(questionId);
                  
                 if (question == null)
                 {
                     return NotFound();
                 }
                 _cache.Set(question);
            }
           
            return question;
        }
        
        [HttpPost]
 
        public async Task< ActionResult<QuestionGetSingleResponse>> PostQuestion(QuestionPostRequest questionpostRequest)
        {
            var savedQuestion= await _dataRepository.PostQuestion(questionpostRequest);
            return CreatedAtAction(nameof(GetQuestion),new{questionId=savedQuestion.QuestionId},savedQuestion);
        }
        [HttpPut("{questionId}")]
        [Authorize(Policy ="MustBeQuestionAuthor")]
        public async Task< ActionResult<QuestionGetSingleResponse>> PutQuestion(int questionId,QuestionPutRequest questionRequest)
        {
            var question =await _dataRepository.GetQuestion(questionId);
            questionRequest.Title= String.IsNullOrEmpty(questionRequest.Title) ?
                                   question.Title:
                                   questionRequest.Title;
            questionRequest.Content= String.IsNullOrEmpty(questionRequest.Content) ?
                                   question.Content:
                                   questionRequest.Content;                       

           var savedQuestion= await _dataRepository.PutQuestion(questionId,questionRequest);
           _cache.Remove(savedQuestion.QuestionId);
           return savedQuestion;

        }
        [HttpDelete("{questionId}")]
        [Authorize(Policy ="MustBeQuestionAuthor")]

        public ActionResult DeleteQuestion(int questionId)

       {

         var question =_dataRepository.GetQuestion(questionId);
          if (question == null)
          {
             return NotFound();

          }

         _dataRepository.DeleteQuestion(questionId);
         _cache.Remove(questionId);
         return NoContent();
       }
       [HttpPost("answer")]
       public ActionResult<AnswerGetResponse> PostAnswer (AnswerPostRequest answer)
       {
        var questionExist= _dataRepository.QuestionExists(answer.QuestionId.Value);
        if (!questionExist)
        {
            return NotFound();
        }
        var savedAnswer=_dataRepository.PostAnswer(answer);
        _cache.Remove(answer.QuestionId.Value);
        return savedAnswer;
       }
       private async Task<string> GetUserName () 
       {
           var request =new HttpRequestMessage(HttpMethod.Get,_auth0UserInfo);
           request.Headers.Add("Authorization",Request.Headers["Authorization"].First());
           var client= _httpClientFactory.CreateClient();
           var response = await client.SendAsync(request);
           if (response.IsSuccessStatusCode)
           {
               var jsonContent = await response.Content.ReadAsStringAsync();
               var user = JsonSerializer.Deserialize<User>( jsonContent,new JsonSerializerOptions

            {

              PropertyNameCaseInsensitive = true

            });

             return user.Name;
           }
           else {
                  return "";
           }
           
       }

    }
}