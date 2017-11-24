using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class DataContext : DbContext
    {
        public DbSet<Trace> Traces { get; set; }
        public DbSet<TraceOrigin> TraceOrigins { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite("Data Source=trace.db");
        }
        
    }
}