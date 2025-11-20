using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBooks.Data;
using MvcBooks.Models;

namespace MvcBooks.Controllers
{
    public class BooksController : Controller
    {
        private readonly BooksContext _context;
        private const int PageSize = 10;

        public BooksController(BooksContext context)
        {
            _context = context;
        }

        // GET: /Books
        public async Task<IActionResult> Index(string searchString, string genreFilter, int page = 1)
        {
            var query = _context.Books.AsQueryable();

            // Excluir eliminados
            query = query.Where(b => !b.IsDeleted);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchString) ||
                    b.Author.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(genreFilter))
            {
                query = query.Where(b => b.Genre == genreFilter);
            }

            int totalBooks = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalBooks / (double)PageSize);

            var books = await query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewData["CurrentSearch"] = searchString;
            ViewData["GenreFilter"] = genreFilter;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;
            ViewData["Genres"] = _context.Books
                .Where(b => !b.IsDeleted)
                .Select(b => b.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            ViewData["TotalBooks"] = totalBooks;

            return View(books);
        }

        // GET: /Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            if (book == null) return NotFound();

            return View(book);
        }

        // GET: /Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "📘 Libro creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsDeleted) return NotFound();

            return View(book);
        }

        // POST: /Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "✏️ Libro editado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.Id == id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: /Books/Delete/5 (confirmación)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: /Books/Delete (soft-delete, se mueve a papelera)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null && !book.IsDeleted)
            {
                book.IsDeleted = true;
                book.DeletedAt = DateTime.UtcNow;
                _context.Update(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "🗑️ Libro movido a la papelera.";
            }
            else
            {
                TempData["ErrorMessage"] = "⚠️ No se encontró el libro o ya está en la papelera.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Books/Recycle  (lista la papelera)
        public async Task<IActionResult> Recycle()
        {
            var deleted = await _context.Books
                .Where(b => b.IsDeleted)
                .OrderByDescending(b => b.DeletedAt)
                .ToListAsync();

            return View(deleted);
        }

        // POST: /Books/Restore/5   (restaurar desde papelera)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null && book.IsDeleted)
            {
                book.IsDeleted = false;
                book.DeletedAt = null;
                _context.Update(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "♻️ Libro restaurado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "⚠️ No se encontró el libro en la papelera.";
            }

            return RedirectToAction(nameof(Recycle));
        }

        // POST: /Books/PermanentDelete/5  (borra de DB)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PermanentDelete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null && book.IsDeleted)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "🗑️ Libro eliminado permanentemente.";
            }
            else
            {
                TempData["ErrorMessage"] = "⚠️ No se encontró el libro en la papelera.";
            }

            return RedirectToAction(nameof(Recycle));
        }
    }
}
