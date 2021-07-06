using LinkExtractor.Models;
using System.Collections.Generic;

namespace LinkExtractor.UI.DataServices
{
    public class EmployeeDataService : IEmployeeDataService
    {
        public IEnumerable<Employee> GetAll()
        {
            return new List<Employee>()
            {
                new Employee() {Name = "Negura", Surname = "Constantin", Email = "example@developmentaid.org"},
                new Employee() {Name = "Bajora", Surname = "Vasile", Email = "vasea.bajora@gmail.com"},
                new Employee() {Name = "Spinu", Surname = "Anatol", Email = "example2@developmentaid.org"}
            };
        }
    }
}
