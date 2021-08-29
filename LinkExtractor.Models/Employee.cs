using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkExtractor.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeWorkshifts = new Collection<EmployeeWorkshift>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }
        [Required,MaxLength(320)]
        [ EmailAddress]
        public string Email { get; set; }
        public int Quantity { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public ICollection<EmployeeWorkshift> EmployeeWorkshifts { get; set; }
    }
}
