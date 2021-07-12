using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.Models
{
    public class EmployeeWorkshift
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WorkshiftId { get; set; }
        public Employee Employee { get; set; }
        public Workshift Workshift { get; set; }
    }
}
