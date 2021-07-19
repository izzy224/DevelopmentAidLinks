using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class WorkshiftRepository : GenericRepository<Workshift, LinkExtractorDbContext>, IWorkshiftRepository
    {
        public WorkshiftRepository(LinkExtractorDbContext context) : base(context)
        {
        }

        public async override Task<Workshift> GetByIdAsync(int id)
        {
            return await Context.Workshifts
                //.Include(w => w.Employees)
                .SingleAsync(w => w.Id == id);
        }



    }
}
