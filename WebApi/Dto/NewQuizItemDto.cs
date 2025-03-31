using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto
{
    public class NewQuizItemDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Pytanie musi mieć od 3 do 200 znaków.")]
        public string Question { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Muszą być co najmniej 2 opcje odpowiedzi.")]
        [MaxLength(10, ErrorMessage = "Maksymalna liczba opcji to 10.")]
        public List<string> Options { get; set; } = new();

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Indeks poprawnej odpowiedzi musi być liczbą nieujemną.")]
        public int CorrectOptionIndex { get; set; }
    }
}