using System;

namespace Test
{
    public class Trace
    {
        public int TraceId { get; set; }
        public DateTime TraceDate { get; set; }      
        public string Origin { get; set; }
        public string Module { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public string Object { get; set; }
        public string ObjectId { get; set; }
        public string Details { get; set; }
        public string CorrelationId { get; set; }
        public int Level { get; set; }
    }

}
