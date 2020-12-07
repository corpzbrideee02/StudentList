using System;
using System.Collections.Generic;

namespace HelpdeskDAL.temp
{
    public partial class Employees
    {
        public Employees()
        {
            CallsEmployee = new HashSet<Calls>();
            CallsTech = new HashSet<Calls>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public bool? IsTech { get; set; }
        public byte[] StaffPicture { get; set; }
        public byte[] Timer { get; set; }

        public virtual Departments Department { get; set; }
        public virtual ICollection<Calls> CallsEmployee { get; set; }
        public virtual ICollection<Calls> CallsTech { get; set; }
    }
}
