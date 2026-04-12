using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Services.Validations
{
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            return value is string str &&
                   !string.IsNullOrWhiteSpace(str);
        }
    }
}
