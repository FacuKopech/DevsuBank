using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Persona
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(18, 99, ErrorMessage = "La edad debe estar entre 18 y 99")]
        public int Edad { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string Direccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public int Telefono { get; set; }
    }
}