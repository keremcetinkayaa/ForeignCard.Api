using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Customers
{
    public class GetCustomerInfoModel
    {
        public class CustomerGetRequest : IRequest<CustomerGetResponse?>
        {
            public int CustomerId { get; set; }
        }

        public class CustomerGetResponse
        {
            public int CustomerId { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string NationalId { get; set; } = "";
            public string? PhoneNo { get; set; }
            public string? Email { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
