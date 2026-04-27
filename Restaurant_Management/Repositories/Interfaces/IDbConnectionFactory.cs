using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Restaurant_Management.Repositories.Interfaces
{
    public interface IDbConnectionFactory
    {
        SqlConnection GetConnection();   
    }
}
