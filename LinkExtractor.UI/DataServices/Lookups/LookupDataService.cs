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
    public class LookupDataService : IEmployeeLookupDataService, ITeamsLookupDataService, IWorkshiftLookupDataService
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

        public async Task<IEnumerable<LookupItem>> GetTeamsLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Teams.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.Name
                    }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetWorkshiftLookupAsync()
        {
            using (var context = _contextCreator())
            {
                return await context.Workshifts.AsNoTracking()
                    .Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.Date.ToShortDateString()
                    }).ToListAsync();
            }
        }

        public async Task<LookupItem> GetWorkshiftLookupByDateAsync(DateTime date)
        {
            using(var context = _contextCreator())
            {
                return await context.Workshifts.AsNoTracking().Where(e => e.Date == date)
                    .Select(e => new LookupItem { Id = e.Id, DisplayMember = e.Date.ToShortDateString() })
                    .FirstOrDefaultAsync();
            }
        }

    }
}
