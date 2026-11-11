using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ForeignCard.Common.Abstractions
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
