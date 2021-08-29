using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class EmployeeWorkshiftRepository : GenericRepository<EmployeeWorkshift, LinkExtractorDbContext>, IEmployeeWorkshiftRepository
    {
        public EmployeeWorkshiftRepository(LinkExtractorDbContext context) : base(context)
        {

        }

        public async Task<List<int>> GetEmployeesIdByWorkshiftAsync(int workshiftId)
        {
            return await Context.EmployeeWorkshifts
                .Where(e => e.WorkshiftId == workshiftId)
                .Select(e => e.EmployeeId)
                .ToListAsync();
        } 
        public async Task<EmployeeWorkshift> GetByFk(int workshiftId, int employeeId)
        {
            return await Context.EmployeeWorkshifts
                .Where(e => e.EmployeeId == employeeId && e.WorkshiftId == workshiftId)
                .FirstOrDefaultAsync();
        }

    }
}
