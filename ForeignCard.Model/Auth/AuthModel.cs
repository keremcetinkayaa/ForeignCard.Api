using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Auth
{
    public class LoginModel
    {
        public class LoginRequest : IRequest<LoginResponse?>
        {
            public string NationalId { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class LoginResponse
        {
            public string Token { get; set; } = "";
            public int CustomerId { get; set; }
            public string FullName { get; set; } = "";
            public string Role { get; set; } = "";
        }

        public class LoginUserRow
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string? PasswordHash { get; set; }
            public string Role { get; set; } = "";
        }
    }
}
