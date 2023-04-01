
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using QA.Data;
namespace QA.Authorization 
{
   public class MustBeQuestionAuthorHandler :AuthorizationHandler<MustBeQuestionAuthorRequirement>
   {
     private readonly IDataRepository _dataRepository;
     private readonly IHttpContextAccessor _httpContextAccessor;
     public MustBeQuestionAuthorHandler (IDataRepository dataRepository,
                                              IHttpContextAccessor httpContextAccessor)
     {
         _dataRepository=dataRepository;
         _httpContextAccessor=httpContextAccessor;
     }  
     protected async  override Task HandleRequirementAsync (AuthorizationHandlerContext context
                                                             ,MustBeQuestionAuthorRequirement requirement)                                                        
     {
         if (!context.User.Identity.IsAuthenticated) 
         {
             context.Fail();
             return;
         }
         var quesionId=_httpContextAccessor.HttpContext.Request.RouteValues["questionId"];
         int questionIdAsInt =Convert.ToInt32(quesionId);
         var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
         var question= await _dataRepository.GetQuestion(questionIdAsInt);
         if (question ==null)
          {
              context.Succeed(requirement);
              return;
          }
          if (question.UserId != userId)
          {
           context.Fail();
           return;
          }
          context.Succeed(requirement);
     }                                       
   }
}