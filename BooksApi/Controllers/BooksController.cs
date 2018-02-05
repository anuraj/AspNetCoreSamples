using System.Linq;
using BooksApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly BooksApiDbContext _booksApiDbContext;
        public BooksController(BooksApiDbContext booksApiDbContext)
        {
            _booksApiDbContext = booksApiDbContext;
        }

        [HttpGet]
        public IQueryable<Book> GetBooks()
        {
            return _booksApiDbContext.Books;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetBook(int id)
        {
            Book book = _booksApiDbContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult PutBook(int id, [FromBody]Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            _booksApiDbContext.Entry(book).State = EntityState.Modified;

            try
            {
                _booksApiDbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostBook([FromBody]Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _booksApiDbContext.Books.Add(book);
            _booksApiDbContext.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, book);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBook(int id)
        {
            Book book = _booksApiDbContext.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            _booksApiDbContext.Books.Remove(book);
            _booksApiDbContext.SaveChanges();

            return Ok(book);
        }

        private bool BookExists(int id)
        {
            return _booksApiDbContext.Books.Any(e => e.Id == id);
        }
    }
}