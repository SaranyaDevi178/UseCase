using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthorService.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize]
    public class BooksController : ControllerBase
    {
        private readonly DigitalbooksContext _context;

        public BooksController(DigitalbooksContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            return await _context.Books.ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
          if (_context.Books == null)
          {
              return NotFound();
          }
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId)
            {
                return BadRequest();
            }

         //   _context.Entry(book).State = EntityState.Modified;

            try
            {
               var DbBook= _context.Books.FirstOrDefault(x => x.BookId.Equals(id));  
               if (DbBook != null)
                { 
                DbBook.BookName = book.BookName;    
                DbBook.Price = book.Price;
                DbBook.Publisher = book.Publisher;
                DbBook.Content=book.Content;    
                DbBook.Active=book.Active;  
                await _context.SaveChangesAsync();
                  
                    return Ok(DbBook);
                }
                return NotFound("Book not found");
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

            return NoContent();
        }
        [HttpGet]
        [Route("GetBookByUserId")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            return await _context.Books.Where(x => x.UserId == id).ToListAsync();
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            book.CreatedDate = DateTime.Now;
            book.Createdby = book.UserId;
            book.ModifiedDate = DateTime.Now;
            book.Modifiedby = book.UserId;
            bool IsExist = _context.Books.Any(x => x.BookName.Equals(book.BookName));
          if (_context.Books == null || IsExist == true)
          {
              return Problem("Entity set 'DigitalbooksContext.Books'  is null.");
          }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }
        


        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
       

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
