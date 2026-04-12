using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiLearn.MultiApp.Models
{
    public class AppMenuItem
    {
        public bool IsViewAuthOnly { get; set; } = false; /* for now, any able to view must have auth */
        public bool IsViewTabOn { get; set; } = false;
        public bool IsViewFlyoutOn { get; set; } = true;
        public Type Name { get; set; } = typeof(Type);
        public Type NameModel { get; set; } = typeof(Type);

    }
}
