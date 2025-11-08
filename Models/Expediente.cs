using System.ComponentModel.DataAnnotations;

namespace DAS_Desafio2_Dilma8a.Models
{
    public class Expediente
    {
        public int ExpedienteId { get; set; }

        [Required(ErrorMessage = "El campo Alumno es obligatorio")]
        public int AlumnoId { get; set; }
        public Alumno? Alumno { get; set; }

        [Required(ErrorMessage = "El campo Materia es obligatorio")]
        public int MateriaId { get; set; }
        public Materia? Materia { get; set; }

        [Required(ErrorMessage = "El campo Nota Final es obligatorio")]
        [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100")]
        [Display(Name = "Nota Final")]
        public decimal NotaFinal { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
