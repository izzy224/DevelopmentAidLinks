using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByIdAsync(int employeeId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Employee employee);
        void Remove(Employee model);
    }
}