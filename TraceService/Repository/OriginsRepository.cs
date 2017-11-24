using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repository
{
    public class OriginsRepository : IOriginsRepository
    {
        private DataContext _dataContext;

        public OriginsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public TraceOrigin Add(TraceOrigin origin)
        {
            _dataContext.Add<TraceOrigin>(origin);
            return origin;
        }

        public bool Delete(int originId)
        {
            var item = (from o in _dataContext.TraceOrigins
                        where o.OriginId == originId
                        select o).SingleOrDefault();
            
            if(item == null)
                return false;
            else
            {
                _dataContext.Remove<TraceOrigin>(item);
                return true;
            }

        }

        public IEnumerable<TraceOrigin> GetAll()
        {
            var items = from o in _dataContext.TraceOrigins
                        orderby o.Origin
                        select o;
            
            return items;
        }

        public async Task<IEnumerable<TraceOrigin>> GetAllAsync()
        {
            var items = from o in _dataContext.TraceOrigins
                        orderby o.Origin
                        select o;
            
            return await items.ToListAsync();
        }

        public TraceOrigin GetById(int originId)
        {
            var item = from o in _dataContext.TraceOrigins
                       where o.OriginId == originId
                       select o;

            return item.SingleOrDefault();

        }

        public TraceOrigin Update(TraceOrigin origin)
        {
            var item = (from o in _dataContext.TraceOrigins
                        where o.OriginId == origin.OriginId
                        select o).SingleOrDefault();

            if(item == null)
                return null;
            else
            {
                item.Origin = origin.Origin;

                _dataContext.Update(item);
                return item;
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

        public TraceOrigin GetByName(string name)
        {
            TraceOrigin t;
            t = (from to in _dataContext.TraceOrigins
                 where to.Origin == name
                 select to).SingleOrDefault();

            return t;
        }

        public async Task<TraceOrigin> GetByNameAsync(string name)
        {
            TraceOrigin t;
            t = await (from to in _dataContext.TraceOrigins
                       where to.Origin == name
                       select to).SingleOrDefaultAsync();

            return t;
        }
        
    }
}