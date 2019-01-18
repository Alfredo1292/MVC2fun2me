using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class ScoreCardExperto
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "Nombre del Modelo"), Required]
        public string NombreModelo { get; set; }

        [Display(Name = "Rango Inicial"), Required]
        public string RangoInicial { get; set; }

        [Display(Name = "Rango Final"), Required]
        public string RangoFinal { get; set; }

        [Display(Name = "Puntaje"), Required]
        public int? Puntaje { get; set; }

        public string Mensaje { get; set; }
    }
}