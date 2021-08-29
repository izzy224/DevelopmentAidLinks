using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee, LinkExtractorDbContext>, IEmployeeRepository
    {

        public EmployeeRepository(LinkExtractorDbContext context) :base(context)
        {
        }

        public async Task<List<int>> GetAllIdAsync()
        {
            return await Context.Employees.Select(e => e.Id).ToListAsync();
        }

        public override async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await Context.Employees.SingleAsync(e => e.Id == employeeId);
        }

        public async Task<bool> HasShiftsAsync(int employeeId)
        {
            return await Context.EmployeeWorkshifts.AsNoTracking()
                //.Include(e => e.Employees)
                //.AnyAsync(e => e.Employees.Any(e => e.Id == employeeId));
                .AnyAsync(e => e.EmployeeId == employeeId);

        }
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await Context.Set<Employee>().ToListAsync();
        }
        public async Task<int> GetQuantityAsync(int id)
        {
            var x = await Context.Employees.Where(x => x.Id == id).ToListAsync();
            return x.FirstOrDefault().Quantity;
        }
    }
}

