using API.Data;
using API.Entities;
using API.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(NewRegisterRequest newRegisterRequest)
        {

            if (await UsersExists(newRegisterRequest.UserName)) return BadRequest("Username is taken");

            var user = _mapper.Map<AppUser>(newRegisterRequest);

            using var hmac = new HMACSHA512();

            user.UserName = newRegisterRequest.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(newRegisterRequest.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users
                .Add(user);

            await _context.SaveChangesAsync();

            return new UserResponse
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .Where(x => x.UserName == loginRequest.UserName.ToLower())
                .Include(x => x.Photos)
                .SingleOrDefaultAsync();

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return new UserResponse
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            }; ;
        }

        private async Task<bool> UsersExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
