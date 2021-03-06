using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() {
            return await _context.Users.ToListAsync();
        }        
        
        [HttpGet("{id}")]
        public async Task< ActionResult<AppUser>> GetUsers(int id) {
            return await  _context.Users.FindAsync(id);
        }
    }
}
