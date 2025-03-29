using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/quiz")]
    public class ApiQuizAdminController : ControllerBase
    {
        private readonly IQuizAdminService _service;
        private readonly LinkGenerator _linkGenerator;

        public ApiQuizAdminController(IQuizAdminService service, LinkGenerator linkGenerator)
        {
            _service = service;
            _linkGenerator = linkGenerator;
        }

        [HttpPost]
        public ActionResult<object> AddQuiz(NewQuizDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var quiz = _service.AddQuiz(new Quiz { Title = dto.Title });

            var location = _linkGenerator.GetPathByAction(
                HttpContext, 
                nameof(GetQuiz), 
                null, 
                new { quizId = quiz.Id });

            return Created(location, quiz);
        }

        [HttpGet("{quizId}")]
        public ActionResult<Quiz> GetQuiz(int quizId)
        {
            var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            return quiz is null ? NotFound() : Ok(quiz);
        }
    }

    public class NewQuizDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }
    }
}