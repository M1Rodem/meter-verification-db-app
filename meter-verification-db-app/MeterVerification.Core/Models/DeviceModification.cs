using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Models
{
    public class DeviceModification // Модификация СИ
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // название модификации
        public string? Designation { get; set; } // обозначение
        public int DeviceTypeId { get; set; } // ссылка на тип прибора
        public virtual DeviceType DeviceType { get; set; } = null!;
        public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();
        public override string ToString() => $"{Name} ({Designation ?? "без обозначения"})";
    }
}
