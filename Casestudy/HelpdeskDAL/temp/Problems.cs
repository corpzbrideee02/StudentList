using System;
using System.Collections.Generic;

namespace HelpdeskDAL.temp
{
    public partial class Problems
    {
        public Problems()
        {
            Calls = new HashSet<Calls>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public byte[] Timer { get; set; }

        public virtual ICollection<Calls> Calls { get; set; }
    }
}
