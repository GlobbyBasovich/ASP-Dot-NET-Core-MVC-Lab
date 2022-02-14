using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Lab10.Models
{
    public partial class Fix
    {
        public Fix()
        {
            Works = new HashSet<Work>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Price { get; set; }

        [JsonIgnore]
        public virtual ICollection<Work> Works { get; set; }
    }
}
