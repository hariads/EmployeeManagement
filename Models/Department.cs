using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EmployeeManagement.Models
{
    public class Department
    {

        [Key]
        public int DeptId { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}