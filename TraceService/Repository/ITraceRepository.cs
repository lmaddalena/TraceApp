using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public interface ITraceRepository
    {
        IEnumerable<Trace> GetAll();
        Task<IEnumerable<Trace>> GetAllAsync();        
        Trace GetById(int id);
        Task<Trace> GetByIdAsync(int id);
        IEnumerable<Trace> GetByRange(string origin, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Trace>> GetByRangeAsync(string origin, DateTime fromDate, DateTime toDate);
        Trace Add(Trace trace);
        Trace Update(Trace trace);
        Task<Trace> UpdateAsync(Trace trace);
        bool Delete(int traceId);
        Task<bool> DeleteAsync(int traceId);
        void Save();
        Task<int> SaveAsync();
    }
}