using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Customers
{
    public class DeleteCustomerModel
    {
        public class CustomerDeleteRequest : IRequest<CustomerDeleteResponse>
        {
            public int CustomerId { get; set; }
        }

        public class CustomerDeleteResponse
        {
            public int CustomerId { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
