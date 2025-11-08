using System.ComponentModel.DataAnnotations;

namespace DAS_Desafio2_Dilma8a.Models
{
    public class Alumno
    {
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El campo Apellido es obligatorio")]
        [StringLength(100)]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "El campo Fecha Nacimiento es obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo Grado es obligatorio")]
        [StringLength(50)]
        public string Grado { get; set; } = null!;

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
