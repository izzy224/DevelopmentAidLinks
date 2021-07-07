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

        public async Task<List<Employee>> GetAllAsync()
        {
            using(var context = _contextCreator())
            {
                return await context.Employees.AsNoTracking().ToListAsync();
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
