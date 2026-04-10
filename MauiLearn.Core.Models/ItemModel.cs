using MauiLearn.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Models
{
    public class ItemModel : BaseEntity<Guid>
    {
        public required string Title { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public required string Details { get; set; }

        public string DateRange => $"{From:MM/dd} - {To:MM/dd}";
    }
}
