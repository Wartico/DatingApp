using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Requests
{
    public class NewRegisterRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
