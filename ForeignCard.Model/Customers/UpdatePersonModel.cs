using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForeignCard.Model.Customers
{
    public class UpdatePersonModel 
    {
        public class UpdatePersonRequest 
        {
            public string? PhoneNo { get; set; }
            public string? Email { get; set; }
        }
    }
}
