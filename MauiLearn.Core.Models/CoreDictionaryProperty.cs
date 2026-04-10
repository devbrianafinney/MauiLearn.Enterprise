using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MauiLearn.Core.Models
{
    public class CoreDictionaryProperty
    {
        //public string SectionName { get; init; } = "BuilderProperties";
        public required object PropertyName { get; init; }
        public required object PropertyValue { get; init; }

        public Type PropertyNameType => PropertyName.GetType();
        public Type PropertyValueType => PropertyValue.GetType();

        public CoreDictionaryProperty() { } // <!---- only for ??

        [SetsRequiredMembers]
        public CoreDictionaryProperty(object name, object value)
        {
            //SectionName = SectionName;
            PropertyName = name;
            PropertyValue = value;
        }
    }
}
