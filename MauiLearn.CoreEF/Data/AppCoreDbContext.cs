using MauiLearn.Core.Models;

//AppDbContext.DbSets.cs from above => using MauiLearn.CoreEF.Models;
//using MauiLearn.CoreEF.Interfaces;

using Microsoft.EntityFrameworkCore;
//using static System.Net.Mime.MediaTypeNames;

namespace MauiLearn.CoreEF.Data;
/*
 * Based foundation models, not any "outside" other projects only there will have their own
 */
public partial class CoreDbContext : DbContext
{
    public DbSet<AuthProfile> AuthProfile { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<ItemModel> ItemModel { get; set; }

}