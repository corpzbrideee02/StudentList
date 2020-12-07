using System;
using System.Collections.Generic;

namespace HelpdeskDAL.temp
{
    public partial class Departments
    {
        public Departments()
        {
            Employees = new HashSet<Employees>();
        }

        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public byte[] Timer { get; set; }

        public virtual ICollection<Employees> Employees { get; set; }
    }
}
