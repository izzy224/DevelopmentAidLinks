using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Lookups
{
    public class LookupDataService : IEmployeeLookupDataService
    {
        private Func<LinkExtractorDbContext> _contextCreator;

        public LookupDataService(Func<LinkExtractorDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetEmployeeLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Employees.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.Name + ' ' + f.Surname
                    }).ToListAsync();
            }
        }
    }
}
