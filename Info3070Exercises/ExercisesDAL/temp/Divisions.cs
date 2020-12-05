using System;
using System.Collections.Generic;

namespace ExercisesDAL.temp
{
    public partial class Divisions
    {
        public Divisions()
        {
            Courses = new HashSet<Courses>();
            Students = new HashSet<Students>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Timer { get; set; }

        public virtual ICollection<Courses> Courses { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
