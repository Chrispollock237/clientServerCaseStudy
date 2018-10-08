namespace HelpdeskDAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Problem : HelpdeskEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Timer { get; set; }
    }
}
