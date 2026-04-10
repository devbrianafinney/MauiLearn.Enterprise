using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Models
{
    public class MauiCancelToken
    {
        public required string TokenLocation { get; set; }
        public required string TokenReason { get; set; } 
        public required string TokenMessage { get; set; }
        public DateTime TokenCreated = DateTime.Now;
    }
}
