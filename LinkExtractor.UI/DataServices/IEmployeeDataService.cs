using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices
{
    public interface IEmployeeDataService
    {
        Task<List<Employee>> GetAllAsync();
    }
}