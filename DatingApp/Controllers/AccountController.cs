using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private DataContext _context { get; }
        private  ITokenServices _tokenServices { get; }

        public AccountController(DataContext context, ITokenServices tokenServices)
        {
            _context = context;
            _tokenServices = tokenServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                userName = registerDTO.Username,
                passwordHas = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                passwordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                UserName = user.userName,
                Token = _tokenServices.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO) {

            var user = await _context.Users.SingleOrDefaultAsync(x => x.userName == loginDTO.username.ToLower());

            if (user == null) return Unauthorized("User invalido");

            using var hmac = new HMACSHA512(user.passwordSalt);

            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));

            for (int i = 0; i < computedhash.Length; i++) {
                if (computedhash[i] != user.passwordHas[i]) return Unauthorized("user and password invalid");
            }

            return new UserDTO {
                UserName = user.userName,
                Token = _tokenServices.CreateToken(user)
            };
        }

        public async Task<bool> UserExists(string username) {

            return await _context.Users.AnyAsync(x => x.userName == username.ToLower());
        }
        
    }
}
