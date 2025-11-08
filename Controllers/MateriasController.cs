using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Data;
using DAS_Desafio2_Dilma8a.Models;

namespace DAS_Desafio2_Dilma8a.Controllers
{
    public class MateriasController : Controller
    {
        private readonly AppDbContext _context;

        public MateriasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Materias.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materia materia)
        {
            // Validación: no permitir crear materias con el mismo nombre (sin distinguir mayúsculas/minúsculas)
            if (!string.IsNullOrWhiteSpace(materia?.NombreMateria))
            {
                var nombreNormalized = materia.NombreMateria.Trim().ToLower();
                if (await _context.Materias.AnyAsync(m => m.NombreMateria.ToLower() == nombreNormalized))
                {
                    ModelState.AddModelError("NombreMateria", "Ya existe una materia con ese nombre.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.MateriaId) return NotFound();
            // Validación: no permitir que al editar quede con el mismo nombre que otra materia
            if (!string.IsNullOrWhiteSpace(materia?.NombreMateria))
            {
                var nombreNormalized = materia.NombreMateria.Trim().ToLower();
                if (await _context.Materias.AnyAsync(m => m.NombreMateria.ToLower() == nombreNormalized && m.MateriaId != materia.MateriaId))
                {
                    ModelState.AddModelError("NombreMateria", "Ya existe otra materia con ese nombre.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Materias.AnyAsync(m => m.MateriaId == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(materia);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return NotFound();
            return View(materia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null) return RedirectToAction(nameof(Index));

            // Comprobar expedientes relacionados: si existen, no permitir eliminar
            var tieneExpedientes = await _context.Expedientes.AnyAsync(e => e.MateriaId == id);
            if (tieneExpedientes)
            {
                ModelState.AddModelError(string.Empty, "No se puede eliminar la materia porque tiene expedientes asociados.");
                return View(materia);
            }

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
