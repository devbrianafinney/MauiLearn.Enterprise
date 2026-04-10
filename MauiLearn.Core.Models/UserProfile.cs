using System;
using System.Collections.Generic;
using System.Text;

namespace MauiLearn.Core.Models
{
    // Concrete Guid-based entity
    /*
     * - Id uses set so EF can materialize entities. The User constructor 
     * assigns a Guid.NewGuid() for new instances created in code; if your 
     * DB generates the GUID (server-side), EF will overwrite it when 
     * materializing.
     */
    public class UserProfile : BaseEntity<Guid>
    {
        // Example additional properties
        public string? DisplayName { get; set; }
        public string? IntroductionMessage { get; set; }
        public string? Email { get; set; }
    }
}
