using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend2025.Models;
using Backend2025.Services;

namespace Backend2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly MessageContext _context;
        private readonly IUserService _service;

        public UsersController(IUserService userService)
        {
            _service = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            return Ok(await _service.GetUsersAsync());
        }

        // GET: api/Users/5
        [HttpGet("{username}")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            var user = await _service.GetUserAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, User user)
        {
            if (username != user.UserName)
            {
                return BadRequest("Username does not match, unlucky");
            }



            if (await _service.UpdateUserAsync(user))
            {
                return NoContent();
            }

            return NotFound();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(User user)
        {
            User? newUser = await _service.CreateUserAsync(user);
            if (newUser == null)
            {
                return BadRequest("User could not be created");
            }

            return CreatedAtAction(nameof(GetUser), new { username = newUser.UserName },newUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            if (await _service.DeleteUserAsync(username))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}