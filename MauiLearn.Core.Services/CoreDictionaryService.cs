using System;
using System.Collections.Generic;
using System.Text;
using MauiLearn.Core.Models;
using MauiLearn.Core.Services.Interfaces;

namespace MauiLearn.Core.Services
{
    public class CoreDictionaryService : ICoreDictionaryService
    {
        private readonly Dictionary<string, CoreDictionaryProperty> _items = new();

        public IReadOnlyDictionary<string, CoreDictionaryProperty> Items => _items;

        public void AddOrUpdate(object name, object value)
        {
            var key = name.ToString()!;
            _items[key] = new CoreDictionaryProperty(name, value);
        }

        public bool TryGet(string name, out CoreDictionaryProperty? property)
            => _items.TryGetValue(name, out property);

        public object? GetValue(string name)
            => _items.TryGetValue(name, out var prop) ? prop.PropertyValue : null;

        public T? GetValue<T>(string name)
            => _items.TryGetValue(name, out var prop) ? (T?)prop.PropertyValue : default;

        public bool Remove(string name)
            => _items.Remove(name);
    }
}
