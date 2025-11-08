using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Data;

namespace DAS_Desafio2_Dilma8a.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Reportes/PromedioPorAlumno
        public async Task<IActionResult> PromedioPorAlumno()
        {
            var query = await _context.Expedientes
                .Include(e => e.Alumno)
                .AsNoTracking()
                .GroupBy(e => new { e.AlumnoId, e.Alumno!.Nombre, e.Alumno.Apellido })
                .Select(g => new
                {
                    AlumnoId = g.Key.AlumnoId,
                    NombreCompleto = g.Key.Nombre + " " + g.Key.Apellido,
                    Promedio = g.Average(x => x.NotaFinal)
                })
                .ToListAsync();

            // Map to a simple view model
            var lista = query.Select(q => new PromedioAlumnoViewModel
            {
                AlumnoId = q.AlumnoId,
                NombreCompleto = q.NombreCompleto,
                Promedio = q.Promedio
            }).ToList();

            return View(lista);
        }
    }

    public class PromedioAlumnoViewModel
    {
        public int AlumnoId { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public decimal Promedio { get; set; }
    }
}
