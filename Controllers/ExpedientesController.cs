using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Data;
using DAS_Desafio2_Dilma8a.Models;

namespace DAS_Desafio2_Dilma8a.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly AppDbContext _context;

        public ExpedientesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expedientes = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .AsNoTracking()
                .ToListAsync();
            return View(expedientes);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["AlumnoId"] = new SelectList(await _context.Alumnos.ToListAsync(), "AlumnoId", "NombreCompleto");
            ViewData["MateriaId"] = new SelectList(await _context.Materias.ToListAsync(), "MateriaId", "NombreMateria");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el alumno ya está inscrito en la materia
                var materiaExistente = await _context.Expedientes
                    .AnyAsync(e => e.AlumnoId == expediente.AlumnoId && e.MateriaId == expediente.MateriaId);

                if (materiaExistente)
                {
                    ModelState.AddModelError(string.Empty, "El alumno ya está inscrito en esta materia.");
                }
                else
                {
                    _context.Add(expediente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AlumnoId"] = new SelectList(await _context.Alumnos.ToListAsync(), "AlumnoId", "NombreCompleto", expediente.AlumnoId);
            ViewData["MateriaId"] = new SelectList(await _context.Materias.ToListAsync(), "MateriaId", "NombreMateria", expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null) return NotFound();
            ViewData["AlumnoId"] = new SelectList(await _context.Alumnos.ToListAsync(), "AlumnoId", "NombreCompleto", expediente.AlumnoId);
            ViewData["MateriaId"] = new SelectList(await _context.Materias.ToListAsync(), "MateriaId", "NombreMateria", expediente.MateriaId);
            return View(expediente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expediente expediente)
        {
            if (id != expediente.ExpedienteId) return NotFound();
            if (ModelState.IsValid)
            {
                // Verificar si el alumno ya está inscrito en la materia (excluyendo el registro actual)
                var materiaExistente = await _context.Expedientes
                    .AnyAsync(e => e.AlumnoId == expediente.AlumnoId 
                                  && e.MateriaId == expediente.MateriaId 
                                  && e.ExpedienteId != expediente.ExpedienteId);

                if (materiaExistente)
                {
                    ModelState.AddModelError(string.Empty, "El alumno ya está inscrito en esta materia.");
                }
                else
                {
                    try
                    {
                        _context.Update(expediente);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!await _context.Expedientes.AnyAsync(e => e.ExpedienteId == id)) return NotFound();
                        throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AlumnoId"] = new SelectList(await _context.Alumnos.ToListAsync(), "AlumnoId", "NombreCompleto", expediente.AlumnoId);
            ViewData["MateriaId"] = new SelectList(await _context.Materias.ToListAsync(), "MateriaId", "NombreMateria", expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var expediente = await _context.Expedientes.Include(e => e.Alumno).Include(e => e.Materia).FirstOrDefaultAsync(e => e.ExpedienteId == id);
            if (expediente == null) return NotFound();
            return View(expediente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente != null)
            {
                _context.Expedientes.Remove(expediente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
