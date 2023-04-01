using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace QA.Model
{
    public class QuestionPostRequest
    {
        [Required]
        public string Title { get; set; }
        [Required(ErrorMessage ="please you must type some content")]
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
