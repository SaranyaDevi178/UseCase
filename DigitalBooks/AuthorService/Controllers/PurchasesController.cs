using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthorService.Models;

namespace AuthorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly DigitalbooksContext _context;

        public PurchasesController(DigitalbooksContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            return await _context.Purchases.ToListAsync();
        }
        [HttpGet]
        [Route("GetBookByUserIdForReaders")]
        public async Task<ActionResult<IEnumerable<Book>>> GetReaderBooks(string emailId)
        {
            List<Book> values = new List<Book>();
            if (_context.Books == null)
            {
                return NotFound();
            }
            if (_context.Purchases.Any(x => x.EmailId == emailId))
            {
                var bookids = _context.Purchases.Where(x => x.EmailId == emailId).Select(x => x.BookId);
                return await _context.Books.Where(x => bookids.Contains(x.BookId)).ToListAsync();
                //if(bookids.Count()>0)
                //{
                //    var purchasedbooks=new List<Book>();
                //    return await _context.Book.Where(x => x.BookId==bookids);
                //}
            }
            else
            {
                return null;
            }
            return await _context.Books.ToListAsync();
        }

        [HttpGet]
        [Route("Getpurchaseeml")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchase(string eml)
        {
            if (_context.Purchases == null) { return NotFound(); }
            var purchase = await _context.Purchases.Where(x => x.EmailId == eml).ToListAsync();

            if (purchase == null)
            {
                return NotFound();

            }
            else
            {
                foreach (var p in purchase)
                {
                    p.Book = await _context.Books.Where(x => x.BookId == p.BookId).SingleOrDefaultAsync(); p.Book.Purchases = null;

                }
            }

            return purchase;
        }



        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            if (_context.Purchases == null)
            {
                return Problem("Entity set 'DigitalBooksContext.Purchases'  is null.");
            }

            var count = _context.Purchases.Where(x => x.EmailId == purchase.EmailId && x.BookId == purchase.BookId).Count();
            if (count == 0)
            {
                bool result = purchase.callPaymentAuzreFunPost();



                if (result)
                    return Ok(purchase);
                else
                    return BadRequest("Something went wrong");
            }
            else
            {
                return Problem("Purchase Already Exists");
            }
        }


        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(int id)
        {
            return (_context.Purchases?.Any(e => e.PurchaseId == id)).GetValueOrDefault();
        }
    }
}
