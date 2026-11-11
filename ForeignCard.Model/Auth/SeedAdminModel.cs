using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Auth
{
    public class SeedAdminModel
    {
        public class AdminRequest : IRequest<AdminResponse>
        {
            public string NationalId { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class AdminResponse
        {
            public bool Created { get; set; }
        }
    }
}
