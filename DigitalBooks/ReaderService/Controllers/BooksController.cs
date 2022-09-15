using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReaderService.Models;

namespace ReaderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly DigitalbooksContext _context;

        public BooksController(DigitalbooksContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("SearchBook")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBook(string BName, string Author, string Publisher, DateTime Pd)
        {
            if (_context.Books == null)
            {
                return NotFound();
            }
            var userlist = await _context.UserTables.Where(x => x.UserName.Equals(Author)).ToListAsync();
            int userId = 0;
            if (userlist.Count() > 0)
                userId = userlist.Select(x => x.UserId).FirstOrDefault();
            var book = await _context.Books.Where(x => x.BookName.Equals(BName) && x.UserId == userId && x.Publisher == Publisher && x.PublishedDate == Pd).ToListAsync();
            if (book == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var item in book)
                {
                    item.User = null;
                }
            }
            return book;
        }

        //[HttpGet]
        //[Route("Readerbook")]
        //public async Task<ActionResult<IEnumerable<Book>>> Readerbook(string emailid, string username)
        //{
        //    if (_context.Books == null)
        //    {
        //        return NotFound();
        //    }
        //    var userlist = await _context.UserTables.Where(x => x.EmailId.Equals(emailid)).ToListAsync();
        //    int userId = 0;
        //    if (userlist.Count() > 0)
        //        userId = userlist.Select(x => x.UserId).FirstOrDefault();
        //    var book = await _context.UserTables.Where(x => x.UserName.Equals(username)).ToListAsync();
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        foreach (var item in book)
        //        {
        //            item.User = null;
        //        }
        //    }
        //    return book;
        //}
        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        { 
          if (_context.Books == null|| _context.Books.Any(x=>x.Active==false))
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
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBook(int id, Book book)
        //{
        //    if (id != book.BookId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(book).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BookExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Books
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Book>> PostBook(Book book)
        //{
        //  if (_context.Books == null)
        //  {
        //      return Problem("Entity set 'DigitalbooksContext.Books'  is null.");
        //  }
        //    _context.Books.Add(book);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        //}

        //// DELETE: api/Books/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBook(int id)
        //{
        //    if (_context.Books == null)
        //    {
        //        return NotFound();
        //    }
        //    var book = await _context.Books.FindAsync(id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Books.Remove(book);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool BookExists(int id)
        {
            return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }
    }
}
