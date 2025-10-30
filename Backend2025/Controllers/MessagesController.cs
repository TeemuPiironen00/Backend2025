using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend2025.Models;
using Backend2025.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Backend_2024.Middleware;

namespace Backend2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserAuthenticationService _userAuthenticationService;

        public MessagesController(IMessageService service, IUserAuthenticationService userAuthenticationService)
        {
            _messageService = service;
            _userAuthenticationService = userAuthenticationService;
        }

        // GET: api/Messages
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessages()
        {
            return Ok(await _messageService.GetMessagesAsync());
        }

        [HttpGet("sent/{username}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMySentMessages(string username)
        {
            if (this.User.FindFirst(ClaimTypes.Name).Value == username)
            {
                return Ok(await _messageService.GetMySentMessagesAsync(username));

            }
            return Unauthorized();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDTO>> GetMessage(long id)
        {
            var message = await _messageService.GetMessageAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMessage(long id, MessageDTO message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            string userName = this.User.FindFirst(ClaimTypes.Name).Value;
            if (! await _userAuthenticationService.IsMyMessage(userName, id))
            {
                return Unauthorized();
            }


            if (await _messageService.UpdateMessageAsync(message))
            {
                return NoContent();
            }
            return NotFound();
            
        }

        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MessageDTO>> PostMessage(MessageDTO message)
        {
            var newMessage = await _messageService.CreateMessageAsync(message);

            if (newMessage == null)
            {
                return NotFound();
            }
            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(long id)
        {
            if (await _messageService.DeleteMessageAsync(id))
            {
                return NoContent() ;

            }
            return NotFound() ;
        }

        
    }
}
