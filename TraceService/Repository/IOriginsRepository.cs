using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Repository
{
    public interface IOriginsRepository
    {
        IEnumerable<TraceOrigin> GetAll();
        Task<IEnumerable<TraceOrigin>> GetAllAsync();
        TraceOrigin GetById(int originId);
        TraceOrigin GetByName(string name);
        Task<TraceOrigin> GetByNameAsync(string name);
        TraceOrigin Add(TraceOrigin origin);
        TraceOrigin Update(TraceOrigin origin);
        bool Delete(int originId);
        void Save();
        Task<int> SaveAsync();
        
    }

}