using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Data;
using DAS_Desafio2_Dilma8a.Models;

namespace DAS_Desafio2_Dilma8a.Controllers
{
    public class AlumnosController : Controller
    {
        private readonly AppDbContext _context;

        public AlumnosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _context.Alumnos.AsNoTracking().ToListAsync();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno)
        {
            // Validación: no permitir crear alumnos con el mismo Nombre + Apellido (case-insensitive)
            if (!string.IsNullOrWhiteSpace(alumno?.Nombre) && !string.IsNullOrWhiteSpace(alumno?.Apellido))
            {
                var nombreNormalized = alumno.Nombre.Trim().ToLower();
                var apellidoNormalized = alumno.Apellido.Trim().ToLower();
                if (await _context.Alumnos.AnyAsync(a => a.Nombre.ToLower() == nombreNormalized && a.Apellido.ToLower() == apellidoNormalized))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un alumno con ese nombre y apellido.");
                    ModelState.AddModelError("Apellido", "");
                }
            }

            // Validación: FechaNacimiento debe ser anterior a la fecha de hoy
            if (alumno != null)
            {
                // Si la fecha no es anterior a hoy (>= hoy), añadimos error
                if (alumno.FechaNacimiento >= DateTime.Today)
                {
                    ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento debe ser anterior a la fecha de hoy.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno)
        {
            if (id != alumno.AlumnoId) return NotFound();
            // Validación: no permitir que al editar quede con el mismo Nombre + Apellido que otro alumno
            if (!string.IsNullOrWhiteSpace(alumno?.Nombre) && !string.IsNullOrWhiteSpace(alumno?.Apellido))
            {
                var nombreNormalized = alumno.Nombre.Trim().ToLower();
                var apellidoNormalized = alumno.Apellido.Trim().ToLower();
                if (await _context.Alumnos.AnyAsync(a => a.Nombre.ToLower() == nombreNormalized && a.Apellido.ToLower() == apellidoNormalized && a.AlumnoId != alumno.AlumnoId))
                {
                    ModelState.AddModelError("Nombre", "Ya existe otro alumno con ese nombre y apellido.");
                    ModelState.AddModelError("Apellido", "");
                }
            }

            // Validación: FechaNacimiento debe ser anterior a la fecha de hoy
            if (alumno != null)
            {
                if (alumno.FechaNacimiento >= DateTime.Today)
                {
                    ModelState.AddModelError("FechaNacimiento", "La fecha de nacimiento debe ser anterior a la fecha de hoy.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Alumnos.AnyAsync(a => a.AlumnoId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos.AsNoTracking().FirstOrDefaultAsync(a => a.AlumnoId == id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return NotFound();
            return View(alumno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null) return RedirectToAction(nameof(Index));

            // Comprobar expedientes relacionados: si existen, no permitir eliminar
            var tieneExpedientes = await _context.Expedientes.AnyAsync(e => e.AlumnoId == id);
            if (tieneExpedientes)
            {
                // Mostrar mensaje en la misma vista de confirmación
                ModelState.AddModelError(string.Empty, "No se puede eliminar el alumno porque tiene expedientes asociados.");
                return View(alumno);
            }

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
