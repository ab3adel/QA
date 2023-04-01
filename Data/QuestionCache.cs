using Microsoft.Extensions.Caching.Memory;
using QA.Model;
using QA.Data;

namespace QA.Data 
{
    public class QuestionCache :IQuestionCache
    {

        private MemoryCache _Cache {get; set;}
        public QuestionCache () 
      {
          _Cache=new MemoryCache (new MemoryCacheOptions {
              SizeLimit=100
          });
      }
      private string GetCacheKey (int questionId)=> $"Question-{questionId}";
      public QuestionGetSingleResponse Get (int quesitonId)
      {
          QuestionGetSingleResponse question;
        _Cache.TryGetValue(GetCacheKey(quesitonId),out question);
        return question;
      
      }
      public void Set (QuestionGetSingleResponse question )
      {
          var cacheEntry =new MemoryCacheEntryOptions ().SetSize(1);
          _Cache.Set(GetCacheKey(question.QuestionId),question,cacheEntry);
    }
    public void Remove (int questionId) 
    {
        _Cache.Remove(GetCacheKey(questionId));
    }
}
}