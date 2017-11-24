using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class TraceOrigin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OriginId { get; set; }

        [Required]
        [MaxLength(255),MinLength(3)]
        public string Origin { get; set; }
    }

}