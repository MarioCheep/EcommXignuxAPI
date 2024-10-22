using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN
{
    public class EN_Employee
    {
        public Int64 employeeId { get; set; }
        public Int64 employeeNumber { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string userName { get; set; }
        public string passEncrypt { get; set; }
        public string name { get; set; }
        public List<EN_EmployeeRol> roles { get; set; }
    }

    public class EN_EmployeeRol
    {
        public string roleName { get; set; }
        public string category { get; set; }
        public string groupName { get; set; }
    }

}
