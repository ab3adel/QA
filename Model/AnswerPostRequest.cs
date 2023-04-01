using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace QA.Model
{
    public class AnswerPostRequest 
    {
        [Required]
        public int? QuestionId {get; set;}
        public string Content {get; set;}
        public int UserId {get; set;}
        public string UserName{get; set;}
        public DateTime Created {get; set;}
    }
}