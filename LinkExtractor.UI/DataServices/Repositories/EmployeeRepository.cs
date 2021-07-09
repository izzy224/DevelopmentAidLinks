using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private LinkExtractorDbContext _context;

        public EmployeeRepository(LinkExtractorDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await _context.Employees.SingleAsync(e => e.Id == employeeId);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
