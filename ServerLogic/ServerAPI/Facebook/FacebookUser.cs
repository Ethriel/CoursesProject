using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerAPI.Facebook
{
    public class FacebookUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string PictureUrl { get; set; }
        public string UserId { get; set; }
    }
}
