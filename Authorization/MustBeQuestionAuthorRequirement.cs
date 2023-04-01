
using Microsoft.AspNetCore.Authorization;
namespace QA.Authorization 
{
    public class MustBeQuestionAuthorRequirement :IAuthorizationRequirement
    {
       public MustBeQuestionAuthorRequirement ()
       {
           
       }
    }
}