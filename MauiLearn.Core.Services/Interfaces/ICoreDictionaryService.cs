using System;
using System.Collections.Generic;
using System.Text;
using MauiLearn.Core.Models;

namespace MauiLearn.Core.Services.Interfaces
{
        public interface ICoreDictionaryService
    {
            void AddOrUpdate(object name, object value);
            bool TryGet(string name, out CoreDictionaryProperty? property);
            object? GetValue(string name);
            T? GetValue<T>(string name);
            bool Remove(string name);
            IReadOnlyDictionary<string, CoreDictionaryProperty> Items { get; }
        }
}
