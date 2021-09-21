using API.Entities;
using API.Extensions;
using System.Linq;

namespace API.Models
{
    public class LikeResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public string PhotoUrl { get; set; }
        public string City { get; set; }

        public static LikeResponse MapFrom(AppUser user)
        {
            return new LikeResponse
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            };
        }
    }
}
