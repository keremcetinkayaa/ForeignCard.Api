using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Customers
{
    public class CreateCustomerModel
    {
        public class CustomerCreateRequest : IRequest<CustomerCreateResponse>
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string NationalId { get; set; } = "";
            public string? PhoneNo { get; set; }
            public string? Email { get; set; }
            public string Password { get; set; } = "";
        }

        public class CustomerCreateResponse
        {
            public int Id { get; set; }
        }
    }
}
