// 2/3/26:
// From Bing Write on Phone 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace MauiLearn.Core.Models
{
    //at at Maui to use
   // public IModel<T> : INotifyPropertyChanged //raise
    public class TodoItem 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
