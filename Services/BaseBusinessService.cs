using AutoMapper;
using HMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HMS.Services
{
    public interface IBaseBusinessService<T1,T2>
    {
        Task<List<T2>> GetAllAsync();
        Task<T1?> GetByIdAsync(object id);
        Task<T2?> Insert(T2 model);
        Task<T2?> Edit(T2 model);
        Task<bool> Remove(object id);
    }
    public class BaseBusinessService<T1, T2> : IBaseBusinessService<T1,T2>
        where T1 : class
        where T2 : class
    {
        protected readonly IMapper _mapper;
        protected readonly HMSContext _db;
        protected readonly DbSet<T1> dbSet;
        public BaseBusinessService(IMapper mapper, HMSContext db)
        {
            _mapper = mapper;
            _db = db;
            dbSet = db.Set<T1>();
        }
        public async Task<List<T2>> GetAllAsync()
        {
            var data = await dbSet.ToListAsync();
            return _mapper.Map<List<T2>>(data);
        }
        public async Task<T1?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }
        public async virtual Task<T2?> Insert(T2 model)
        {
            try
            {
                T1 data = _mapper.Map<T1>(model);
                await dbSet.AddAsync(data);
                await _db.SaveChangesAsync();
                return _mapper.Map<T2>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async virtual Task<T2?> Edit(T2 model)
        {
            try
            {
                var id = (int)(model.GetType()?.GetProperty("Id")?.GetValue(model) ?? 0);
                var data = await GetByIdAsync(id);
                if (data == null) return null;

                var properties = model.GetType().GetProperties();
                foreach (var item in properties)
                {
                    string propertyName = item.Name;
                    var newValue = model.GetType()?.GetProperty(propertyName)?.GetValue(model);
                    data.GetType()?.GetProperty(propertyName)?.SetValue(data, newValue);
                }
                await _db.SaveChangesAsync();
                var obj = _mapper.Map<T2>(data);
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async virtual Task<bool> Remove(object id)
        {
            try
            {
                var data = await GetByIdAsync(id);
                if (data == null) return false;
                dbSet.Remove(data);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
