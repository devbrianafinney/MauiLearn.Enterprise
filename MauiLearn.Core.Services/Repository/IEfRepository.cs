using MauiLearn.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Services.Repository
{
    //public interface IEntity<TModel, TKey> : IBaseEntity<TModel, TKey>
    //    where TKey : struct
    //    where TModel : class, new()
    //{}
    public interface IEfRepository<T, TKey> where T : BaseEntity<TKey>
    {
        Task<T?> GetByIdAsync(TKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id, bool? IsConcreteDelete = false);
    }
}
