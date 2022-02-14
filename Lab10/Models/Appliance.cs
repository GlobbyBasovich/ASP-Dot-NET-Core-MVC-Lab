using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Lab10.Models
{
    public partial class Appliance
    {
        public Appliance()
        {
            Works = new HashSet<Work>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }

        [JsonIgnore]
        public virtual ICollection<Work> Works { get; set; }
    }
}
