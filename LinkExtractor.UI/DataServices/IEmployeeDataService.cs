using LinkExtractor.Models;
using System.Collections.Generic;

namespace LinkExtractor.UI.DataServices
{
    public interface IEmployeeDataService
    {
        IEnumerable<Employee> GetAll();
    }
}