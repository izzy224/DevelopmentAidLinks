using LinkExtractor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Lookups
{
    public interface ITeamsLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetTeamsLookupAsync();
    }
}