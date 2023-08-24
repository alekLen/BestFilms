using System.ComponentModel.DataAnnotations;

namespace BestFilms.Models
{
    public class Film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "заполните")]
        public string Name { get; set; }

        [Required(ErrorMessage = "заполните")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "заполните")]  
        public string Director { get; set; }

        [Required(ErrorMessage = "заполните")]
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = " год(4 цифры)")]
        public string Year { get; set; }

        [Required(ErrorMessage = "заполните")]
        public string Story { get; set; }
        public string? Photo { get; set; }
    }
}
