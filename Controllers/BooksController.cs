using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBookshelfApp.Data;
using MyBookshelfApp.Models;
using MyBookshelfApp.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MyBookshelfApp.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            var user = await GetUserAsync();
            var books = await _context.Books
                .Where(b => b.ApplicationuserId == user.Id)
                .Include(b => b.BookGenres)
                    .ThenInclude(bg => bg.Genre)
                .ToListAsync();

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new BookFormViewModel();
            var genreOptions = await _context.Genres
                .Select(g => new SelectListItem()
                {
                    Text = g.Title,
                    Value = g.Id.ToString()
                }).ToListAsync();

            viewModel.GenreOptions = genreOptions;

            return View(viewModel);
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookFormViewModel bookViewModel)
        {
            try
            {
                var user = await GetUserAsync();
                var book = new Book()
                {
                    Title = bookViewModel.Title,
                    Author = bookViewModel.Author,
                    ApplicationuserId = user.Id,
                };

                book.BookGenres = bookViewModel.GenreIds.Select(genreId => new BookGenre()
                {
                    Book = book,
                    GenreId = genreId
                }).ToList();

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var viewModel = new BookFormViewModel();
            var book = await _context.Books.Include(b => b.BookGenres).FirstOrDefaultAsync(b => b.Id == id);
            var genreOptions = await _context.Genres
                .Select(g => new SelectListItem()
                {
                    Text = g.Title,
                    Value = g.Id.ToString()
                }).ToListAsync();

            viewModel.Title = book.Title;
            viewModel.Author = book.Author;
            viewModel.GenreOptions = genreOptions;
            viewModel.GenreIds = book.BookGenres.Select(bg => bg.GenreId).ToList();

            return View(viewModel);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, BookFormViewModel bookViewModel)
        {
            try
            {
                var bookDataModel = await _context.Books.Include(b => b.BookGenres).FirstOrDefaultAsync(b => b.Id == id);

                bookDataModel.Title = bookViewModel.Title;
                bookDataModel.Author = bookViewModel.Author;
                bookDataModel.BookGenres.Clear();
                bookDataModel.BookGenres = bookViewModel.GenreIds.Select(genreId => new BookGenre()
                {
                    BookId = bookDataModel.Id,
                    GenreId = genreId
                }).ToList();

                _context.Books.Update(bookDataModel);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
        private async Task<ApplicationUser> GetUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
    }
}
