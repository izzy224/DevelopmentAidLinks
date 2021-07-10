using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
    }
}

