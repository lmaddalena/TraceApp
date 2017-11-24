using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class TraceRepository : ITraceRepository
    {
        private DataContext _dataContext;

        public TraceRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<Trace> GetAll()
        {
            var trace = from t in _dataContext.Traces
                        select t;

            return trace;
        }

        public async Task<IEnumerable<Trace>> GetAllAsync()
        {
            var trace = from t in _dataContext.Traces
                        select t;

            return await trace.ToListAsync();
        }

        public IEnumerable<Trace> GetByRange(string origin, DateTime fromDate, DateTime toDate)
        {
            var trace = from t in _dataContext.Traces
                        where t.Origin == origin && t.TraceDate >= fromDate && t.TraceDate <= toDate
                        select t;

            return trace;

        }

        public async Task<IEnumerable<Trace>> GetByRangeAsync(string origin, DateTime fromDate, DateTime toDate)
        {
            var trace = from t in _dataContext.Traces
                        where t.Origin == origin && t.TraceDate >= fromDate && t.TraceDate <= toDate
                        select t;

            return await trace.ToListAsync();

        }

        public Trace GetById(int id)
        {
            var trace = from t in _dataContext.Traces
                        where t.TraceId == id
                        select t;

            return trace.SingleOrDefault();
        }

        public async Task<Trace> GetByIdAsync(int id)
        {
            var trace = from t in _dataContext.Traces
                        where t.TraceId == id
                        select t;

            return await trace.SingleOrDefaultAsync();
        }

        public Trace Add(Trace trace)
        {

            _dataContext.Add<Trace>(trace);

            return trace;
        }

        public Trace Update(Trace trace)
        {
            var item = (from t in _dataContext.Traces
                        where t.TraceId == trace.TraceId
                        select t).SingleOrDefault();

            if(item == null)
                return null;
            else
            {
                item.CorrelationId = trace.CorrelationId;
                item.Description = trace.Description;
                item.Details = trace.Details;
                item.Level = trace.Level;
                item.Module = trace.Module;
                item.Object = trace.Object;
                item.ObjectId = trace.ObjectId;
                item.Operation = trace.Operation;
                item.Origin = trace.Origin;
                item.TraceDate = trace.TraceDate;

                _dataContext.Update(item);

                return item;
            }

        }

        public async Task<Trace> UpdateAsync(Trace trace)
        {
            var item = await (from t in _dataContext.Traces
                              where t.TraceId == trace.TraceId
                              select t).SingleOrDefaultAsync();

            if(item == null)
                return null;
            else
            {
                item.CorrelationId = trace.CorrelationId;
                item.Description = trace.Description;
                item.Details = trace.Details;
                item.Level = trace.Level;
                item.Module = trace.Module;
                item.Object = trace.Object;
                item.ObjectId = trace.ObjectId;
                item.Operation = trace.Operation;
                item.Origin = trace.Origin;
                item.TraceDate = trace.TraceDate;

                _dataContext.Update(item);

                return item;
            }

        }
        

        public bool Delete(int traceId)
        {
            var item = (from t in _dataContext.Traces
                        where t.TraceId == traceId
                        select t).SingleOrDefault();
            
            if(item == null)
                return false;
            else
            {
                _dataContext.Remove<Trace>(item);
                return true;
            }
        }

        public async Task<bool> DeleteAsync(int traceId)
        {
            var item = await (from t in _dataContext.Traces
                              where t.TraceId == traceId
                              select t).SingleOrDefaultAsync();
            
            if(item == null)
                return false;
            else
            {
                _dataContext.Remove<Trace>(item);
                return true;
            }
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }
       
        public async Task<int> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

    }
}