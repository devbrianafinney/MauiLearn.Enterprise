using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MauiLearn.Core.Models;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsDeleted { get; set; } = false;


    public string DateCreatedDisplay => DateCreated.ToLocalTime().ToString("g");
    public string DateUpdatedDisplay => DateUpdated.ToLocalTime().ToString("g");
}

