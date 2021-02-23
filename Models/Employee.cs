using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {

        [Key]
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string DeptCode { get; set; }
        public Department Department { get; set; }
    }
}