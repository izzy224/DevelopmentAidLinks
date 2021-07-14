using LinkExtractor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Lookups
{
    public interface IWorkshiftLookupDataService
    {
        Task<List<LookupItem>> GetWorkshiftLookupAsync();
        Task<LookupItem> GetWorkshiftLookupByDateAsync(DateTime date);
    }
}