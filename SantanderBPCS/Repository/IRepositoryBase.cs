using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantanderBPCS.Repository
{
    public interface IRepositoryBase
    {
        Task<int> Execute(string query, object param = null);
    }
}
