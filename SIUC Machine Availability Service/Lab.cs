//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIUCMachineAvailabilityService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lab
    {
        public Lab()
        {
            this.Machines = new HashSet<Machine>();
        }
    
        public int lid { get; set; }
        public string lname { get; set; }
        public string lbuilding { get; set; }
        public Nullable<int> lfloor { get; set; }
        public string lroom { get; set; }
        public System.TimeSpan lopen { get; set; }
        public System.TimeSpan lclose { get; set; }
        public string lstat { get; set; }
        public System.DateTime lmdate { get; set; }
        public int mcount { get; set; }
    
        public virtual ICollection<Machine> Machines { get; set; }
    }
}
