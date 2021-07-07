using LinkExtractor.DAL;
using LinkExtractor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkExtractor.UI.DataServices
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private Func<LinkExtractorDbContext> _contextCreator;

        public EmployeeDataService(Func<LinkExtractorDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            using(var context = _contextCreator())
            {
                return await context.Employees.AsNoTracking().SingleAsync(e => e.Id == employeeId);
            }
        }

        public async Task SaveAsync(Employee employee)
        {
            using(var context = _contextCreator())
            {
                context.Employees.Add(employee);
                context.Entry(employee).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        //public IEnumerable<Employee> GetAll()
        //{
        //    using(var context = _contextCreator())
        //    {
        //        return context.Employees.AsNoTracking().ToList();
        //    }
        //}
    }
}
