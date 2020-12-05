using System;
using System.Collections.Generic;

namespace ExercisesDAL.temp
{
    public partial class Courses
    {
        public Courses()
        {
            Grades = new HashSet<Grades>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public int DivisionId { get; set; }
        public byte[] Timer { get; set; }

        public virtual Divisions Division { get; set; }
        public virtual ICollection<Grades> Grades { get; set; }
    }
}
