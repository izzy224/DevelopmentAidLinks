using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.Models
{
    public class Workshift
    {
        public Workshift()
        {
            EmployeeWorkshifts = new Collection<EmployeeWorkshift>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<EmployeeWorkshift> EmployeeWorkshifts { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}
