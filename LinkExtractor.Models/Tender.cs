using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkExtractor.Models
{
    public class Tender
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public int WorkshiftId { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Workshift Workshift { get; set; }
    }
}
