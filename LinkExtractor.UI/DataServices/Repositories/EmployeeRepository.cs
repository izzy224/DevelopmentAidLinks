using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, LinkExtractorDbContext>, IEmployeeRepository
    {

        public EmployeeRepository(LinkExtractorDbContext context) :base(context)
        {
        }
        

        public override async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await Context.Employees.SingleAsync(e => e.Id == employeeId);
        }

        public async Task<bool> HasShiftsAsync(int employeeId)
        {
            return await Context.Workshifts.AsNoTracking()
                .Include(e => e.Employees)
                .AnyAsync(e => e.Employees.Any(e => e.Id == employeeId));
        }
    }
}

