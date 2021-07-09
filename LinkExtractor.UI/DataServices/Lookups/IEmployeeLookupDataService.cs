using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Lookups
{
    public interface IEmployeeLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetEmployeeLookupAsync();
    }
}