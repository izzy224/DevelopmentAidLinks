using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public interface IEmployeeRepository:IGenericRepository<Employee>
    {
        public Task<bool> HasShiftsAsync(int employeeId);
    }
}