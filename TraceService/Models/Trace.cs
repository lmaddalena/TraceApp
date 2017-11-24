using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Trace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TraceId { get; set; }
        public DateTime TraceDate { get; set; }
        
        [Required]
        [MaxLength(255),MinLength(3)]
        public string Origin { get; set; }
        public string Module { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public string Object { get; set; }
        public string ObjectId { get; set; }
        public string Details { get; set; }
        public string CorrelationId { get; set; }
        public TraceLevelEnum Level { get; set; }
    }

    public enum TraceLevelEnum
    {
        Information = 0,
        Warning = 1,
        Error = 2
    }
}