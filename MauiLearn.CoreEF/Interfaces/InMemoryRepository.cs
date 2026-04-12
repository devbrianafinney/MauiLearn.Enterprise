using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Repository;

namespace MauiLearn.CoreEF.Interfaces
{
    public class InMemoryRepository<TModel, TKey> : IEfRepository<TModel, TKey>
        where TModel : BaseEntity<TKey>
    {
        private readonly List<TModel> _items = new();

        public Task<IEnumerable<TModel>> GetAllAsync() => Task.FromResult(_items.AsEnumerable());

        public Task<TModel?> GetByIdAsync(TKey id) =>
            Task.FromResult(_items.FirstOrDefault(i => EqualityComparer<TKey>.Default.Equals(i.Id, id)));

        public Task AddAsync(TModel entity)
        {
            _items.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(TModel entity)
        {
            var existing = _items.FirstOrDefault(i => EqualityComparer<TKey>.Default.Equals(i.Id, entity.Id));
            if (existing != null)
            {
                _items.Remove(existing);
                _items.Add(entity);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(TKey id, bool? IsConcreteDelete)
        {
            var existing = _items.FirstOrDefault(i => EqualityComparer<TKey>.Default.Equals(i.Id, id));
            if (IsConcreteDelete == null || IsConcreteDelete == true) { if (existing != null) _items.Remove(existing); }
            if (existing != null)
            {
                existing.IsDeleted = true;
                existing.DateUpdated = DateTime.UtcNow;
            }
            return Task.CompletedTask;
        }
    }

}
