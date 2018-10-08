namespace HelpdeskDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee : HelpdeskEntity
    { 

        [StringLength(4)]
        public string Title { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(25)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int DepartmentId { get; set; }

        public bool? IsTech { get; set; }

        public byte[] StaffPicture { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]

        public virtual Department Department { get; set; }
    }
}
