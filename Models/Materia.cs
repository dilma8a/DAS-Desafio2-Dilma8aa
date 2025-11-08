using System.ComponentModel.DataAnnotations;

namespace DAS_Desafio2_Dilma8a.Models
{
    public class Materia
    {
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "El campo Materia es obligatorio")]
        [StringLength(150)]
        [Display(Name = "Materia")]
        public string NombreMateria { get; set; } = null!;

        [Required(ErrorMessage = "El campo Docente es obligatorio")]
        [StringLength(100)] 
        public string Docente { get; set; } = null!;

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();
    }
}
