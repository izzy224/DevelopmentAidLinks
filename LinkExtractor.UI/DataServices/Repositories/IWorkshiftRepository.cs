using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public interface IWorkshiftRepository : IGenericRepository<Workshift>
    {
        Task<List<Employee>> GetAllEmployeesAsync();
    }
}