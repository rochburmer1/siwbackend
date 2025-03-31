using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Routing;
using ApplicationCore.Interfaces.AdminService;
using ApplicationCore.Models.QuizAggregate;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using WebApi.Dto;

namespace WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/v1/admin/quizzes")]
    public class ApiQuizAdminController : ControllerBase
    {
        private readonly IQuizAdminService _service;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        public ApiQuizAdminController(IQuizAdminService service, LinkGenerator linkGenerator)
        {
            _service = service;
            _mapper = _mapper;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Tworzy nowy quiz bez pytań
        /// </summary>
        [HttpPost]
        public ActionResult<QuizDto> AddQuiz(LinkGenerator link, NewQuizDto dto)
        {
            var quiz = _service.AddQuiz(_mapper.Map<Quiz>(dto));

            return Created(
                link.GetPathByAction(HttpContext, nameof(GetQuiz), null, new { quizId = quiz.Id }),
                _mapper.Map<QuizDto>(quiz) // Mapowanie do DTO
            );
        }


        /// <summary>
        /// Pobiera quiz o podanym ID
        /// </summary>
        [HttpGet("{quizId}")]
        public ActionResult<QuizDto> GetQuiz(int quizId)
        {
            var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            return quiz is null ? NotFound() : Ok(_mapper.Map<QuizDto>(quiz));
        }

        /// <summary>
        /// Modyfikuje quiz (zmiana tytułu lub dodanie/usunięcie pytań)
        /// </summary>
        [HttpPatch("{quizId}")]
        [Consumes("application/json-patch+json")]
        public ActionResult<Quiz> UpdateQuiz(int quizId, [FromBody] JsonPatchDocument<Quiz> patchDoc, [FromServices] IValidator<QuizItemDto> validator)
        {
            var quiz = _service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId);
            if (quiz is null || patchDoc is null)
            {
                return NotFound(new { error = $"Quiz with id {quizId} not found" });
            }

            int previousCount = quiz.Items.Count;

            // Zastosowanie zmian na quizie
            patchDoc.ApplyTo(quiz, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdzenie, czy dodano nowe pytanie
            if (previousCount < quiz.Items.Count)
            {
                QuizItem newItem = quiz.Items[^1]; // Pobranie ostatniego dodanego pytania

                // Mapowanie do DTO, by sprawdzić walidację
                var newItemDto = _mapper.Map<QuizItemDto>(newItem);
                var validationResult = validator.Validate(newItemDto);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                quiz.Items.RemoveAt(quiz.Items.Count - 1); // Usunięcie go z listy quizu
                _service.AddQuizItemToQuiz(quizId, newItem); // Dodanie przez serwis
            }

            _service.UpdateQuiz(quiz); // Aktualizacja quizu w bazie

            return Ok(_service.FindAllQuizzes().FirstOrDefault(q => q.Id == quizId));
        }
    }

    /// <summary>
    /// DTO dla nowego quizu
    /// </summary>
    public class NewQuizDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; }
    }
}
