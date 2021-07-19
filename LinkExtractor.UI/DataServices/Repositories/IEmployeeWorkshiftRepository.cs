using LinkExtractor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public interface IEmployeeWorkshiftRepository : IGenericRepository<EmployeeWorkshift>
    {
        Task<List<int>> GetEmployeesIdByWorkshiftAsync(int workshiftId);
        Task<EmployeeWorkshift> GetByFk(int workshiftId, int employeeId);
    }
}
