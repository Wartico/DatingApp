using API.Data;
using API.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberResponse>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();

            var usersToReturn = _mapper.Map<IEnumerable<MemberResponse>>(users);

            return Ok(usersToReturn);
        }

        /// <summary>
        /// Get a specific users
        /// </summary>
        /// <param name="userName">User name of a specific user</param>
        /// <returns>The data fro the </returns>
        [HttpGet("{userName}")]
        
        public async Task<ActionResult<MemberResponse>> GetUser(string userName)
        {
            return await _userRepository.GetMemberAsync(userName);
        }

    }
}
