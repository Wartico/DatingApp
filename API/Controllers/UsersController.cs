using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get the list of all users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public async Task<List<AppUser>> GetUsers()
        {

            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Get a specific users
        /// </summary>
        /// <param name="id">Id of a specific user</param>
        /// <returns>The data fro the </returns>
        [HttpGet("{id}")]
        public async Task<AppUser> GetUser(int id)
        {

            return await _context.Users.FindAsync(id);
        }
    }
}
