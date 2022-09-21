using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReaderService.Models;

namespace ReaderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserTablesController : ControllerBase
    {
        private readonly DigitalbooksContext _context;

        public UserTablesController(DigitalbooksContext context)
        {
            _context = context;
        }

        // GET: api/UserTables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTable>>> GetUserTables()
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            return await _context.UserTables.ToListAsync();
        }

        // GET: api/UserTables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserTable>> GetUserTable(int id)
        {
          if (_context.UserTables == null)
          {
              return NotFound();
          }
            var userTable = await _context.UserTables.FindAsync(id);

            if (userTable == null)
            {
                return NotFound();
            }

            return userTable;
        }

        // PUT: api/UserTables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, UserTable userTable)
        {
            if (id != userTable.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userTable).State = EntityState.Modified;

            try
            {
                var DbUser = _context.UserTables.FirstOrDefault(x => x.UserId.Equals(id));

                DbUser.UserName = userTable.UserName;
                DbUser.EmailId = userTable.EmailId;
                DbUser.FirstName = userTable.FirstName;
                DbUser.LastName = userTable.LastName;
                DbUser.Active = userTable.Active;
                DbUser.Password = AuthServer.EncryptDecrypt.EncodePasswordToBase64(userTable.Password);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTableExists(id))
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

        // POST: api/UserTables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserTable>> PostUserTable(UserTable userTable)
        {
            bool IsExist = _context.UserTables.Any(x => x.UserName.Equals(userTable.UserName));
            if (_context.UserTables == null || IsExist == true)
            {
                return Problem("Entity set 'DigitalbooksContext.UserTables'  is null.");
            }
            userTable.Password = EncryptDecrypt.EncodePasswordToBase64(userTable.Password);
            _context.UserTables.Add(userTable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTable", new { id = userTable.UserId }, userTable);
        }

        // DELETE: api/UserTables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTable(int id)
        {
            if (_context.UserTables == null)
            {
                return NotFound();
            }
            var userTable = await _context.UserTables.FindAsync(id);
            if (userTable == null)
            {
                return NotFound();
            }

            _context.UserTables.Remove(userTable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserTableExists(int id)
        {
            return (_context.UserTables?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
