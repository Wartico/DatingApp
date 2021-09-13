using API.Data;
using API.Entities;
using API.Extensions;
using API.Interface;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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
        [HttpGet("{userName}", Name = "GetUser")]
        
        public async Task<ActionResult<MemberResponse>> GetUser(string userName)
        {
            return await _userRepository.GetMemberAsync(userName);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateRequest memberUpdateRequest)
        {
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());

            _mapper.Map(memberUpdateRequest, user);

            _userRepository.Update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update user.");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponse>> AddPhoto(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest("No file was passed.");
            }
            var user = await _userRepository.GetUserByUserNameAsync(User.GetUserName());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
                return CreatedAtRoute("GetUser", new { username = user.UserName },_mapper.Map<PhotoResponse>(photo));

            return BadRequest("Problem adding photo");
        }
    }
}
