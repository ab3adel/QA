using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using QA.Model;
using static Dapper.SqlMapper;
namespace QA.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly string _connectionString ;
        public DataRepository (IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public AnswerGetResponse GetAnswer(int answerId)
        {
            using (var connection = new SqlConnection(_connectionString))

            {

                connection.Open();
                return connection.QueryFirstOrDefault<AnswerGetResponse>(
                  @"EXEC dbo.Answer_Get_ByAnswerId @AnswerId =@AnswerId",
                  new { AnswerId = answerId }

                );

            }
        }

        public async Task< QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))

            {

               await connection.OpenAsync();
               using ( GridReader results= connection.QueryMultiple(@"
               EXEC dbo.Question_GetSingle @QuestionId=@QuestionId;
               EXEC dbo.Answer_Get_ByQuestionId @QuestionId=@QuestionId",
               new{QuestionId=questionId})) 
               {
                   var question= results.Read<QuestionGetSingleResponse>().FirstOrDefault();
                   if (question !=null)
                   {
                       question.Answers=results.Read<AnswerGetResponse>().ToList();
                   }
                   return question;
               }
            }
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(@"EXEC dbo.Question_GetMany");
            }
        }
         
         public IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers () 
         {
             using (var connection= new SqlConnection (_connectionString))
             {
                 connection.Open();
                 var questionDictionary= new Dictionary<int ,QuestionGetManyResponse>();
                 
         return  connection.Query<QuestionGetManyResponse,AnswerGetResponse,QuestionGetManyResponse>(
                     "EXEC dbo.Question_GetMany_WithAnswers",
                     map:(q , a)=> 
                     {
                       QuestionGetManyResponse question;
                       if (!questionDictionary.TryGetValue(q.QuestionId,out question))
                       {
                           question=q;
                           question.Answers=new List<AnswerGetResponse> ();
                           questionDictionary.Add(question.QuestionId,question);
                        
                       }
                       question.Answers.Add(a);
                       return question;
                     },splitOn:"QuestionId"
                 )
                 .Distinct()
                 .ToList();
                 
                 
             }
         }
        public IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(@"EXEC dbo.Question_GetMany_BySearch @Search =@Search",new { Search = search });
            }
        }
        public IEnumerable<QuestionGetManyResponse> GetQuestionBySearchWithPaging (string search,int pageSize ,int pageNumber)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
              connection.Open();
              var parameters = new {
                  Search =search,
                  PageNumber= pageNumber,
                  PageSize =pageSize
                  };
              return connection.Query<QuestionGetManyResponse>(
                             @"EXEC dbo.Question_GetMany_BySearch_WithPaging
                             @Search=@Search,
                             @PageNumber=@PageNumber,
                             @PageSize=@PageSize",parameters);
            }
        }
        public IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<QuestionGetManyResponse>(@"EXEC dbo.Question_GetUnanswered");
            }
        }
       public async Task<IEnumerable<QuestionGetManyResponse>>  GetUnansweredQuestionsAsync()
       {
           using (var connection =new SqlConnection(_connectionString))
           {
               await connection.OpenAsync();
               return await connection.QueryAsync<QuestionGetManyResponse>(@"EXEC dbo.Question_GetUnanswered");
           }
       }
        public bool QuestionExists(int questionId)
        {
            using (var connection = new
                   SqlConnection(_connectionString))
            {

                connection.Open();
                return connection.QueryFirst<bool>(
                                                  @"EXEC dbo.Question_Exists @QuestionId =@QuestionId"
                                                   ,new { QuestionId = questionId }

                );

            }
        }

        public async Task <QuestionGetSingleResponse> PostQuestion (QuestionPostRequest question)
        {
            using (var connection=new SqlConnection(_connectionString))
            {
                connection.Open();
                var questionId = connection.QueryFirst<int>( @"EXEC dbo.Question_Post
                                                             @Title = @Title, @Content = @Content,
                                                             @UserId = @UserId, @UserName = @UserName,
                                                             @Created = @Created",
                                                             question);
                return  await GetQuestion(questionId);
            }
        }

        public async Task< QuestionGetSingleResponse> PutQuestion (int questionId,QuestionPutRequest question)
        {
            using (var connection= new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute( 
                                    @"EXEC dbo.Question_Put
                                    @QuestionId = @QuestionId, @Title = @Title,
                                    @Content = @Content",
                                    new { QuestionId = questionId, question.Title,
                                           question.Content }
                                  );
                return await GetQuestion(questionId);        
            }

        }
        public void DeleteQuestion (int questionId)
        {
            using (var connection=new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(
                                    @"EXEC dbo.Question_Delete
                                    @QuestionId = @QuestionId",
                                    new { QuestionId = questionId }
                                   );
            }
        }
        public AnswerGetResponse PostAnswer(AnswerPostRequest answer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<AnswerGetResponse>(
                                                                @"EXEC dbo.Answer_Post
                                                                @QuestionId = @QuestionId, @Content = @Content,
                                                                @UserId = @UserId, @UserName = @UserName,
                                                                @Created = @Created", answer
                                                                );
            }
        }
    }
}
