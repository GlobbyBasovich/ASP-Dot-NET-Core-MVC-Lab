using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

#nullable disable

namespace Lab10.Models
{
    public partial class Work
    {
        public int Id { get; set; }
        public int Appliance { get; set; }
        public int Count { get; set; }
        public int Fix { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [JsonIgnore]
        public virtual Appliance ApplianceNavigation { get; set; }
        [JsonIgnore]
        public virtual Fix FixNavigation { get; set; }

        [JsonIgnore]
        public int? Cost => Count * FixNavigation.Price;
    }
}
