using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Services.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}
