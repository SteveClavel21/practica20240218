using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica20240218.Models;

namespace Practica20240218.Controllers
{
    public class ProductosController : Controller
    {
        private readonly prueba20240218DBContext _context;

        public ProductosController(prueba20240218DBContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index()
        {
            var prueba20240218DBContext = _context.Productos.Include(p => p.Categoria);
            return View(await prueba20240218DBContext.ToListAsync());
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre");
            return View();
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductoId,Nombre,Precio,Descripcion,CategoriaId")] Producto producto, IFormFile imagen)
        {
            if (imagen != null && imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagen.CopyToAsync(memoryStream);
                    producto.Imagen = memoryStream.ToArray();
                }
            }
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
               
            //}
            //ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            //return View(producto);8
        }

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            return View(producto);
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,Nombre,Precio,Descripcion,CategoriaId")] Producto producto, IFormFile imagen)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (imagen != null && imagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imagen.CopyToAsync(memoryStream);
                    producto.Imagen = memoryStream.ToArray();
                }
                _context.Update(producto);
                await _context.SaveChangesAsync();

            }
            else
            {
                var productFind = await _context.Productos.FirstOrDefaultAsync(s => s.ProductoId == producto.ProductoId);
                if (productFind?.Imagen?.Length > 0)
                    producto.Imagen = productFind.Imagen;
                productFind.Nombre = producto.Nombre;
                productFind.Precio = producto.Precio;
                productFind.Descripcion = producto.Descripcion;
                productFind.CategoriaId = producto.CategoriaId;

                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            try
            {
              
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.ProductoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Nombre", producto.CategoriaId);
            //return View(producto);
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'prueba20240218DBContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteImagen(int? id)
        {
            var productFind = await _context.Productos.FirstOrDefaultAsync(s => s.ProductoId == id);
            productFind.Imagen = null;
            _context.Update(productFind);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
          return (_context.Productos?.Any(e => e.ProductoId == id)).GetValueOrDefault();
        }
    }
}
